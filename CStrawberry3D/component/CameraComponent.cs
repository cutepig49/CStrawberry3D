using OpenTK;
using CStrawberry3D.TK;
namespace CStrawberry3D.Component
{
    public class CameraComponent:IComponent
    {
        public static CameraComponent Create()
        {
            return new CameraComponent();
        }
        public Matrix4 ViewMatrix
        {
            get
            {
                var translationMatrix = Matrix4.CreateTranslation(-Node.X, -Node.Y, -Node.Z);
                var rotationMatrix = Matrix4.CreateRotationX(-Node.Rx) * Matrix4.CreateRotationY(-Node.Ry) * Matrix4.CreateRotationZ(-Node.Rz);
                var viewMatrix =  rotationMatrix * translationMatrix;
                return viewMatrix;
            }
        }
        public CameraComponent():base()
        {
            Name = CAMERA_COMPONENT;
        }
    }
}
