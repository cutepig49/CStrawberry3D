using CStrawberry3D.Component;
using OpenTK;

namespace CStrawberry3D.Core
{
    public class Scene
    {
        public static Scene Create()
        {
            return new Scene();
        }
        public StrawberryNode Root { get; private set; }
        public StrawberryNode Camera { get; private set; }
        public Vector4 AmbientLight { get; set; }
        Scene()
        {
            Root = StrawberryNode.Create();
            Camera = CreateCamera();
            AmbientLight = new Vector4(0.6f, 0.6f, 0.6f, 1);
        }
        public StrawberryNode CreateCamera()
        {
            var camera = StrawberryNode.Create();
            camera.AddComponent(new CameraComponent());
            return camera;
        }
        public StrawberryNode AddDirectionalLight()
        {
            var light = Root.CreateChild();
            light.AddComponent(new DirectionalLightComponent());
            return light;
        }
    }
}

