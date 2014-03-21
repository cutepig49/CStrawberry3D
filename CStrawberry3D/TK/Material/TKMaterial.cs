using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using CStrawberry3D.Core;
using CStrawberry3D.Component;
using OpenTK.Graphics.OpenGL;
using System.Xml;
using System.IO;

namespace CStrawberry3D.TK
{
    public enum MaterialID
    {
        SkyboxMaterial,
        TexturedMaterial,
        GlobalColorMaterial
    }
    public class SceneDescription
    {
        public Matrix4 ViewMatrix { get; set; }
        public Matrix4 ProjectionMatrix { get; set; }
        public Matrix4 WorldMatrix { get; set; }
        public Vector4 AmbientLight { get; set; }
        public DirectionalLightComponent[] DirectionalLights { get; set; }
        public SceneDescription()
        {
            ViewMatrix = Matrix4.Identity;
            ProjectionMatrix = Matrix4.Identity;
            WorldMatrix = Matrix4.Identity;
            AmbientLight = new Vector4();
        }
    }
    //public class TKMaterialDescription
    //{
    //    public string MaterialName{get;set;}
    //    public string VertexShaderFileName{get;set;}
    //    public string FragmentShaderFileName{get;set;}
    //    public Dictionary<string, Vector4> Vector4Set{get;set;}
    //    public TKMaterialDescription()
    //    {
    //        MaterialName = string.Empty;
    //        VertexShaderFileName = string.Empty;
    //        FragmentShaderFileName = string.Empty;
    //        Vector4Set = new Dictionary<string,Vector4>();
    //    }
    //}
    public abstract class TKMaterial:TKAsset
    {
        //static Dictionary<string, Vector4> _ParseVector4(XmlNode vector4Dom)
        //{
        //    var vector4Set = new Dictionary<string, Vector4>();
        //    foreach (XmlElement value in vector4Dom)
        //    {
        //        if (string.IsNullOrEmpty(value.InnerText))
        //        {
        //            vector4Set[value.Name] = new Vector4();
        //        }
        //        else
        //        {
        //            var split = value.InnerText.Split(',');
        //            var v = new Vector4(Convert.ToSingle(split[0]), Convert.ToSingle(split[1]), Convert.ToSingle(split[2]), Convert.ToSingle(split[3]));
        //            vector4Set[value.Name] = v;
        //        }
        //    }
        //    return vector4Set;
        //}
        //static TKMaterialDescription[] _ParseMaterialScript(string xmlString)
        //{
        //    var root = new XmlDocument();
        //    root.LoadXml(xmlString);
        //    var descList = new List<TKMaterialDescription>();
        //    foreach (XmlElement materialDom in root.ChildNodes)
        //    {
        //        if (materialDom.Name != "Material")
        //        {
        //            continue;
        //        }

        //        var desc = new TKMaterialDescription();

        //        var materialName = materialDom.SelectSingleNode("MaterialName").InnerText;
        //        desc.MaterialName = materialName;

        //        var shaderDom = materialDom.SelectSingleNode("Shader");
        //        var vsDom = shaderDom.SelectSingleNode("VertexShader");
        //        var fsDom = shaderDom.SelectSingleNode("FragmentShader");
        //        if (vsDom != null)
        //        {
        //            var fileName = vsDom.SelectSingleNode("FileName").InnerText;

        //            var propertiesDom = vsDom.SelectSingleNode("Properties");
        //            var vector4Set = new Dictionary<string, Vector4>();
        //            if (propertiesDom != null)
        //            {
        //                var vector4Dom = propertiesDom.SelectSingleNode("Vector4");
        //                vector4Set = _ParseVector4(vector4Dom);
        //            }
        //            desc.VertexShaderFileName = fileName;
        //        }
        //        if (fsDom != null)
        //        {
        //            var fileName = fsDom.SelectSingleNode("FileName").InnerText;

        //            var propertiesDom = fsDom.SelectSingleNode("Properties");
        //            var vector4Set = new Dictionary<string, Vector4>();

        //            if (propertiesDom != null)
        //            {
        //                var vector4Dom = propertiesDom.SelectSingleNode("Vector4");
        //                vector4Set = _ParseVector4(vector4Dom);
        //            }
        //            desc.FragmentShaderFileName = fileName;
        //        }
        //        descList.Add(desc);
        //    }
        //    return descList.ToArray();
        //}
        //public static TKMaterial[] CreateFromFile(string fileName)
        //{
        //    using (var fileReader = new StreamReader(fileName))
        //    {
        //        var xmlString = fileReader.ReadToEnd();
        //        var desces = _ParseMaterialScript(xmlString);
        //        var materialList = new List<TKMaterial>();
        //        foreach (var d in desces)
        //        {
        //            materialList.Add(new TKMaterial(d));
        //        }
        //        return materialList.ToArray();
        //    }
        //}
        protected static int[] GetTextureObjects(TKTexture[] textures)
        {
            var tmp = new List<int>();
            foreach (var t in textures)
            {
                tmp.Add(t.TextureObject);
            }
            return tmp.ToArray();
        }
        protected static Vector2[] GetTextureRects(TKTexture[] textures)
        {
            var tmp = new List<Vector2>();
            foreach (var t in textures)
            {
                tmp.Add(new Vector2(t.Width, t.Height));
            }
            return tmp.ToArray();
        }
        public int NumPasses
        {
            get
            {
                return Effect.NumPasses;
            }
        }
        public TKEffect Effect { get; private set; }
        //public TKMaterialDescription Description { get; private set; }
        public string MaterialName { get; set; }

        public bool Transprent { get; set; }
        public bool CastShadow { get; set; }
        public bool ReceiveShadow { get; set; }

        public MaterialID MaterialID { get; private set; }

        protected TKMaterial(TKEffect effect, MaterialID materialID)
        {
            //var program = TKProgram.Create(desc.VertexShaderFileName, desc.FragmentShaderFileName);
            ////TODO
            //var effect = TKEffect.Create(new[] { program });
            Effect = effect;
            MaterialID = materialID;
        }
        Vector4[] _GetDirectionalLights(DirectionalLightComponent[] lights)
        {
            if (lights != null)
            {
                var tmp = new List<Vector4>();
                foreach (var l in lights)
                {
                    tmp.Add(l.DiffuseColor);
                }
                return tmp.ToArray();
            }
            else
            {
                return new Vector4[0];
            }
        }
        Vector3[] _GetDirections(DirectionalLightComponent[] lights)
        {
            if (lights != null)
            {
                var tmp = new List<Vector3>();
                foreach (var l in lights)
                {
                    tmp.Add(l.Node.Forward);
                }
                return tmp.ToArray();
            }
            else
            {
                return new Vector3[0];
            }
        }
        public virtual void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {
            var clearColor = new float[4];
            GL.GetFloat(GetPName.ColorClearValue, clearColor);
            Effect.SetUniformValue(UniformIdentifer.uClearColor, new Vector4(clearColor[0], clearColor[1], clearColor[2], clearColor[3]));
            Effect.SetUniformValue(UniformIdentifer.uPositionIndex, (int)GBufferTextureType.Position);
            Effect.SetUniformValue(UniformIdentifer.uNormalIndex, (int)GBufferTextureType.Normal);
            Effect.SetUniformValue(UniformIdentifer.uDiffuseIndex, (int)GBufferTextureType.Diffuse);
            Effect.SetUniformValue(UniformIdentifer.uDepthIndex, (int)GBufferTextureType.Depth);

            Effect.SetUniformValue(UniformIdentifer.uMVMatrix, desc.WorldMatrix);
            Effect.SetUniformValue(UniformIdentifer.uNMatrix, Mathf.TransformNormalMatrix(desc.WorldMatrix));
            Effect.SetUniformValue(UniformIdentifer.uPMatrix, desc.ProjectionMatrix);
            Effect.SetUniformValue(UniformIdentifer.uVMatrix, desc.ViewMatrix);

            Effect.SetUniformValue(UniformIdentifer.uAmbientLight, desc.AmbientLight);
            Effect.SetUniformValue(UniformIdentifer.uDirectionalLights, _GetDirectionalLights(desc.DirectionalLights));
            Effect.SetUniformValue(UniformIdentifer.uDirections, _GetDirections(desc.DirectionalLights));
            Effect.SetUniformValue(UniformIdentifer.uNumDirections, desc.DirectionalLights != null?desc.DirectionalLights.Count():0);
            Effect.SetUniformValue(UniformIdentifer.uMaterialID, (int)MaterialID);

            Effect.SetAttributeValue(AttributeIdentifer.aVertexPosition, entry.PositionBuffer);
            Effect.SetAttributeValue(AttributeIdentifer.aVertexNormal, entry.NormalBuffer);
            Effect.SetAttributeValue(AttributeIdentifer.aVertexColor, entry.ColorBuffer);
            Effect.SetAttributeValue(AttributeIdentifer.aTextureCoord, entry.TextureCoordBuffer);


            Effect.BeginPass(pass);
        }
        public virtual void Clear()
        {
            Effect.EndPass();
        }
    }
}
