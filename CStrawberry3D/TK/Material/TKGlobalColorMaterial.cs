using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace CStrawberry3D.TK
{
    public class TKGlobalColorMaterial : TKMaterial
    {
        public static TKGlobalColorMaterial Create(TKShaderManager shaderManager, Vector4 globalColor)
        {
            return new TKGlobalColorMaterial(shaderManager.GlobalColorEffect, globalColor, MaterialID.GlobalColorMaterial);
        }
        public Vector4 GlobalColor { get; set; }
        public TKGlobalColorMaterial(TKEffect effect, Vector4 globalColor, MaterialID materialID) :
            base(effect, materialID)
        {
            GlobalColor = globalColor;
        }

        public override void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {
            Effect.SetUniformValue(UniformIdentifer.uGlobalColor, GlobalColor);
            base.Apply(pass, entry, desc);
        }
    }
}
