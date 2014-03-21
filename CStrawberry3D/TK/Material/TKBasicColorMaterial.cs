using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace CStrawberry3D.TK
{
    public class TKBasicColorMaterial : TKMaterial
    {
        public static TKBasicColorMaterial Create(TKShaderManager shaderManager)
        {
            return new TKBasicColorMaterial(shaderManager.BasicColorEffect, MaterialID.GlobalColorMaterial);
        }
        TKBasicColorMaterial(TKEffect effect, MaterialID materialID)
            : base(effect, materialID)
        {
        }
        public override void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {
            base.Apply(pass, entry, desc);
        }
    }
}
