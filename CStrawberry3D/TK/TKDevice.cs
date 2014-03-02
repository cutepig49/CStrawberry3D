using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.Windows;
using CStrawberry3D.Core;
using CStrawberry3D.Platform;

namespace CStrawberry3D.TK
{
    public class TKDevice
    {
        public static TKDevice Create()
        {
            return new TKDevice();
        }
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
        TKDevice()
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
        public void GetError(Logger logger)
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                logger.Error(error.ToString());
            }
        }
    }
}
