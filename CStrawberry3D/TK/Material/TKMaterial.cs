using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using CStrawberry3D.Core;
using CStrawberry3D.Component;
using OpenTK.Graphics.OpenGL;

namespace CStrawberry3D.TK
{
    public abstract class TKMaterial
    {
        public TKEffect Effect { get; private set; }

        public bool IsTransprent { get; set; }
        public bool CastShadow { get; set; }
        public bool ReceiveShadow { get; set; }

        public TKMaterial(TKEffect effect)
        {
            Effect = effect;
        }
        Vector4[] _GetDirectionalLights(DirectionalLightComponent[] lights)
        {
            var tmp = new List<Vector4>();
            foreach (var l in lights)
            {
                tmp.Add(l.DiffuseColor);
            }
            return tmp.ToArray();
        }
        Vector3[] _GetDirections(DirectionalLightComponent[] lights)
        {
            var tmp = new List<Vector3>();
            foreach (var l in lights)
            {
                tmp.Add(l.Node.Forward);
            }
            return tmp.ToArray();
        }
        public virtual void Apply(int pass, TKEntry entry, Matrix4 mvMatrix)
        {
            var renderState = TKRenderer.Singleton.RenderState;
            var clearColor = new float[4];
            GL.GetFloat(GetPName.ColorClearValue, clearColor);
            Effect.SetUniformValue(UniformIdentifer.uClearColor, new Vector4(clearColor[0], clearColor[1], clearColor[2], clearColor[3]));
            Effect.SetUniformValue(UniformIdentifer.uPositionIndex, (int)GBufferTextureType.Position);
            Effect.SetUniformValue(UniformIdentifer.uNormalIndex, (int)GBufferTextureType.Normal);
            Effect.SetUniformValue(UniformIdentifer.uDiffuseIndex, (int)GBufferTextureType.Diffuse);

            Effect.SetUniformValue(UniformIdentifer.uMVMatrix, mvMatrix);
            Effect.SetUniformValue(UniformIdentifer.uNMatrix, Mathf.TransformNormalMatrix(mvMatrix));
            Effect.SetUniformValue(UniformIdentifer.uPMatrix, renderState.ProjectionMatrix);
            Effect.SetUniformValue(UniformIdentifer.uVMatrix, renderState.ViewMatrix);

            Effect.SetUniformValue(UniformIdentifer.uAmbientLight, renderState.AmbientLight);
            Effect.SetUniformValue(UniformIdentifer.uDirectionalLights, _GetDirectionalLights(renderState.DirectionalLights.ToArray()));
            Effect.SetUniformValue(UniformIdentifer.uDirections, _GetDirections(renderState.DirectionalLights.ToArray()));
            Effect.SetUniformValue(UniformIdentifer.uNumDirections, renderState.DirectionalLights.Count);

            Effect.SetAttributeValue(AttributeIdentifer.aVertexPosition, entry.PositionBuffer);
            Effect.SetAttributeValue(AttributeIdentifer.aVertexNormal, entry.NormalBuffer);
            Effect.SetAttributeValue(AttributeIdentifer.aVertexColor, entry.ColorBuffer);
            Effect.SetAttributeValue(AttributeIdentifer.aTextureCoord, entry.TexCoordBuffer);
            Effect.BeginPass(pass);
        }
        public virtual void Clear()
        {
            Effect.EndPass();
        }
    }

    public class GlobalColorMaterial : TKMaterial
    {
        public Vector4 GlobalColor { get; set; }
        public GlobalColorMaterial(Vector4 globalColor) :
            base(TKRenderer.Singleton.ShaderManager.GlobalColorEffect)
        {
            GlobalColor = globalColor;
        }

        public override void Apply(int pass, TKEntry entry, Matrix4 mvMatrix)
        {
            Effect.SetUniformValue(UniformIdentifer.uGlobalColor, GlobalColor);
            base.Apply(pass, entry, mvMatrix);
        }
    }

    public class BasicColorMaterial : TKMaterial
    {
        public BasicColorMaterial()
            : base(TKRenderer.Singleton.ShaderManager.BasicColorEffect)
        {
        }
        public override void Apply(int pass, TKEntry entry, Matrix4 mvMatrix)
        {
            base.Apply(pass, entry, mvMatrix);
        }
    }
    public class TexturedMaterial : TKMaterial
    {
        int[] _GetTextureObjects(TKTexture[] textures)
        {
            var tmp = new List<int>();
            foreach (var t in textures)
            {
                tmp.Add(t.TextureObject);
            }
            return tmp.ToArray();
        }

        public TKTexture Texture { get; private set; }
        public TexturedMaterial(TKTexture texture)
            : base(TKRenderer.Singleton.ShaderManager.TexturedEffect)
        {
            Texture = texture;
        }
        public override void Apply(int pass, TKEntry entry, Matrix4 mvMatrix)
        {
            Effect.SetUniformValue(UniformIdentifer.uSamplers, _GetTextureObjects(new []{Texture}));
            Effect.SetUniformValue(UniformIdentifer.uNumSamplers, 1);
            base.Apply(pass, entry, mvMatrix);
        }
    }
    public class TexturedPhongMaterial : TKMaterial
    {
        public TexturedPhongMaterial()
            : base(TKRenderer.Singleton.ShaderManager.TexturedPhongEffect)
        {
        }
        public override void Apply(int pass, TKEntry entry, Matrix4 mvMatrix)
        {
            base.Apply(pass, entry, mvMatrix);
        }
    }
    public class DeferredMaterial:TKMaterial
    {
        public DeferredMaterial()
            :base(TKRenderer.Singleton.ShaderManager.DeferredEffect)
        {

        }
        public override void Apply(int pass, TKEntry entry, Matrix4 mvMatrix)
        {
            var textures = TKRenderer.Singleton.GBuffer.Textures;

            Effect.SetUniformValue(UniformIdentifer.uDeferredPosition, textures[0].TextureObject);
            Effect.SetUniformValue(UniformIdentifer.uDeferredDiffuse, textures[1].TextureObject);
            Effect.SetUniformValue(UniformIdentifer.uDeferredNormal, textures[2].TextureObject);
            base.Apply(pass, entry, mvMatrix);
        }
    }
}
