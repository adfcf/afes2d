using OpenTK.Mathematics;

namespace Afes2D.Gfx {
    public sealed class Camera {

        public float ZoomIn { get; set; }
        public float Rotation { get; set; }

        public Vector2 Deslocation { get; set; }

        public Vector2 ScreenSize { get; set; }

        public Matrix4 View { 
            get {
                var view = Matrix4.Identity;
                view *= Matrix4.CreateScale(ZoomIn, ZoomIn, 1f);
                view *= Matrix4.CreateTranslation(-ScreenSize.X / 2, -ScreenSize.Y / 2, 0);
                view *= Matrix4.CreateRotationZ(Rotation);
                view *= Matrix4.CreateTranslation(ScreenSize.X / 2, ScreenSize.Y / 2, 0);
                view *= Matrix4.CreateTranslation(-Deslocation.X, -Deslocation.Y, 0);
                return view;
            }
        }

        public Camera(Vector2 screenSize) {
            ScreenSize = screenSize;
            ZoomIn = 1.0f;
        }

    }
}
