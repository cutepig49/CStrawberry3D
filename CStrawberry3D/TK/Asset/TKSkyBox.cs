using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK
{
    public class TKSkyBox:TKAsset
    {
        TKTexture PositiveX { get; set; }
        TKTexture NegativeX { get; set; }
        TKTexture PositiveY { get; set; }
        TKTexture NegativeY { get; set; }
        TKTexture PositiveZ { get; set; }
        TKTexture NegativeZ { get; set; }
        public TKSkyBox(TKTexture positiveX, TKTexture negativeX, TKTexture positiveY, TKTexture negativeY, TKTexture positiveZ, TKTexture negativeZ):base()
        {
            PositiveX = positiveX;
            NegativeX = negativeX;
            PositiveY = positiveY;
            NegativeY = negativeY;
            PositiveZ = positiveZ;
            NegativeZ = negativeZ;
        }
    }
}