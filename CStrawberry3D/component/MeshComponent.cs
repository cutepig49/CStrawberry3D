using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using CStrawberry3D.TK;

namespace CStrawberry3D.Component
{
    public class MeshComponent : EmptyComponent
    {
        public TKMesh Mesh { get; private set; }

        public MeshComponent(TKMesh mesh)
            : base()
        {
            Name = MESH_COMPONENT;
            Mesh = mesh;
        }
        public void Draw(bool isTransparentPass)
        {
            Mesh.Draw(isTransparentPass, Node.matrixWorld);
        }
    }
}