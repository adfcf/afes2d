using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using Afes2D.Gfx;

namespace Afes2D.Core {

    public abstract class Game : IDisposable {

        public static readonly double OneSecond = 1.0D;

        protected GameWindow Window { get; }
        protected Renderer2D Renderer { get; }

        public Game(string title, Vector2i windowDimension) {

            var gameWindowSettings = new GameWindowSettings {
                UpdateFrequency = 60.0,
                RenderFrequency = 60.0,
            };

            var nativeWindowSettings = new NativeWindowSettings {          
                Title = title,
                Size = windowDimension,
                StartVisible = false,
            };

            Window = new(gameWindowSettings, nativeWindowSettings);
            Renderer = new(this);

            Window.UpdateFrame += Update;
            Window.RenderFrame += Render;
            Window.Load += Load;
            Window.Unload += Unload;

        }

        protected abstract void OnLoad();
        protected abstract void OnUpdate(double deltaTime);
        protected abstract void OnRender(double deltaTime, Renderer2D renderer);
        protected virtual void OnUnload() { }

        private void Load() {

            Window.CenterWindow();

            Window.WindowBorder = WindowBorder.Fixed;
            Window.IsVisible = true;

            Renderer2D.ClearColor(new(0.3f, 0.5f, 0.8f));
            Renderer.Init();

            OnLoad();

        }

        private void Update(FrameEventArgs args) {

            if (GL.GetError() != ErrorCode.NoError) {
                Console.WriteLine(GL.GetError());
            }

            OnUpdate(args.Time);

            if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                Window.Close();

        }

        double timer;
        int frames;

        private void Render(FrameEventArgs args) {

            timer += args.Time;
            if (timer >= OneSecond) {
                Console.WriteLine("FPS: {0}", frames);
                timer = 0;
                frames = 0;
            }

            Renderer2D.Clear();
            OnRender(args.Time, Renderer);
            Window.SwapBuffers();

            ++frames;

        }

        private void Unload() {
            OnUnload();
        }

        public void Run() {
            Window.Run();
        }

        public void Dispose() {
            Window.Dispose();
            Console.WriteLine("Closing application...");
        }

        public Vector2i ViewportSize {
            get {
                return Window.ClientRectangle.Size;
            }
        }

    }
}
