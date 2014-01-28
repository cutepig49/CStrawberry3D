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

namespace CStrawberry3D.TK
{

    public delegate void UpdateFrame(TKRenderer renderer, float dt);
    public class TKRenderer
    {
        static TKRenderer _singleton;
        public static TKRenderer Singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = new TKRenderer();
                return _singleton;
            }
        }
        public UpdateFrame UpdateFrame;
        public GameWindow Form { get; private set; }
        public Scene Scene { get; private set; }
        public Clock Clock { get; private set; }
        public Logger Logger { get; private set; }
        public TKDevice Device { get; private set; }
        public TKRenderState RenderState { get; private set; }
        public TKLoader Loader { get; private set; }
        public TKShaderManager ShaderManager { get; private set; }
        public TKInput Input { get; private set; }
        public TKGBuffer GBuffer { get; private set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }
        TKMesh screenQuad;
        bool _needRecompile;
        string _shaderPath;
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
        TKRenderer() { }
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
        public void Init(string title, int width, int height)
        {
            Form = new GameWindow(width, height, GraphicsMode.Default, title);
            Form.WindowBorder = WindowBorder.Fixed;
            Scene = new Scene();
            Clock = new Clock();
            Logger = new Logger();
            RenderState = new TKRenderState();
            Loader = new TKLoader();
            ShaderManager = new TKShaderManager();
            Input = new TKInput();
            Device = new TKDevice();
            GBuffer = new TKGBuffer();

            _LoadScrrenQuad();
            
            Form.Title = string.Format("{0} - {1}", Form.Title, Device.AdapterName);
            ZNear = 1.0f;
            ZFar = 10000.0f;
            
            Logger.ShowConsole = true;
            Logger.PendingOnError = true;

            Form.Load += _Load;
            Form.UpdateFrame += _UpdateFrame;
            Form.RenderFrame += _RenderFrame;
        }
        void _LoadScrrenQuad()
        {
            Loader.LoadAsset("ScreenAlignedQuad.3ds");
            screenQuad = Loader.GetMesh("ScreenAlignedQuad.3ds");
            screenQuad.Materials[0] = new DeferredMaterial();
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
            UpdateFrame(this, Clock.Delta);


        }
        void _UpdateRenderState(List<StrawberryNode> nodes)
        {
            RenderState.ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(Mathf.PI / 4, Form.ClientSize.Width / Form.ClientSize.Height, ZNear, ZFar);
            RenderState.ViewMatrix = Scene.Camera.GetComponent<CameraComponent>().ViewMatrix;
            RenderState.AmbientLight = Scene.AmbientLight;
            RenderState.DirectionalLights.Clear();

            foreach (var node in nodes)
            {
                var directionalLights = node.getComponents<DirectionalLightComponent>();
                RenderState.DirectionalLights.AddRange(directionalLights);
            }
        }
        void _RenderFrame(object sender, FrameEventArgs e)
        {
            if (_needRecompile)
                RecompileShader();

            var nodes = Scene.Root.GetAll();
            _UpdateRenderState(nodes);


            Device.Clear();
            GBuffer.Apply();

            foreach (var node in nodes)
            {
                node.UpdateWorldMatrix();
                var mesh = node.GetComponent<MeshComponent>();
                if (mesh != null)
                    mesh.Draw(false);
            }

            GBuffer.Clear();

            screenQuad.Draw(false, Matrix4.CreateTranslation(0, 0, 0));

            Device.GetError();

            Form.SwapBuffers();
        }
    }
}
