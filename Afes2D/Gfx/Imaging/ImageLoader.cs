using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;

namespace Afes2D.Gfx.Imaging {
    internal static class ImageLoader {

        public static RawImage Load(string fileName) {

            var image = Image.Load<Rgba32>(fileName);
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            Span<Rgba32> pixels = new(new Rgba32[image.Width * image.Height]);
            image.CopyPixelDataTo(pixels);

            byte[] data = new byte[image.Width * image.Height * 4];
            for (int y = 0, index = 0; y < image.Height; ++y) {
                for (int x = 0; x < image.Width; ++x) {
                    data[index++] = pixels[x + y * image.Width].R;
                    data[index++] = pixels[x + y * image.Width].G;
                    data[index++] = pixels[x + y * image.Width].B;
                    data[index++] = pixels[x + y * image.Width].A;
                }
            }

            return new RawImage(data, image.Width, image.Height);

        }

        public static RawImage[] Load(string fileName, int spritesPerColumn, int spritesPerRow) {

            RawImage[] images = new RawImage[spritesPerRow * spritesPerColumn];

            var image = Image.Load<Rgba32>(fileName);

            int spriteWidth = image.Width / spritesPerRow;
            int spriteHeight = image.Height / spritesPerColumn;

            Span<Rgba32> pixels = new(new Rgba32[spriteWidth * spritesPerRow * spriteHeight * spritesPerColumn]);
            image.CopyPixelDataTo(pixels);

            byte[] data = new byte[spriteWidth * spriteHeight * 4];

            for (int j = 0; j < spritesPerColumn; ++j) {
                for (int i = 0; i < spritesPerRow; ++i) {

                    for (int y = spriteHeight * (j + 1) - 1, index = 0; y >= spriteHeight * j; --y) {
                        for (int x = spriteWidth * (i + 1) - 1; x >= i * spriteWidth; --x) {
                            data[index++] = pixels[x + y * image.Width].R;
                            data[index++] = pixels[x + y * image.Width].G;
                            data[index++] = pixels[x + y * image.Width].B;
                            data[index++] = pixels[x + y * image.Width].A;
                        }
                    }

                    images[spritesPerRow * j + i] = new(data, spriteWidth, spriteHeight);
                    images[spritesPerRow * j + i].FlipHorizontally();

                }
            }

            return images;

        }


    }
}
