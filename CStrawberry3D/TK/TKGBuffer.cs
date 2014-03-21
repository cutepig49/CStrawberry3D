using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using CStrawberry3D.Platform;

namespace CStrawberry3D.TK
{
    public enum GBufferTextureType
    {
        Position,
        Diffuse,
        Normal,
        Depth
    }
    public class TKGBuffer
    {
        public static TKGBuffer Create(int formWidth, int formHeight)
        {
            return new TKGBuffer(formWidth, formHeight);
        }
        public int FrameBufferObject { get; private set; }
        public TKTexture PositionTexture { get; private set; }
        public TKTexture DiffuseTexture { get; private set; }
        public TKTexture NormalTexture { get; private set; }
        public TKTexture DepthTexture { get; private set; }
        TKTexture _internalDepthTexture;

        TKGBuffer(int formWidth, int formHeight)
        {
            FrameBufferObject = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferObject);

            PositionTexture = TKTexture.Create();
            DiffuseTexture = TKTexture.Create();
            NormalTexture = TKTexture.Create();
            DepthTexture = TKTexture.Create();
            _internalDepthTexture = TKTexture.Create();

            PositionTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, formWidth, formHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero, false);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, PositionTexture.TextureObject, 0);

            DiffuseTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, formWidth, formHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero, false);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, DiffuseTexture.TextureObject, 0);

            NormalTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, formWidth, formHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero, false);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, NormalTexture.TextureObject, 0);

            DepthTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, formWidth, formHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero, false);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment3, TextureTarget.Texture2D, DepthTexture.TextureObject, 0);

            _internalDepthTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32f, formWidth, formHeight, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero, false);
            GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, _internalDepthTexture.TextureObject, 0);

            var drawBuffers = new[] { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2, DrawBuffersEnum.ColorAttachment3 };
            GL.DrawBuffers(drawBuffers.Length, drawBuffers);

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                Logger.Error(string.Format("FB error, status: {0}", status));
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public void Begin(TKDevice device)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferObject);
            device.Clear();
        }
        public void End()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
