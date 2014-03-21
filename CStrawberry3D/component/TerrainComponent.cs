using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CStrawberry3D.TK;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using FreeImageAPI;

namespace CStrawberry3D.Component
{
    class TerrainComponent:IComponent
    {
        public static TerrainComponent Create(TKTexture hightmap, float widthFactor, float heightFactor, float lengthFactor)
        {
            return new TerrainComponent(hightmap, widthFactor, heightFactor, lengthFactor);
        }
        public TKMesh _terrainMesh;
        Bitmap _hightmapBmp;
        public TKTexture Hightmap { get; private set; }
        public float TerrainWidth{get;private set;}
        public float TerrainLength{get;private set;}
        public float WidthFactor { get; private set; }
        public float LengthFactor { get; private set; }
        public float HeightFactor { get; private set; }
        public DrawDescription DrawDesc
        {
            get
            {
                return new DrawDescription
                {
                    Entries = _terrainMesh.Entries,
                    WorldMatrix = Node.WorldMatrix
                };
            }
        }
        public TerrainComponent(TKTexture hightmap, float widthFactor, float heightFactor, float lengthFactor):base()
        {
            Name = TERRAIN_COMPONENT;
            WidthFactor = widthFactor;
            HeightFactor = heightFactor;
            LengthFactor = lengthFactor;
            Hightmap = hightmap;
            var dib = Hightmap.GenBitmap();
            _hightmapBmp = FreeImage.GetBitmap(dib);
            TerrainWidth = (_hightmapBmp.Width-1)*widthFactor;
            TerrainLength = (_hightmapBmp.Height-1)*(lengthFactor);

            var vertices = new List<Vector3>();
            for (int x = 0; x < _hightmapBmp.Width; x++)
            {
                for (int y = 0; y < _hightmapBmp.Height; y++)
                {
                    var color = _hightmapBmp.GetPixel(x, y);
                    vertices.Add(new Vector3(y*widthFactor, color.R*heightFactor, x*lengthFactor));
                }
            }
            var indices = new List<uint>();

            for (uint x = 0; x < _hightmapBmp.Width-1; x++)
            {
                for (uint y = 0; y < _hightmapBmp.Height - 1; y++)
                {
                    uint width = (uint)_hightmapBmp.Width;
                    indices.Add(x * width + y + 1);
                    indices.Add(x * width + y);
                    indices.Add((x + 1) * width + y);


                    indices.Add(x * width + y + 1);
                    indices.Add((x + 1) * width + y);
                    indices.Add((x + 1) * width + y + 1);
                }
            }

            var entry = TKMeshEntry.Create(vertices.ToArray(), indices.ToArray(), null, null, null, TKGlobalColorMaterial.Create(TKRenderer.Singleton.ShaderManager, new Vector4(1, 1, 1, 1)), new TKMeshDescription { HasVertices = true });
            _terrainMesh = TKMesh.Create(new[] { entry });
        }
    }
}
