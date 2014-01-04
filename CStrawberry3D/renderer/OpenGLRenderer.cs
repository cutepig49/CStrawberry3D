using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
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

        private float _total = 0;
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
            GL.ClearColor(Color4.Black);

        }
        private void resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, _window.Width, _window.Height);
        }
        private void updateFrame(object sender, FrameEventArgs e)
        {
            float delta = _clock.ElapsedMilliseconds * 0.001f;
            scene.root.getAll()[0].rotateX((float)Math.PI * delta);
            if (_total >= 1)
            {
                Console.WriteLine(_total);
                Console.WriteLine(scene.root.getAll()[0].rx);
                Console.Read();
            }
        }
        private void renderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);

            Matrix4 pMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, (float)_window.Width / _window.Height, 0.1f, 100f);

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
        }

    }
}
