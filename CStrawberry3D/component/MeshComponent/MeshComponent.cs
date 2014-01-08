using CStrawberry3D.loader;
using CStrawberry3D.shader;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace CStrawberry3D.component
{



    public class MeshComponent : Component
    {
        private Mesh _mesh;

        public MeshComponent(Mesh mesh)
            : base()
        {
            _name = Component.MESH_COMPONENT;
            _mesh = mesh;
        }
        public void draw(bool isTransparentPass, Matrix4 pMatrix, Matrix4 mvMatrix)
        {
            _mesh.draw(isTransparentPass, pMatrix, mvMatrix);
        }
    }
}