//using CStrawberry3D.component;
//using CStrawberry3D.Core;
//using OpenTK;
//using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using CStrawberry3D.Platform;

//namespace CStrawberry3D.renderer
//{
//    public class OpenGLRenderer
//    {
//        private static OpenGLRenderer _singleton = null;
//        public static OpenGLRenderer getSingleton()
//        {
//            if (_singleton == null)
//            {
//                _singleton = new OpenGLRenderer();
//            }
//            return _singleton;
//        }

//        private GameWindow _window = null;
//        private Scene _scene = new Scene();
//        private Stopwatch _clock = new Stopwatch();
//        private RenderState _renderState = new RenderState();
//        public RenderState renderState
//        {
//            get
//            {
//                return _renderState;
//            }
//        }
//        private Logger _logger = Logger.getSingleton();
//        public Logger logger
//        {
//            get
//            {
//                return _logger;
//            }
//        }

//        public bool isChanged = false;
//        public string filePath;
//        private void shaderChanged()
//        {
//            if (!isChanged)
//                return;
//            if (filePath == ShaderManager.basicColorFragmentShaderPath || filePath == ShaderManager.basicColorVertexShaderPath)
//            {
//                ShaderManager.basicColorProgram.changeProgram(new shader.Program(ShaderManager.basicColorVertexShaderPath, ShaderManager.basicColorFragmentShaderPath));
//            }
//            else if (filePath == ShaderManager.globalColorVertexShaderPath || filePath == ShaderManager.globalColorFragmentShaderPath)
//            {
//                ShaderManager.globalColorProgram.changeProgram(new shader.Program(ShaderManager.globalColorVertexShaderPath, ShaderManager.globalColorFragmentShaderPath));
//            }
//            else if (filePath == ShaderManager.texturedVertexShaderPath || filePath == ShaderManager.texturedFragmentShaderPath)
//            {
//                ShaderManager.texturedProgram.changeProgram(new shader.Program(ShaderManager.texturedVertexShaderPath, ShaderManager.texturedFragmentShaderPath));
//            }
//            else if (filePath == ShaderManager.texturedPhongVertexShaderPath || filePath == ShaderManager.texturedPhongFragmentShaderPath)
//            {
//                ShaderManager.texturedPhongProgram.changeProgram(new shader.Program(ShaderManager.texturedPhongVertexShaderPath, ShaderManager.texturedPhongFragmentShaderPath));
//            }
//            logger.info(string.Format("{0} changed, and re-compiled.", filePath));
//            isChanged = false;
//        }
//        private string _caption;
//        private int _period = 30;
//        private int _currFrames = 0;
//        private float _startTime = 0;
//        private float _totalTime = 0;
//        public float totalTime
//        {
//            get
//            {
//                return _totalTime;
//            }
//        }
//        private float _deltaTime = 0;
//        public float deltaTime
//        {
//            get
//            {
//                return _deltaTime;
//            }
//        }
//        public Scene scene
//        {
//            get
//            {
//                return _scene;
//            }
//        }
//        public OpenGLRenderer()
//        {
//            if (_singleton != null)
//            {
//                throw new Exception("Exception: Use OpenGLRenderer.getSingleton()");
//            }
//            _singleton = this;
//        }
//        public void init(string caption, int width, int height, bool isDebug)
//        {
//            logger.console = isDebug;
//            _caption = caption;
//            _window = new GameWindow(width, height, GraphicsMode.Default, caption);
//            _window.Load += load;
//            _window.Resize += resize;
//            _window.RenderFrame += renderFrame;
//            _window.UpdateFrame += updateFrame;
//        }
//        public void run()
//        {
//            _clock.Start();
//            _window.Run();
//        }
//        private void load(object sender, EventArgs e)
//        {
//            _window.VSync = VSyncMode.On;
//            GL.Enable(EnableCap.Multisample);
//            GL.Enable(EnableCap.DepthTest);
//            GL.Enable(EnableCap.Texture2D);
//            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
//            GL.ClearColor(Color4.Pink);

//        }
//        private void resize(object sender, EventArgs e)
//        {
//            GL.Viewport(0, 0, _window.Width, _window.Height);
//        }
//        private void updateFrame(object sender, FrameEventArgs e)
//        {
//             _deltaTime = _clock.ElapsedMilliseconds * 0.001f;
//             _totalTime += _deltaTime;
//            _clock.Restart();


//            _currFrames++;
//            if (_currFrames > _period)
//            {
//                float fps = _currFrames / (_totalTime - _startTime);
//                _window.Title = _caption + " FPS: " + fps;
//                _startTime = _totalTime;
//                _currFrames = 0;
//            }

//            //scene.root.getAll()[1].ry = _totalTime * 0.25f;
//        }
//        private void renderFrame(object sender, FrameEventArgs e)
//        {
//            shaderChanged();

//            GL.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);

//            //TODO: bug!
//            Matrix4 viewMatrix = Matrix4.LookAt(scene.camera.translation, new Vector3(0,0,0), Vector3.UnitY);

//            Matrix4 pMatrix = Matrix4.CreatePerspectiveFieldOfView(Mathf.degreeToRadian(70), (float)_window.Width / _window.Height, 0.1f, 10000);

//            _renderState.ready(viewMatrix, scene.ambientColor);

//            List<StrawberryNode> directionalLights = new List<StrawberryNode>();
//            List<StrawberryNode> pointLights = new List<StrawberryNode>();
//            List<StrawberryNode> drawableNodes = new List<StrawberryNode>();
            
//            foreach (var node in scene.root.GetAll())
//            {
//                //Bug!
//                node.UpdateWorldMatrix();
//                foreach (var component in node.components)
//                {
//                    switch (component.name)
//                    {
//                        case Component.MESH_COMPONENT:
//                            drawableNodes.Add(node);
//                            break;
//                        case Component.DIRECTIONAL_LIGHT_COMPONENT:
//                            directionalLights.Add(node);
//                            break;
//                    }
//                }
//            }

//            foreach (var node in directionalLights)
//            {
//                foreach (var component in node.GetComponent(Component.DIRECTIONAL_LIGHT_COMPONENT))
//                {
//                    var directionalLightComponent = (DirectionalLightComponent)component;
//                    _renderState.directionalLights.Add(directionalLightComponent.diffuseColor);
//                    _renderState.directions.Add(node.Forward);
//                }
//            }

//            var isTransparentPass = false;
//            foreach (var node in drawableNodes)
//            {
//                //
//                Console.WriteLine(node.Forward);
//                foreach (var component in node.GetComponent(Component.MESH_COMPONENT))
//                {
//                    var meshComponent = (MeshComponent)component;
//                    meshComponent.draw(isTransparentPass, pMatrix, node.matrixWorld);
//                }
//            }


//            ErrorCode error = GL.GetError();
//            if (error != ErrorCode.NoError)
//            {
//                Logger.getSingleton().error(error.ToString());
//            }

//            _window.SwapBuffers();
//            _renderState.restore();

      
//        }

//    }
//}
