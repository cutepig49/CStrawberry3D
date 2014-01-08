using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CStrawberry3D.loader
{
    public class Texture
    {
        private int _textureObject;
        public int textureObject
        {
            get
            {
                return _textureObject;
            }
        }
        public Texture(int width, int height, IntPtr pixels)
        {
            _textureObject = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _textureObject);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public Texture(int textureObject)
        {
            _textureObject = textureObject;
        }
    }
}
