using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using FreeImageAPI;

namespace CStrawberry3D.TK
{
    public class TKTexture:TKAsset
    {
        public bool IsAvailable
        {
            get
            {
                return FreeImage.IsAvailable();
            }
        }
        public bool IsFromFile { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TextureObject { get; private set; }
        public PixelFormat PixelFormat { get; private set; }
        public PixelType PixelType { get; private set; }
        public TKTexture()
            :base()
        {
            TextureObject = GL.GenTexture();
            Width = 0;
            Height = 0;
        }
        public void TexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, PixelFormat pixelFormat, PixelType pixelType, IntPtr pixels, bool isFromFile)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureObject);
            GL.TexImage2D(target, level, internalFormat, width, height, border, pixelFormat, pixelType, pixels);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            Width = width;
            Height = height;
            PixelFormat = pixelFormat;
            PixelType = pixelType;
            IsFromFile = isFromFile;
        }
        public bool SaveToBmp(string filePath)
        {
            var bmp = GenBitmap();
            FreeImage.Save(FREE_IMAGE_FORMAT.FIF_BMP, bmp, filePath, 0);
            bmp.SetNull();
            return true;
        }
        public bool SaveToPng(string filePath)
        {
            var bmp = GenBitmap();
            FreeImage.Save(FREE_IMAGE_FORMAT.FIF_PNG, bmp, filePath, 0);
            bmp.SetNull();
            return true;
        }
        public FIBITMAP GenBitmap()
        {
            int size;
            switch(PixelType)
            {
                case OpenTK.Graphics.OpenGL.PixelType.UnsignedByte:
                    size = sizeof(Byte);
                    break;
                case OpenTK.Graphics.OpenGL.PixelType.UnsignedInt:
                    size = sizeof(uint);
                    break;
                default:
                    size = 1;
                    break;
            }
            var pixels = new Byte[Width * Height * 4 * size];

            OpenTK.Graphics.OpenGL.PixelFormat format;
                format = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;

            GL.BindTexture(TextureTarget.Texture2D, TextureObject);
            GL.GetTexImage(TextureTarget.Texture2D, 0, format, PixelType, pixels);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            var fbitmap = FreeImage.ConvertFromRawBits(pixels, Width, Height, Width*4*size, (uint)(32*size), 0xff000000, 0x00ff0000, 0x0000ff00, false);
            return fbitmap;
        }
    }
}
