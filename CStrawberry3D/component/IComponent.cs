using CStrawberry3D.Core;

namespace CStrawberry3D.Component
{
    public class IComponent
    {
        public const string EMPTY_COMPONENT = "EmptyComponent";
        public const string MESH_COMPONENT = "MeshComponent";
        public const string DIRECTIONAL_LIGHT_COMPONENT = "DirectionalLightComponent";
        public const string CAMERA_COMPONENT = "CameraComponent";
        public const string TERRAIN_COMPONENT = "TerrainComponent";

        public string Guid { get; protected set; }
        public string Name { get; protected set; }
        public bool Actice { get; set; }
        public StrawberryNode Node { get; set; }
        protected IComponent()
        {
            Guid = System.Guid.NewGuid().ToString();
            Name = EMPTY_COMPONENT;
            Actice = true;
        }
    }
}
