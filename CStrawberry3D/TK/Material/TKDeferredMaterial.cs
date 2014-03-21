using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace CStrawberry3D.TK
{
    public class TKDeferredMaterial : TKMaterial
    {
        public static TKDeferredMaterial Create(TKShaderManager shaderManager, TKGBuffer gBuffer)
        {
            return new TKDeferredMaterial(shaderManager.DeferredEffect, gBuffer, MaterialID.GlobalColorMaterial);
        }
        TKGBuffer _gBuffer;
        public TKDeferredMaterial(TKEffect effect, TKGBuffer gBuffer, MaterialID materialID)
            : base(effect, MaterialID.GlobalColorMaterial)
        {
            _gBuffer = gBuffer;
        }
        public override void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {

            Effect.SetUniformValue(UniformIdentifer.uDeferredPosition, _gBuffer.PositionTexture.TextureObject);
            Effect.SetUniformValue(UniformIdentifer.uDeferredDiffuse, _gBuffer.DiffuseTexture.TextureObject);
            Effect.SetUniformValue(UniformIdentifer.uDeferredNormal, _gBuffer.NormalTexture.TextureObject);
            Effect.SetUniformValue(UniformIdentifer.uDeferredDepth, _gBuffer.DepthTexture.TextureObject);
            base.Apply(pass, entry, desc);
        }
    }
}
