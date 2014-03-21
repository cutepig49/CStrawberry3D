using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace CStrawberry3D.TK
{
    public class TKCubeMap:TKAsset
    {
        public static TKCubeMap CreateFromFile(string fileName)
        {
            using (var fileReader = new StreamReader(fileName))
            {
                var loader = TKRenderer.Singleton.Loader;
                var xmlString = fileReader.ReadToEnd();
                var xml = new XmlDocument();
                xml.LoadXml(xmlString);
                var root = xml.FirstChild;
                var positiveXName = root.SelectSingleNode("PositiveX").InnerText;
                var negativeXName = root.SelectSingleNode("NegativeX").InnerText;
                var positiveYName = root.SelectSingleNode("PositiveY").InnerText;
                var negativeYName = root.SelectSingleNode("NegativeY").InnerText;
                var positiveZName = root.SelectSingleNode("PositiveZ").InnerText;
                var negativeZName = root.SelectSingleNode("NegativeZ").InnerText;
                return new TKCubeMap(positiveXName, negativeXName, positiveYName, negativeYName, positiveZName, negativeZName);
            }
        }
        public int CubeMapObject { get; private set; }
        TKCubeMap(string positiveXFileName, string negativeXFileName, string positiveYFileName, string negativeYFileName, string positiveZFileName, string negativeZFileName)
            : base()
        {
            CubeMapObject = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, CubeMapObject);

            var desc = TKTexture.GetImageDescription(positiveXFileName);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, PixelInternalFormat.Rgba8, desc.Width, desc.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, desc.Pixels);

            desc = TKTexture.GetImageDescription(negativeXFileName);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, PixelInternalFormat.Rgba8, desc.Width, desc.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, desc.Pixels);

            //这里为什么要上下颠倒？
            desc = TKTexture.GetImageDescription(positiveYFileName);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, PixelInternalFormat.Rgba8, desc.Width, desc.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, desc.Pixels);

            desc = TKTexture.GetImageDescription(negativeYFileName);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, PixelInternalFormat.Rgba8, desc.Width, desc.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, desc.Pixels);

            desc = TKTexture.GetImageDescription(positiveZFileName);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, PixelInternalFormat.Rgba8, desc.Width, desc.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, desc.Pixels);

            desc = TKTexture.GetImageDescription(negativeZFileName);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, PixelInternalFormat.Rgba8, desc.Width, desc.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, desc.Pixels);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        }
    }
}