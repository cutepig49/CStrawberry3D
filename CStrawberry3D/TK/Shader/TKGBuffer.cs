using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace CStrawberry3D.TK
{
    public enum GBufferTextureType
    {
        Position,
        Diffuse,
        Normal
    }
    public class TKGBuffer
    {
        public int FrameBufferObject { get; private set; }
        public List<TKTexture> Textures { get; private set; }
        public TKTexture DepthTexture { get; private set; }

        public TKGBuffer()
        {
            Textures = new List<TKTexture>();
            for (int i = 0; i < Enum.GetNames(typeof(GBufferTextureType)).Length; i++)
                Textures.Add(new TKTexture());

            FrameBufferObject = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferObject);

            DepthTexture = new TKTexture();

            for (var i = 0; i < Textures.Count; i++)
            {
                Textures[i].TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, TKRenderer.Singleton.FormWidth, TKRenderer.Singleton.FormHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero, false);
                GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0 + i, TextureTarget.Texture2D, Textures[i].TextureObject, 0);
            }
            DepthTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32f, TKRenderer.Singleton.FormWidth, TKRenderer.Singleton.FormHeight, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero, false);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, DepthTexture.TextureObject, 0);

            var drawBuffers = new[] { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2, DrawBuffersEnum.ColorAttachment3 };
            GL.DrawBuffers(drawBuffers.Length, drawBuffers);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                TKRenderer.Singleton.Logger.Error(string.Format("FB error, status: {0}", status));
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public void Apply()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferObject);
            TKRenderer.Singleton.Device.Clear();
        }
        public void Clear()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
