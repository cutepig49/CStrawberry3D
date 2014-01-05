using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CStrawberry3D.core;

namespace CStrawberry3D.renderer
{
    public class RenderState
    {
        public bool useLight = false;
        public List<StrawberryNode> directionalLights = new List<StrawberryNode>();
        public List<StrawberryNode> pointLights = new List<StrawberryNode>();

        public void restore()
        {
            useLight = false;
            directionalLights.Clear();
            pointLights.Clear();
        }
    }
}
