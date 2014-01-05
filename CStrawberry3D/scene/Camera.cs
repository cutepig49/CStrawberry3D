using CStrawberry3D.core;
using OpenTK;

namespace CStrawberry3D.scene
{
    public class Camera:StrawberryNode
    {
        public Camera(Vector3 position, Vector3 target, Vector3 up)
            : base()
        {
            _translation = position;
            lookAt(target, up);
        }
    }
}
