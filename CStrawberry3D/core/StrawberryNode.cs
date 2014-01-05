using CStrawberry3D.component;
using OpenTK;
using System.Collections.Generic;

namespace CStrawberry3D.core
{
    public class StrawberryNode
    {
        private static int _objectCount = 0;
        private int _id = _objectCount++;
        public int id
        {
            get
            {
                return _id;
            }
        }
        private string _guid = System.Guid.NewGuid().ToString();
        public string guid
        {
            get
            {
                return _guid;
            }
        }
        private List<Component> _components = new List<Component>();
        public List<Component> components
        {
            get
            {
                return _components;
            }
        }
        private StrawberryNode _parent = null;
        private List<StrawberryNode> _children = new List<StrawberryNode>();
        protected Vector3 _up = new Vector3();
        protected Vector3 _translation = new Vector3();
        public Vector3 translation
        {
            get
            {
                return _translation;
            }
            set
            {
                _translation = value;
            }
        }
        public float x
        {
            get
            {
                return _translation.X;
            }
            set
            {
                _translation.X = value;
            }
        }
        public float y
        {
            get
            {
                return _translation.Y;
            }
            set
            {
                _translation.Y = value;
            }
        }
        public float z
        {
            get
            {
                return _translation.Z;
            }
            set
            {
                _translation.Z = value;
            }
        }
        protected Vector3 _rotation = new Vector3();
        public Vector3 rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }
        public float rx
        {
            get
            {
                return _rotation.X;
            }
            set
            {
                _rotation.X = value;
            }
        }
        public float ry
        {
            get
            {
                return _rotation.Y;
            }
            set
            {
                _rotation.Y = value;
            }
        }
        public float rz
        {
            get
            {
                return _rotation.Z;
            }
            set
            {
                _rotation.Z = value;
            }
        }
        protected Vector3 _scale = new Vector3();
        private Matrix4 _matrixLocal = new Matrix4();
        private Matrix4 _matrixWorld = new Matrix4();
        public Matrix4 matrixWorld
        {
            get
            {
                return _matrixWorld;
            }
        }
        bool _matrixNeedUpdate = true;
        public Dictionary<object, object> userData = new Dictionary<object, object>();
        public StrawberryNode()
        {
        }
        public void translate(Vector3 value)
        {
            _translation += value;
        }
        public void translateX(float x)
        {
            translate(new Vector3(x, 0, 0));
        }
        public void translateY(float y)
        {
            translate(new Vector3(0, y, 0));
        }
        public void translateZ(float z)
        {
            translate(new Vector3(0,0,z));
        }
        public void rotate(Vector3 value)
        {
            _rotation += value;
        }
        public void rotateX(float x)
        {
            rotate(new Vector3(x, 0, 0));
        }
        public void rotateY(float y)
        {
            rotate(new Vector3(0, y, 0));
        }
        public void rotateZ(float z)
        {
            rotate(new Vector3(0, 0, z));
        }
        public void lookAt(Vector3 target, Vector3 up)
        {
            //TODO
        }
        private void _updateLocalMatrix()
        {
            _matrixLocal = Matrix4.CreateTranslation(_translation);
            _matrixLocal *= Matrix4.CreateRotationX(_rotation.X);
            _matrixLocal *= Matrix4.CreateRotationY(_rotation.Y);
            _matrixLocal *= Matrix4.CreateRotationZ(_rotation.Z);
        }
        public void updateWorldMatrix()
        {
            if (!_matrixNeedUpdate)
                return;
            _updateLocalMatrix();
            if (_parent != null)
            {
                _parent.updateWorldMatrix();
                _matrixWorld = _parent._matrixWorld * _matrixLocal;
            }
            else
            {
                _matrixWorld = _matrixLocal;
            }
            //TODO
            //_matrixNeedUpdate = false;
        }
        public void addComponent(component.Component component)
        {
            if (!_components.Contains(component))
            {
                _components.Add(component);
            }
        }
        public void addChild(StrawberryNode child)
        {
            if (!_children.Contains(child))
            {
                _children.Add(child);
                child._parent = this;
            }
        }
        public void removeChild(StrawberryNode child)
        {
            if (_children.Contains(child))
                _children.Remove(child);
        }
        public StrawberryNode createChild()
        {
            var child = new StrawberryNode();
            addChild(child);
            return child;
        }
        public List<StrawberryNode> getAll()
        {
            List<StrawberryNode> list = new List<StrawberryNode>();
            _addSelfAndChildren(ref list);
            return list;
        }
        private void _addSelfAndChildren(ref List<StrawberryNode> list)
        {
            list.Add(this);
            foreach(var node in _children)
            {
                node._addSelfAndChildren(ref list);
            }
        }
    }
}
