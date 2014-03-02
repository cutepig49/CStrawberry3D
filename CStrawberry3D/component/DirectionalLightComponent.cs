using OpenTK;

namespace CStrawberry3D.Component
{
   public class DirectionalLightComponent:IComponent
    {
       public Vector4 DiffuseColor { get; set; }
       public Vector4 SpecularColor { get; set; }

        public DirectionalLightComponent():base()
        {
            Name = DIRECTIONAL_LIGHT_COMPONENT;
            DiffuseColor = new Vector4(1, 1, 1, 1);
            SpecularColor = new Vector4(1, 1, 1, 1);
        }
    }
}
