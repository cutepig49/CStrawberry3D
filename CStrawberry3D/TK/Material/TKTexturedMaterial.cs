using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace CStrawberry3D.TK
{
    public class TKTexturedMaterial : TKMaterial
    {
        public static TKTexturedMaterial Create(TKShaderManager shaderManager, TKTexture texture)
        {
            return new TKTexturedMaterial(shaderManager.TexturedEffect, texture, MaterialID.TexturedMaterial);
        }

        public TKTexture Texture { get; private set; }
        TKTexturedMaterial(TKEffect effect, TKTexture texture, MaterialID materialID)
            : base(effect, materialID)
        {
            Texture = texture;
        }
        public override void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {
            Effect.SetUniformValue(UniformIdentifer.uSamplers, GetTextureObjects(new[] { Texture }));
            Effect.SetUniformValue(UniformIdentifer.uSamplerRects, GetTextureRects(new[] { Texture }));
            Effect.SetUniformValue(UniformIdentifer.uNumSamplers, 1);
            base.Apply(pass, entry, desc);
        }
    }
}
