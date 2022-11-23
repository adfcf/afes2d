using Afes2D.Gfx.Texture;
using OpenTK.Mathematics;

namespace Afes2D.Gfx {
    public sealed class Animation {

        public enum Mode {
            Line,
            Column,
            Flow,
            Random
        }

        public SpriteSheet Texture { get; }
        public Vector2i SpriteAmount { get; }
        public Vector2i CurrentSprite { get; set; }
        public Mode CurrentMode { get; set; }

        public double Delay { get; set; }

        public Animation(SpriteSheet texture, Vector2i spriteAmount, double delay) {

            CurrentMode = Mode.Line;

            Texture = texture;
            SpriteAmount = spriteAmount;
            Delay = delay;

        }

        public int SpriteIndex {
            get {
                return (SpriteAmount.X * CurrentSprite.Y) + CurrentSprite.X;
            }
        }

        private double timer;

        public void Tick(double elapsedTime) {
            timer += elapsedTime;
            if (timer >= Delay) {
                timer = 0;
                NextSprite();
            }
        }

        private void NextSprite() {
            switch (CurrentMode) {
                case Mode.Line:
                    CurrentSprite += Vector2i.UnitX;
                    if (CurrentSprite.X >= SpriteAmount.X) {
                        CurrentSprite = new(0, CurrentSprite.Y);
                    }
                    break;
                case Mode.Column:
                    CurrentSprite += Vector2i.UnitY;
                    if (CurrentSprite.Y >= SpriteAmount.Y) {
                        CurrentSprite = new(CurrentSprite.X, 0);
                    }
                    break;
                case Mode.Flow:
                    CurrentSprite += Vector2i.UnitX;
                    if (CurrentSprite.X >= SpriteAmount.X) {
                        CurrentSprite = new(0, CurrentSprite.Y + 1);
                        if (CurrentSprite.Y >= SpriteAmount.Y) {
                            CurrentSprite = Vector2i.Zero;
                        }
                    }
                    break;
                case Mode.Random:
                    CurrentSprite = new(Random.Shared.Next(SpriteAmount.X), Random.Shared.Next(SpriteAmount.Y));
                    break;
            }
        }

    }
}
