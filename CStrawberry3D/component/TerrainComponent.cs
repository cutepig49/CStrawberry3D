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
        public static TerrainComponent Create(TKTexture hightmap, float widthFactor, float heightFactor, float lengthFactor, uint uRepeat, uint vRepeat)
        {
            return new TerrainComponent(hightmap, widthFactor, heightFactor, lengthFactor, uRepeat, vRepeat);
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
        float _Lerp(float a,float b, float t)
        {
            return a - (a * t) + (b * t);
        }
        public float SampleHeight(float x, float z)
        {
            x /= WidthFactor;
            z /= LengthFactor;
            int row = (int)z;
            int col = (int)x;
            if (row == _hightmapBmp.Height-1)
            {
                row--;
            }
            if (col == _hightmapBmp.Width-1)
            {
                col--;
            }
            float A = Convert.ToSingle(_hightmapBmp.GetPixel(row, col).R);
            float B = Convert.ToSingle(_hightmapBmp.GetPixel(row, col + 1).R);

            float C = Convert.ToSingle(_hightmapBmp.GetPixel(row + 1, col).R);

            float D = Convert.ToSingle(_hightmapBmp.GetPixel(row + 1, col + 1).R);

            float dx = x - col;
            float dz = z - row;
            if (dz < 1.0f - dx)
            {
                float uy = B - A;
                float vy = C - A;
                return (A + _Lerp(0, uy, dx) + _Lerp(0, vy, dz)) * HeightFactor;
            }
            else
            {
                float uy = C - D;
                float vy = B - D;
                return (D + _Lerp(0, uy, 1 - dx) + _Lerp(0, vy, 1 - dz))*HeightFactor;
            }
        }
        public TerrainComponent(TKTexture hightmap, float widthFactor, float heightFactor, float lengthFactor, uint uRepeat, uint vRepeat):base()
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
            var textureCoords = new List<Vector3>();
            for (int x = 0; x < _hightmapBmp.Width; x++)
            {
                for (int y = 0; y < _hightmapBmp.Height; y++)
                {
                    var color = _hightmapBmp.GetPixel(x, y);
                    vertices.Add(new Vector3(y*widthFactor, SampleHeight(y*widthFactor, x*lengthFactor), x*lengthFactor));
                    var u = (float)y / _hightmapBmp.Height * uRepeat;
                    var v = (float)x / _hightmapBmp.Width * vRepeat;
                    u = u > 1 ? u % (int)u : u;
                    v = v > 1 ? v % (int)v : v;
                    textureCoords.Add(new Vector3(u, v, 0));
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

            var entry = TKMeshEntry.Create(vertices.ToArray(), indices.ToArray(), null, textureCoords.ToArray(), null, TKGlobalColorMaterial.Create(TKRenderer.Singleton.ShaderManager, new Vector4(1, 1, 1, 1)), new TKMeshDescription { HasVertices = true,HasTextureCoords=true });
            _terrainMesh = TKMesh.Create(new[] { entry });
        }
    }
}
