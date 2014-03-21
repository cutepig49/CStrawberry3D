using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK
{
    public abstract class TKPostProcess
    {
        protected TKMaterial _material;
        public virtual void Apply(TKMesh screenQuad, TKTexture screenTexture)
        {
            TKRenderer.Singleton.Draw(_material, screenQuad.FirstEntry, new SceneDescription());
        }
    }
}
