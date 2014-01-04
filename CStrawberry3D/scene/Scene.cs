using System;
using System.Collections.Generic;
using System.Text;
using CStrawberry3D.core;
using OpenTK;

namespace CStrawberry3D.scene
{
    public class Scene
    {
        private StrawberryNode _root = new StrawberryNode();
        public Scene()
        {
        }
        public StrawberryNode root
        {
            get
            {
                return _root;
            }
        }
    }
}
