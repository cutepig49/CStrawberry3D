using CStrawberry3D.core;

namespace CStrawberry3D.component
{
    public class Component
    {
        private string _guid = System.Guid.NewGuid().ToString();
        public string guid
        {
            get
            {
                return _guid;
            }
        }
        protected string _name = "EmptyComponent";
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
