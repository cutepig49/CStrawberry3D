using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using CStrawberry3D.Platform;
using FreeImageAPI;

namespace CStrawberry3D.TK
{
    public struct TextureDescription
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public IntPtr Pixels { get; set; }
    }
    public class TKTexture:TKAsset
    {
        public static TextureDescription GetImageDescription(string fileName)
        {
            var fif = FreeImage.GetFIFFromFilename(fileName);
            var dib = FreeImage.Load(fif, fileName, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            dib = FreeImage.ConvertTo32Bits(dib);
            return GetImageDescription(dib);
        }
        public static TextureDescription GetImageDescription(FIBITMAP dib)
        {
            var width = (int)FreeImage.GetWidth(dib);
            var height = (int)FreeImage.GetHeight(dib);
            var pixels = FreeImage.GetBits(dib);
            return new TextureDescription
            {
                Width = width,
                Height = height,
                Pixels = pixels
            };
        }
        public static TKTexture CreateFromFile(string fileName)
        {
            var fif = FreeImage.GetFIFFromFilename(fileName);
            if (!FreeImage.FIFSupportsReading(fif))
            {
                return null;
            }
            var dib = FreeImage.Load(fif, fileName, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            if (dib.IsNull)
            {
                return null;
            }
            dib = FreeImage.ConvertTo32Bits(dib);
            var desc = GetImageDescription(dib);
            var texture = TKTexture.Create();
            texture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, desc.Width, desc.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, desc.Pixels, true);
            return texture;
        }
        public static TKTexture Create()
        {
            return new TKTexture();
        }
        public static bool IsFreeImageAvailable
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
        public PixelInternalFormat PixelInternalFormat { get; private set; }
        TKTexture():base()
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
            PixelInternalFormat = internalFormat;
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
                case OpenTK.Graphics.OpenGL.PixelType.HalfFloat:
                    size = (int)(sizeof(float) * 0.5f);
                    break;
                default:
                    Logger.Error("Not supported texture format");
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
