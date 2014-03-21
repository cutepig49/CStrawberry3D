using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using CStrawberry3D.TK;

namespace CStrawberry3D.Component
{
    public class MeshComponent : IComponent
    {
        public static MeshComponent Create(TKMesh mesh)
        {
            return new MeshComponent(mesh);
        }
        public TKMesh Mesh { get; private set; }
        public DrawDescription DrawDesc
        {
            get
            {
                return new DrawDescription
                {
                    Entries = Mesh.Entries,
                    WorldMatrix = Node.WorldMatrix
                };
            }
        }
        public MeshComponent(TKMesh mesh)
            : base()
        {
            Name = MESH_COMPONENT;
            Mesh = mesh;
        }
    }
}