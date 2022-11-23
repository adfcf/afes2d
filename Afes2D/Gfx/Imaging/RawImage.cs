using System;

namespace Afes2D.Gfx.Imaging {
    internal sealed class RawImage {

        public static readonly int Channels = 4;

        public int Width { get; }
        public int Height { get; }
        public byte[] Data { get; private set; }

        public RawImage(byte[] data, int width, int height) {

            Data = new byte[width * height * Channels];
            
            for (int i = 0; i < Data.Length; ++i)
                Data[i] = data[i];
            
            Width = width;
            Height = height;

        }

        private void SwapPixel(int baseIndex0, int baseIndex1) {

            byte red, green, blue, alpha;

            red = Data[baseIndex0 + 0];
            green = Data[baseIndex0 + 1];
            blue = Data[baseIndex0 + 2];
            alpha = Data[baseIndex0 + 3];

            Data[baseIndex0 + 0] = Data[baseIndex1 + 0];
            Data[baseIndex0 + 1] = Data[baseIndex1 + 1];
            Data[baseIndex0 + 2] = Data[baseIndex1 + 2];
            Data[baseIndex0 + 3] = Data[baseIndex1 + 3];

            Data[baseIndex1 + 0] = red;
            Data[baseIndex1 + 1] = green;
            Data[baseIndex1 + 2] = blue;
            Data[baseIndex1 + 3] = alpha;

        }

        public void FlipHorizontally() {
            for (int x = 0; x < Width / 2; ++x) {
                for (int y = 0; y < Height; ++y) {
                    SwapPixel(ToIndex(x, y), ToIndex(Width - x - 1, y));
                }
            }
        }

        public void FlipVertically() {
            for (int x = 0; x < Width; ++x) {
                for (int y = 0; y < Height / 2; ++y) {
                    SwapPixel(ToIndex(x, y), ToIndex(x, Height - y - 1));
                }
            }
        }

        public int ToIndex(int x, int y) => (y * Width + x) * Channels;
        public int ToX(int index) => index % (Width * Channels);
        public int ToY(int index) => index / (Width * Channels);

    }
}
