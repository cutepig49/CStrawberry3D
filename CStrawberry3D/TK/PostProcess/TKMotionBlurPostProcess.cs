using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK
{
    class MotionBlurMaterial:TKMaterial
    {
        public TKTexture ScreenTexture { get; set; }
        public static MotionBlurMaterial Create(TKShaderManager shaderManager)
        {
            return new MotionBlurMaterial(shaderManager.MotionBlurPostProcessEffect, MaterialID.SkyboxMaterial);
        }
        MotionBlurMaterial(TKEffect effect, MaterialID materialID):base(effect, materialID)
        {
            ScreenTexture = null;
        }
        public override void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {
            Effect.SetUniformValue(UniformIdentifer.uSamplers, new[] { ScreenTexture.TextureObject, TKRenderer.Singleton.LastScreen.TextureObject });
            Effect.SetUniformValue(UniformIdentifer.uNumSamplers, 2);
            base.Apply(pass, entry, desc);
        }
    }
    public class TKMotionBlurPostProcess:TKPostProcess
    {
        public static TKMotionBlurPostProcess Create(TKShaderManager shaderManager)
        {
            return new TKMotionBlurPostProcess(shaderManager);
        }
        TKMotionBlurPostProcess(TKShaderManager shaderManager):base()
        {
            _material = MotionBlurMaterial.Create(shaderManager);
        }
        public override void Apply(TKMesh screenQuad, TKTexture screenTexture)
        {
            ((MotionBlurMaterial)_material).ScreenTexture = screenTexture;
            base.Apply(screenQuad, screenTexture);
        }
    }
}
