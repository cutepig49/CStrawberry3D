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
        public TKCubeMap Skybox { get;private set; }
        public TKSkyboxMaterial SkyboxMaterial { get; private set; }
        public Matrix4 ViewMatrix
        {
            get
            {
                var translationMatrix = Matrix4.CreateTranslation(-Node.X, -Node.Y, -Node.Z);
                var rotationMatrix = Matrix4.CreateRotationZ(-Node.Rz) * Matrix4.CreateRotationY(-Node.Ry) * Matrix4.CreateRotationX(-Node.Rx);
                var viewMatrix = translationMatrix * rotationMatrix;
                return viewMatrix;
            }
        }
        public bool HasSkybox
        {
            get
            {
                return Skybox != null;
            }
        }
        public CameraComponent():base()
        {
            Name = CAMERA_COMPONENT;
        }
        public void SetSkybox(TKCubeMap skybox)
        {
            Skybox = skybox;
            SkyboxMaterial = TKSkyboxMaterial.Create(TKRenderer.Singleton.ShaderManager, skybox);
        }
        public void RemoveSkybox()
        {
            Skybox = null;
        }
    }
}
