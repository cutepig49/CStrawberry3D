using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK
{
    class GreyPostProcessMaterial:TKMaterial
    {
        public TKTexture ScreenTexture { get; set; }
        public static GreyPostProcessMaterial Create(TKShaderManager shaderManager)
        {
            return new GreyPostProcessMaterial(shaderManager.GreyPostProcessEffect, MaterialID.SkyboxMaterial);
        }
        GreyPostProcessMaterial(TKEffect effect, MaterialID materialID):base(effect, materialID)
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

    public class TKGreyPostProcess:TKPostProcess
    {
        public static TKGreyPostProcess Create(TKShaderManager shaderManager)
        {
            return new TKGreyPostProcess(shaderManager);
        }
        TKGreyPostProcess(TKShaderManager shaderManager):base()
        {
            _material = GreyPostProcessMaterial.Create(shaderManager);
        }
        public override void Apply(TKMesh screenQuad, TKTexture screenTexture)
        {
            ((GreyPostProcessMaterial)_material).ScreenTexture = screenTexture;
            base.Apply(screenQuad, screenTexture);
        }
    }
}
