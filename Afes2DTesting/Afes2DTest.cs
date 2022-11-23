using Afes2D.Core;
using Afes2D.Gfx;
using Afes2D.Gfx.Texture;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.Xml.Schema;

namespace Afes2DTesting {

    internal class Afes2DTest : Game {

        Animation? animation;

        Animation? bat;

        DrawingInfo drawingInfo;

        float x;
        float y;
        float r;

        public Afes2DTest() : base("Afes2D Test", new(1920, 1080)) {}

        Vector4[] pos;

        protected override void OnLoad() {

            pos = new Vector4[10000];
            for (int i = 0; i < pos.Length; i++) {
                pos[i] = new(Random.Shared.Next(1900), Random.Shared.Next(1000), 64, 64);
            }

            animation = new(new("Resources/textura_array.png", 2, 4), new(4, 2), 0.25) {
                CurrentMode = Animation.Mode.Flow
            };

            bat = new(new("Resources/bat.png", 1, 5), new(5, 1), 0.2) {
                CurrentMode = Animation.Mode.Flow
            };

        }

        protected override void OnUpdate(double elapsedTime) {

            animation?.Tick(elapsedTime);
            bat?.Tick(elapsedTime); 

            if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W)) {
                y -= 5;
            } else if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S)) {
                y += 5;
            }

            if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A)) {
                x -= 5;
            } else if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D)) {
                x += 5;
            }

            if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.G)) {
                xx -= 5;
            } else if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.H)) {
                xx += 5;
            } else if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.T)) {
                yy -= 5;
            } else if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.B)) {
                yy += 5;
            }

            if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.R)) {
                r += 0.01f;
            }

            if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.P)) {
                s += 0.01f;
            }


            if (Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.L)) {
                s -= 0.01f;
            }

        }

        float xx, yy, s = 1.0f;
        float time;

        protected override void OnRender(double elapsedTime, Renderer2D renderer) {

            if (animation == null)
                return;

            if (bat == null)
                return;

            time += (float) elapsedTime;

            renderer.CurrentCamera.ZoomIn = s;
            renderer.CurrentCamera.Rotation = r;

            renderer.CurrentCamera.Deslocation = new(xx, yy);

            renderer.Begin(animation.Texture);
            for (int i = 0; i < 192; ++i) {
                for (int j = 0; j < 108; ++j) {
                    drawingInfo.Destination = new(i * 10, j * 10, 10, 10);
                    drawingInfo.Tint = new(0.3f, 0.9f, 0.3f, 1.0f);
                    drawingInfo.SpriteIndex = 0;
                    renderer.Draw(ref drawingInfo);
                }
            }
            renderer.End();

            renderer.Begin(bat.Texture);
            for (int i = 0; i < pos.Length; ++i) {
                    drawingInfo.Destination = pos[i];
                    drawingInfo.Tint = Vector4.One;
                    drawingInfo.SpriteIndex = bat.SpriteIndex;
                    renderer.Draw(ref drawingInfo);
            }
            renderer.End();

            renderer.Begin(animation.Texture);
            drawingInfo.SpriteIndex = animation.SpriteIndex;
            drawingInfo.Tint = Vector4.One;
            drawingInfo.Destination = new(x, y, 50, 50);
            renderer.Draw(ref drawingInfo);
            renderer.End();


        }

    }
}