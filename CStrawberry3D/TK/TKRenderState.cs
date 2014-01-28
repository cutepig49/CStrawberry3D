using CStrawberry3D.Core;
using System.Collections.Generic;
using OpenTK;
using CStrawberry3D.Component;

namespace CStrawberry3D.TK
{
    public class TKRenderState
    {
        public Matrix4 ViewMatrix { get; set; }
        public Matrix4 ProjectionMatrix { get; set; }
        public Vector4 AmbientLight{get;set;}
        public List<DirectionalLightComponent> DirectionalLights;

        public TKRenderState()
        {
            ViewMatrix = Matrix4.Identity;
            ProjectionMatrix = Matrix4.Identity;
            AmbientLight = new Vector4();
            DirectionalLights = new List<DirectionalLightComponent>();
        }
    }
}
