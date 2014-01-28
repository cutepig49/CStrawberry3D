using OpenTK;
using CStrawberry3D.TK;
namespace CStrawberry3D.Component
{
    public struct SkyBox
    {
        TKTexture PositiveX { get; set; }
        TKTexture NegativeX { get; set; }
        TKTexture PositiveY { get; set; }
        TKTexture NegativeY { get; set; }
        TKTexture PositiveZ { get; set; }
        TKTexture NegativeZ { get; set; }
    }
    public class CameraComponent:EmptyComponent
    {
        public SkyBox SkyBox { get; private set; }
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
        public void SetSkybox(SkyBox skyBox)
        {
            SkyBox = skyBox;
        }
        public void DisableSkybox()
        {

        }
    }
}
