using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK
{
    class NegativePostProcessMaterial : TKMaterial
    {
        public TKTexture ScreenTexture { get; set; }
        public static NegativePostProcessMaterial Create(TKShaderManager shaderManager)
        {
            return new NegativePostProcessMaterial(shaderManager.NegativePostProcessEffect, MaterialID.TexturedMaterial);
        }
        NegativePostProcessMaterial(TKEffect effect, MaterialID materialID)
            : base(effect, materialID)
        {
            ScreenTexture = null;
        }
        public override void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {
            Effect.SetUniformValue(UniformIdentifer.uSamplers, new[] { ScreenTexture.TextureObject });
            Effect.SetUniformValue(UniformIdentifer.uNumSamplers, 1);
            base.Apply(pass, entry, desc);
        }
    }

    public class TKNegativePostProcess : TKPostProcess
    {
        public static TKNegativePostProcess Create(TKShaderManager shaderManager)
        {
            return new TKNegativePostProcess(shaderManager);
        }
        TKNegativePostProcess(TKShaderManager shaderManager)
        {
            _material = NegativePostProcessMaterial.Create(shaderManager);
        }
        public override void Apply(TKMesh screenQuad, TKTexture screenTexture)
        {
            ((NegativePostProcessMaterial)_material).ScreenTexture = screenTexture;
            base.Apply(screenQuad, screenTexture);
        }
    }
}
