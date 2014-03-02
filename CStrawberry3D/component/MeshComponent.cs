using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using CStrawberry3D.TK;

namespace CStrawberry3D.Component
{
    public class MeshComponent : IComponent
    {
        public static MeshComponent Create(TKMesh mesh, TKMaterial material)
        {
            return new MeshComponent(mesh, material);
        }
        public TKMesh Mesh { get; private set; }
        public TKMaterial Material { get; private set; }
        public MeshComponent(TKMesh mesh, TKMaterial material)
            : base()
        {
            Name = MESH_COMPONENT;
            Mesh = mesh;
            Material = material;
        }
    }
}