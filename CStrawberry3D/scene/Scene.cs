using CStrawberry3D.component;
using CStrawberry3D.core;
using OpenTK;

namespace CStrawberry3D.scene
{
    public class Scene
    {
        private StrawberryNode _root = new StrawberryNode();
        public StrawberryNode root
        {
            get
            {
                return _root;
            }
        }
        private StrawberryNode _camera;
        public StrawberryNode camera
        {
            get
            {
                return camera;
            }
        }
        private Vector4 _ambientColor = new Vector4(0.6f, 0.6f, 0.6f, 1);
        public Vector4 ambientColor
        {
            get
            {
                return _ambientColor;
            }
            set
            {
                _ambientColor = value;
            }
        }
        public Scene()
        {
            _camera = createCamera();
        }
        public StrawberryNode createCamera()
        {
            StrawberryNode camera = new StrawberryNode();
            camera.addComponent(new CameraComponent());
            return camera;
        }
        public StrawberryNode createDirectionalLight()
        {
            StrawberryNode light = new StrawberryNode();
            light.addComponent(new DirectionalLightComponent());
            return light;
        }
    }
}

