using OpenTK.Mathematics;

namespace Afes2D.Gfx {
    public struct DrawingInfo {

        public int SpriteIndex { get; set; }
        public Vector4 Destination { get; set; }

        public float RotationAngle { get; set; }
        public Vector4 Tint { get; set; }
        public Vector2 RotationOrigin { get; set; }

        public void CenterRotationOrigin() => RotationOrigin = new(Destination.Z / 2, Destination.W / 2);

    }
}
