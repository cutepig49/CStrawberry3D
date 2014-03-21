using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK
{
    class TKScreenMaterial:TKMaterial
    {
             public static TKScreenMaterial Create(TKShaderManager shaderManager, TKTexture texture)
        {
            return new TKScreenMaterial(shaderManager.ScreenEffect, texture, MaterialID.TexturedMaterial);
        }
        public TKTexture Texture { get; set; }
        TKScreenMaterial(TKEffect effect, TKTexture texture, MaterialID materialID)
            : base(effect, materialID)
        {
            Texture = texture;
        }
        public override void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {
            Effect.SetUniformValue(UniformIdentifer.uSamplers, new[]{ Texture.TextureObject});
            Effect.SetUniformValue(UniformIdentifer.uNumSamplers, 1);
            base.Apply(pass, entry, desc);
        }
    }
}
