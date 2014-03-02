using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace CStrawberry3D.TK
{
    public class TKVertexBuffer : IDisposable
    {
        public static TKVertexBuffer Create(int bufferObject, int itemSize, int numItems)
        {
            return new TKVertexBuffer(bufferObject, itemSize, numItems);
        }
        public int BufferObject { get; private set; }
        public int ItemSize { get; private set; }
        public int NumItems { get; private set; }
        TKVertexBuffer(int bufferObject, int itemSize, int numItems)
        {
            BufferObject = bufferObject;
            ItemSize = itemSize;
            NumItems = numItems;
        }
        public void Dispose()
        {
            GL.DeleteBuffer(BufferObject);
        }
    }
}
