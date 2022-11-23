using Afes2D.Core;
using Afes2D.Gfx.Model;
using Afes2D.Gfx.Shader;
using Afes2D.Gfx.Texture;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Afes2D.Gfx {

    public sealed class Renderer2D {

        public static readonly int BatchCapacity = 65536;

        public Camera CurrentCamera { get; }

        TexturedBatchProgram TBP { get; }

        RectBatch Batch { get; }
        Game Instance { get; }

        public bool FlipHorizontally { get; set; }
        public bool FlipVertically { get; set; }

        public Renderer2D(Game instance) {

            Instance = instance;
  
            CurrentCamera = new(Instance.ViewportSize);
            TBP = new();
            Batch = new(BatchCapacity);

        }

        public void Init() {
            TBP.Init();
            Batch.Init();
        }

        public void Begin(SpriteSheet spriteSheet) {

            TBP.Use();

            TBP.SetProjection(Instance.ViewportSize.X, Instance.ViewportSize.Y);
            TBP.SetCamera(CurrentCamera);

            spriteSheet.Use();

        }

        public void End() {
            Batch.Flush();
            Batch.Draw();
        }

        public void Draw(ref DrawingInfo drawingInfo) {

            float verticalFlipDeslocation = 0f;
            float horizontalFlipDeslocation = 0f;

            float verticalFlipScale = 1f;
            float horizontalFlipScale = 1f;

            if (FlipHorizontally) {
                horizontalFlipDeslocation = drawingInfo.Destination.Z;
                horizontalFlipScale = -1f;
            }

            if (FlipVertically) {
                verticalFlipDeslocation = drawingInfo.Destination.W;
                verticalFlipScale = -1f;
            }

            var translation = Matrix4.CreateTranslation(drawingInfo.Destination.X + horizontalFlipDeslocation, drawingInfo.Destination.Y + verticalFlipDeslocation, 0);
            var preTranslation = Matrix4.CreateTranslation(-drawingInfo.RotationOrigin.X, -drawingInfo.RotationOrigin.Y, 0);
            var postTranslation = Matrix4.CreateTranslation(drawingInfo.RotationOrigin.X, drawingInfo.RotationOrigin.Y, 0);
            var scale = Matrix4.CreateScale(horizontalFlipScale * drawingInfo.Destination.Z, verticalFlipScale * drawingInfo.Destination.W, 1f);
            var rotation = Matrix4.CreateRotationZ(drawingInfo.RotationAngle);

            var transform = Matrix4.Identity;
            transform *= scale;
            transform *= preTranslation;
            transform *= rotation;
            transform *= postTranslation;
            transform *= translation;

            Batch.Push(transform, drawingInfo.Tint, drawingInfo.SpriteIndex, 0);

        }

        public static void ClearColor(Vector3 color) => GL.ClearColor(color.X, color.Y, color.Z, 1.0f);
       
        public static void Clear() => GL.Clear(ClearBufferMask.ColorBufferBit);
      

    }
}
