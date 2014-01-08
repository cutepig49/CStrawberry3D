using CStrawberry3D.core;

namespace CStrawberry3D.component
{
    public class Component
    {
        public const string EMPTY_COMPONENT = "EmptyComponent";
        public const string MESH_COMPONENT = "MeshComponent";
        public const string DIRECTIONAL_LIGHT_COMPONENT = "DirectionalLightComponent";

        private string _guid = System.Guid.NewGuid().ToString();
        public string guid
        {
            get
            {
                return _guid;
            }
        }
        protected string _name = EMPTY_COMPONENT;
        public string name
        {
            get
            {
                return _name;
            }
        }
        protected bool _active = true;
        public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }
        public StrawberryNode node;
    }
}
