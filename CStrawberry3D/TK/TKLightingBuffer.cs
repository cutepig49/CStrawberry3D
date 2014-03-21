using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using CStrawberry3D.Platform;

namespace CStrawberry3D.TK
{
    public class TKLightingBuffer
    {
        public static TKLightingBuffer Create(int formWidth, int formHeight, TKMesh screenQuad)
        {
            return new TKLightingBuffer(formWidth, formHeight, screenQuad);
        }
        public TKTexture LightedTexture { get; private set; }
        public int FrameBufferObject { get; private set; }
        TKMesh _screenQuad;
        TKLightingBuffer(int formWidth, int formHeight, TKMesh screenQuad)
        {
            _screenQuad = screenQuad;

            LightedTexture = TKTexture.Create();
            FrameBufferObject = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferObject);

            LightedTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16, formWidth, formHeight, 0, PixelFormat.Rgba, PixelType.HalfFloat, IntPtr.Zero, false);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, LightedTexture.TextureObject, 0);
            GL.DrawBuffers(1, new []{DrawBuffersEnum.ColorAttachment0});

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                Logger.Error(string.Format("LightingBuffer error, status: {0}", status));
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public void Apply(TKDevice device, SceneDescription sceneDesc)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferObject);
            device.Clear();

            TKRenderer.Singleton.Draw(_screenQuad.FirstEntry.Material, _screenQuad.FirstEntry, sceneDesc);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
