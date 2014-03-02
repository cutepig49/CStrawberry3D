using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK
{
    public class TKAsset
    {
        public string Guid { get; private set; }
        protected TKAsset()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}
