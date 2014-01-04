using System;
using System.Collections.Generic;
using System.Text;

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
        protected string _componentName = "EmptyComponent";
        protected bool _isActive = true;
        public string getName()
        {
            return _componentName;
        }
        public void setActive(bool isActive)
        {
            _isActive = isActive;
        }
    }
}
