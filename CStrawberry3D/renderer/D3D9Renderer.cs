//using System;
//using System.Diagnostics;
//using System.Text;
//using SharpDX.Direct3D9;
//using SharpDX.Windows;
//using SharpDX;

//namespace CStrawberry3D.renderer
//{
//    class D3D9Renderer
//    {
//        private static D3D9Renderer _singleton = null;
//        public static D3D9Renderer getSingleton()
//        {
//            if (_singleton == null)
//            {
//                _singleton = new D3D9Renderer();
//            }
//            return _singleton;
//        }
//        private Direct3D _direct3D = null;
//        private Device _device = null;
//        private RenderForm _window = null;
//        public D3D9Renderer()
//        {
//            if (_singleton != null)
//            {
//                throw new Exception("Exception: Use D3DRenderer.getSingleton()");
//            }
//            _singleton = this;
//        }
//        public void init(string windowCaption)
//        {
//            _window = new RenderForm(windowCaption);
//            _window.SetBounds(0, 0, 1000, 1000);
//            _direct3D = new Direct3D();
//            _device = new Device(_direct3D, 0, DeviceType.Hardware, _window.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters(_window.Width, _window.Height));
//        }
//        public void run()
//        {
//            var clock = new Stopwatch();
//            clock.Start();
//            RenderLoop.Run(_window, () =>
//            {
//                var time = clock.ElapsedMilliseconds / 1000.0f;
//                _device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
//                _device.BeginScene();
//                _device.EndScene();
//                _device.Present();
//            });
//            _device.Dispose();
//            _direct3D.Dispose();
//        }
//    }
//}