using Afes2D.Gfx.Imaging;
using OpenTK.Graphics.OpenGL;

namespace Afes2D.Gfx.Texture
{
    public sealed class SpriteSheet : ITexture {

        int Id { get; set; }

        public SpriteSheet(string fileName, int spritesPerColumn, int spritesPerRow) {

            var images = ImageLoader.Load(fileName, spritesPerColumn, spritesPerRow);

            int spriteWidth = images[0].Width;
            int spriteHeight = images[0].Height;

            GL.ActiveTexture(TextureUnit.Texture0);

            Id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DArray, Id);

            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba, spriteWidth, spriteHeight, images.Length, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            for (int i = 0; i < images.Length; ++i)
                GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, i, spriteWidth, spriteHeight, 1, PixelFormat.Rgba, PixelType.UnsignedByte, images[i].Data);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
            GL.BindTexture(TextureTarget.Texture2DArray, 0);

        }

        public SpriteSheet(string fileName) : this(fileName, 1, 1) { }

        public void Use() => GL.BindTexture(TextureTarget.Texture2DArray, Id);

        public void Delete() {
            GL.DeleteTexture(Id);
            Id = 0;
        }

    }
}
