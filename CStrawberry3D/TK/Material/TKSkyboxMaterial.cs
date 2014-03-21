using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK
{
    public class TKSkyboxMaterial : TKMaterial
    {
        public static TKSkyboxMaterial Create(TKShaderManager shaderManager, TKCubeMap texture)
        {
            return new TKSkyboxMaterial(shaderManager.SkyboxEffect, texture, MaterialID.SkyboxMaterial);
        }
        public TKCubeMap CubeMap { get; private set; }

        TKSkyboxMaterial(TKEffect effect, TKCubeMap cubeMap, MaterialID materialID)
            : base(effect, materialID)
        {
            CubeMap = cubeMap;
        }
        public override void Apply(int pass, TKMeshEntry entry, SceneDescription desc)
        {
            Effect.SetUniformValue(UniformIdentifer.uCubeSamplers, new int[] { CubeMap.CubeMapObject });
            base.Apply(pass, entry, desc);
        }
    }
}
