using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CStrawberry3D.scene;
using CStrawberry3D.core;
using CStrawberry3D.component;

namespace CStrawberry3D.renderer
{
    public class OpenGLRenderer
    {
        private static OpenGLRenderer _singleton = null;
        public static OpenGLRenderer getSingleton()
        {
            if (_singleton == null)
            {
                _singleton = new OpenGLRenderer();
            }
            return _singleton;
        }

        private GameWindow _window = null;
        private Scene _scene = new Scene();
        private Stopwatch _clock = new Stopwatch();
        private RenderState _renderState = new RenderState();
        public RenderState renderState
        {
            get
            {
                return renderState;
            }
        }

        private float _totalTime = 0;
        public float totalTime
        {
            get
            {
                return _totalTime;
            }
        }
        private float _deltaTime = 0;
        public float deltaTime
        {
            get
            {
                return _deltaTime;
            }
        }
        public Scene scene
        {
            get
            {
                return _scene;
            }
        }
        public OpenGLRenderer()
        {
            if (_singleton != null)
            {
                throw new Exception("Exception: Use D3DRenderer.getSingleton()");
            }
            _singleton = this;
        }
        public void init(string windowCaption, int width, int height)
        {
            _window = new GameWindow(width, height, GraphicsMode.Default, windowCaption);
            _window.Load += load;
            _window.Resize += resize;
            _window.RenderFrame += renderFrame;
            _window.UpdateFrame += updateFrame;
        }
        public void run()
        {
            _clock.Start();
            _window.Run();
        }
        private void load(object sender, EventArgs e)
        {
            _window.VSync = VSyncMode.On;
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(Color4.Black);

        }
        private void resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, _window.Width, _window.Height);
        }
        private void updateFrame(object sender, FrameEventArgs e)
        {
             _deltaTime = _clock.ElapsedMilliseconds * 0.001f;
             _totalTime += _deltaTime;
            _clock.Restart();

        }
        private void renderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);

            Matrix4 pMatrix = Matrix4.CreatePerspectiveFieldOfView(core.Mathf.PI / 4, (float)_window.Width / _window.Height, 0.1f, 10000);

            foreach (StrawberryNode node in scene.root.getAll())
            {
                foreach (Component component in node.components)
                {
                    if (component.getName() == "DirectionalLightComponent")
                    {
                        _renderState.directionalLights.Add(node);
                    }
                }
            }

            foreach (StrawberryNode node in scene.root.getAll())
            {
                node.updateWorldMatrix();
                foreach (Component component in node.components)
                {
                    switch (component.getName())
                    {
                        case "MeshComponent":
                            MeshComponent meshComponent = (MeshComponent)component;
                            meshComponent.draw(false, pMatrix, node.matrixWorld);
                            break;
                    }
                }
            }

            _window.SwapBuffers();
            _renderState.restore();

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine(error);
            }
      
        }

    }
}
