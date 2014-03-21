using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using CStrawberry3D.Platform;

namespace CStrawberry3D.TK
{
    public class TKPostProcessManager
    {
        public static TKPostProcessManager Create(int formWidth, int formHeight, TKMesh screenQuad)
        {
            return new TKPostProcessManager(formWidth, formHeight, screenQuad);
        }
        public TKTexture tmpTexture { get; private set; }
        public TKTexture HandledTexture { get; private set; }
        TKTexture _handledTexture;
        public int FrameBufferObject { get; private set; }
        public List<TKPostProcess> PostProcessList { get; private set; }
        TKMesh _screenQuad;
        TKTexture _internalDepthTexture;
        TKPostProcessManager(int formWidth, int formHeight, TKMesh screenQuad)
        {
            PostProcessList = new List<TKPostProcess>();
            _screenQuad = screenQuad;

            tmpTexture = TKTexture.Create();
            HandledTexture = TKTexture.Create();
            _internalDepthTexture = TKTexture.Create();
            FrameBufferObject = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferObject);

            tmpTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, formWidth, formHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero, false);

            HandledTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, formWidth, formHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero, false);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, HandledTexture.TextureObject, 0);

            _internalDepthTexture.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32f, formWidth, formHeight, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero, false);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, _internalDepthTexture.TextureObject, 0);

            GL.DrawBuffers(1, new[] { DrawBuffersEnum.ColorAttachment0 });

            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                Logger.Error(string.Format("PostProcessManager error, status: {0}", status));
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            _handledTexture = HandledTexture;
        }
        public void Push(TKPostProcess postProcess)
        {
            PostProcessList.Add(postProcess);
        }
        public void Pop()
        {
            if (PostProcessList.Count != 0)
            {
                PostProcessList.RemoveAt(PostProcessList.Count - 1);
            }
        }
        public void Apply(TKDevice device, TKTexture texture)
        {
            HandledTexture = _handledTexture;


            if (PostProcessList.Count > 0)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferObject);
                var currTexture = texture;
                //GL.Enable(EnableCap.PolygonOffsetFill);
                for (int i = 0; i < PostProcessList.Count; i++)
                {
                    device.Clear();
                    GL.PolygonOffset(i * -0.1f, 1);
                    PostProcessList[i].Apply(_screenQuad, currTexture);
                    GL.BindTexture(TextureTarget.Texture2D, tmpTexture.TextureObject);
                    GL.CopyTexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, 0, 0, tmpTexture.Width, tmpTexture.Height, 0);
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                    currTexture = tmpTexture;
                }
                //GL.Disable(EnableCap.PolygonOffsetFill);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            }
            else
            {
                HandledTexture = texture;
            }
        }
    }
}