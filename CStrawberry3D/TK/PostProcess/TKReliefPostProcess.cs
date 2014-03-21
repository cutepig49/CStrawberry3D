using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK.PostProcess
{
    class ReliefPostProcessMaterial:TKMaterial
    {
        public TKTexture ScreenTexture { get; set; }
        public static ReliefPostProcessMaterial Create(TKShaderManager shaderManager)
        {
            return new ReliefPostProcessMaterial(shaderManager.ReliefPostProcessEffect, MaterialID.SkyboxMaterial);
        }
        ReliefPostProcessMaterial(TKEffect effect, MaterialID materialID):base(effect, materialID)
        {
            ScreenTexture = null;
        }
        public override void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {
            Effect.SetUniformValue(UniformIdentifer.uSamplers, GetTextureObjects(new[] { ScreenTexture }));
            Effect.SetUniformValue(UniformIdentifer.uSamplerRects, GetTextureRects(new[] { ScreenTexture }));
            Effect.SetUniformValue(UniformIdentifer.uNumSamplers, 1);
            base.Apply(pass, entry, desc);
        }
    }
    public class TKReliefPostProcess:TKPostProcess
    {
        public static TKReliefPostProcess Create(TKShaderManager shaderManager)
        {
            return new TKReliefPostProcess(shaderManager);
        }
        TKReliefPostProcess(TKShaderManager shaderManager)
        {
            _material = ReliefPostProcessMaterial.Create(shaderManager);
        }
        public override void Apply(TKMesh screenQuad, TKTexture screenTexture)
        {
            ((ReliefPostProcessMaterial)_material).ScreenTexture = screenTexture;
            base.Apply(screenQuad, screenTexture);
        }
    }
}
