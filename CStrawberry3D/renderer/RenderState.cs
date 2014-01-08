using CStrawberry3D.core;
using System.Collections.Generic;
using OpenTK;

namespace CStrawberry3D.renderer
{
    public class RenderState
    {

        public List<Vector4> directionalLights = new List<Vector4>();
        public List<Vector3> directions = new List<Vector3>();
        public List<StrawberryNode> pointLights = new List<StrawberryNode>();
        public Matrix4 viewMatrix;
        public Vector4 ambientLight;

        public void ready(Matrix4 viewMatrix, Vector4 ambientColor)
        {
            this.viewMatrix = viewMatrix;
            this.ambientLight = ambientColor;
        }

        public void restore()
        {
            viewMatrix = new Matrix4();
            directionalLights.Clear();
            directions.Clear();
            pointLights.Clear();
            ambientLight = new Vector4();
        }
    }
}
