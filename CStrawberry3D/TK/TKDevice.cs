using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.Windows;
using CStrawberry3D.Core;

namespace CStrawberry3D.TK
{
    public class TKDevice
    {
        Vector4 _clearColor;
        public Vector4 ClearColor
        {
            get
            {
                return _clearColor;
            }
            set
            {
                GL.ClearColor(value.X, value.Y, value.Z, value.W);
                _clearColor = value;
            }
        }
        public string AdapterName
        {
            get
            {
                return GL.GetString(StringName.Renderer);
            }
        }
        public TKDevice()
        {
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            ClearColor = new Vector4(0, 0, 0, 1);
        }
        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        public void GetError()
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                TKRenderer.Singleton.Logger.Error(error.ToString());
            }
        }
    }
}
