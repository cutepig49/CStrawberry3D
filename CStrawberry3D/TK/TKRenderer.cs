using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using CStrawberry3D.Core;
using CStrawberry3D.Platform;
using CStrawberry3D.Component;
using CStrawberry3D.Interface;
using CStrawberry3D.TK;
using System.Windows.Forms;

namespace CStrawberry3D.TK
{
    public delegate void UpdateFrame(float dt);
    public struct DrawDescription
    {
        public Matrix4 WorldMatrix { get; set; }
        public TKMeshEntry[] Entries { get; set; }
    }
    public class TKRenderer
    {
        public static TKRenderer Singleton { get; private set; }
        public static TKRenderer Create(string title, int width, int height)
        {
            Singleton = new TKRenderer(title, width, height);
            return Singleton;
        }
        public int CurrDrawCalls { get; private set; }
        public UpdateFrame UpdateFrame;
        public GameWindow Form { get; private set; }
        public Scene Scene { get; private set; }
        public Clock Clock { get; private set; }
        public TKDevice Device { get; private set; }
        public TKLoader Loader { get; private set; }
        public TKShaderManager ShaderManager { get; private set; }
        public TKInput Input { get; private set; }
        public TKGBuffer GBuffer { get; private set; }
        public TKSound Sound { get; private set; }
        public TKLightingBuffer LightingBuffer { get; private set; }
        public TKPostProcessManager PostProcessManager { get; private set; }
        public TKTexture LastScreen { get; private set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }
        public float FormAspect
        {
            get
            {
                return (float)FormWidth / (float)FormHeight;
            }
        }
        public Matrix4 ProjectionMatrix
        {
            get
            {
                return Matrix4.CreatePerspectiveFieldOfView(Mathf.PI / 4, FormAspect, ZNear, ZFar);
            }
        }
        public Matrix4 ViewMatrix
        {
            get
            {
                return Scene.Camera.GetComponent<CameraComponent>().ViewMatrix;
            }
        }
        TKMesh _screenQuad;
        bool _needRecompile;
        string _shaderPath;
        TKScreenMaterial _screenMaterial;
        public int FormWidth
        {
            get
            {
                return Form.ClientSize.Width;
            }
        }
        public int FormHeight
        {
            get
            {
                return Form.ClientSize.Height;
            }
        }
        void _CreateScreenTexture()
        {
            LastScreen = TKTexture.Create();
            LastScreen.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, FormWidth, FormHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero, false);
        }
        TKRenderer(string title, int width, int height)
        {
            Form = new GameWindow(width, height, GraphicsMode.Default, title);
            Form.WindowBorder = WindowBorder.Fixed;
            Scene = Scene.Create();
            Clock = Clock.Create();
            ShaderManager = new TKShaderManager();
            Loader = TKLoader.Create(ShaderManager);
            Input = new TKInput();
            Sound = TKSound.Create();
            Device = TKDevice.Create();
            GBuffer = TKGBuffer.Create(FormWidth, FormHeight);
            _screenMaterial = TKScreenMaterial.Create(ShaderManager, null);
            _LoadScrrenQuad();
            LightingBuffer = TKLightingBuffer.Create(FormWidth, FormHeight, _screenQuad);
            PostProcessManager = TKPostProcessManager.Create(FormWidth, FormHeight, _screenQuad);
            _CreateScreenTexture();


            Form.Title = string.Format("{0} - {1}", Form.Title, Device.AdapterName);
            ZNear = 1.0f;
            ZFar = 1000000.0f;

            Logger.ShowConsole = true;
            Logger.PendingOnError = true;

            Form.Load += _Load;
            Form.UpdateFrame += _UpdateFrame;
            Form.RenderFrame += _RenderFrame;
        }
        public void ShaderChanged(string filePath)
        {
            _needRecompile = true;
            _shaderPath = filePath;
        }
        public void RecompileShader()
        {
            _needRecompile = false;
            ShaderManager.RecompileAllShader();
            Logger.Info(string.Format("File {0} changed, and recompiled.", _shaderPath));
        }
        void _LoadScrrenQuad()
        {
            Loader.LoadAsset("ScreenAlignedQuad.3ds");
            _screenQuad = Loader.GetMesh("ScreenAlignedQuad.3ds");
            _screenQuad.Entries[0].Material = TKDeferredMaterial.Create(ShaderManager, GBuffer);
        }
        public void Run()
        {
            Clock.Start();
            Form.Run();
        }
        void _Load(object sender, EventArgs e)
        {
            Form.VSync = VSyncMode.On;
        }
        void _UpdateFrame(object sender, EventArgs e)
        {
            Clock.Tick();
            Input.Update();

            if (UpdateFrame != null)
            {
                UpdateFrame(Clock.Delta);
            }

            Console.WriteLine("DrawCalls: " + CurrDrawCalls);
            Console.WriteLine("FPS: " + Clock.Fps);
        }

        public void Draw(TKMaterial material, TKMeshEntry entry, SceneDescription desc)
        {
            for (int i = 0; i < material.NumPasses; i++)
            {
                material.Apply(i, entry, desc);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, entry.IndexBuffer.BufferObject);
                GL.DrawElements(PrimitiveType.Triangles, entry.IndexBuffer.NumItems, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                material.Clear();
                CurrDrawCalls++;
            }
        }
        //void _UpdateRenderState(List<StrawberryNode> nodes)
        //{
        //    RenderState.ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(Mathf.PI / 4, Form.ClientSize.Width / Form.ClientSize.Height, ZNear, ZFar);
        //    RenderState.ViewMatrix = Scene.Camera.GetComponent<CameraComponent>().ViewMatrix;
        //    RenderState.AmbientLight = Scene.AmbientLight;
        //    RenderState.DirectionalLights.Clear();

        //    foreach (var node in nodes)
        //    {
        //        var directionalLights = node.getComponents<DirectionalLightComponent>();
        //        RenderState.DirectionalLights.AddRange(directionalLights);
        //    }
        //}
        void _RenderFrame(object sender, FrameEventArgs e)
        {
            CurrDrawCalls = 0;

            if (_needRecompile)
                RecompileShader();

            var nodes = Scene.Root.GetAll();

            var drawList = new List<DrawDescription>();
            var directionalLights = new List<DirectionalLightComponent>();

            foreach (var node in nodes)
            {
                var mesh = node.GetComponent<MeshComponent>();
                if (mesh != null)
                {
                    drawList.Add(mesh.DrawDesc);
                }
                var terrain = node.GetComponent<TerrainComponent>();
                if (terrain != null)
                {
                    drawList.Add(terrain.DrawDesc);
                }
                var dLights = node.GetComponents<DirectionalLightComponent>();
                directionalLights.AddRange(dLights);
            }

            var sceneDesc = new SceneDescription
            {
                AmbientLight = Scene.AmbientLight,
                DirectionalLights = directionalLights.ToArray(),
                ProjectionMatrix = ProjectionMatrix,
                ViewMatrix = ViewMatrix,
                WorldMatrix = Matrix4.Identity
            };

            Device.Clear();
            GBuffer.Begin(Device);

            if (Scene.Camera.CameraComponent.HasSkybox)
            {
                GL.DepthMask(false);
                Draw(Scene.Camera.CameraComponent.SkyboxMaterial, _screenQuad.FirstEntry, sceneDesc);
                GL.DepthMask(true);
            }

            foreach (var draw in drawList)
            {
                sceneDesc.WorldMatrix = draw.WorldMatrix;
                foreach (var entry in draw.Entries)
                {
                    Draw(entry.Material, entry, sceneDesc);
                }
            }

            GBuffer.End();

            LightingBuffer.Apply(Device, sceneDesc);

            PostProcessManager.Apply(Device, LightingBuffer.LightedTexture);

            _ShowOnScreen(PostProcessManager.HandledTexture);

            _SaveScreenTexture();

            Device.GetError();

            Form.SwapBuffers();
        }
        void _SaveScreenTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, LastScreen.TextureObject);
            GL.CopyTexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, 0, 0, FormWidth, FormHeight, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        void _ShowOnScreen(TKTexture texture)
        {
            _screenMaterial.Texture = texture;
            Draw(_screenMaterial, _screenQuad.FirstEntry, new SceneDescription());
        }
    }
}