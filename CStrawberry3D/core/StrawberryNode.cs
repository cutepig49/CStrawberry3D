using System;
using CStrawberry3D.Component;
using OpenTK;
using System.Collections.Generic;

namespace CStrawberry3D.Core
{
    public class StrawberryNode
    {
        private static int _objectCount = 0;
        public int Id { get; private set; }
        public string Guid { get; private set; }
        public List<EmptyComponent> Components { get; private set; }
        public StrawberryNode Parent { get; private set; }
        public List<StrawberryNode> Children { get; private set; }
        public Vector3 up;
        public Vector3 translation;
        public Vector3 Forward
        {
            get
            {
                UpdateWorldMatrix();
                var tmp = matrixWorld.ExtractRotation();
                var euler = Mathf.QuaternionToEuler(matrixWorld.ExtractRotation());
                var z = Math.Cos(euler.Y) * Math.Cos(euler.X);
                var x = Math.Sin(euler.Y) * Math.Cos(euler.X);
                var y = -Math.Sin(euler.X);
                var forward = new Vector3((float)x, (float)y, (float)z);
                return forward;
            }
        }
        public float X
        {
            get
            {
                return translation.X;
            }
            set
            {
                translation.X = value;
            }
        }
        public float Y
        {
            get
            {
                return translation.Y;
            }
            set
            {
                translation.Y = value;
            }
        }
        public float Z
        {
            get
            {
                return translation.Z;
            }
            set
            {
                translation.Z = value;
            }
        }
        public Vector3 rotation;
        public float Rx
        {
            get
            {
                return rotation.X;
            }
            set
            {
                rotation.X = value;
            }
        }
        public float Ry
        {
            get
            {
                return rotation.Y;
            }
            set
            {
                rotation.Y = value;
            }
        }
        public float Rz
        {
            get
            {
                return rotation.Z;
            }
            set
            {
                rotation.Z = value;
            }
        }
        public Vector3 scaling;
        public float Sx
        {
            get
            {
                return scaling.X;
            }
            set
            {
                scaling.X = value;
            }
        }
        public float Sy
        {
            get
            {
                return scaling.Y;
            }
            set
            {
                scaling.Y = value;
            }
        }
        public float Sz
        {
            get
            {
                return scaling.Z;
            }
            set
            {
                scaling.Z = value;
            }
        }
        public Matrix4 matrixTranslation;
        public Matrix4 matrixRotation;
        public Matrix4 matrixScaling;
        public Matrix4 matrixWorld;
        public Matrix4 MatrixLocal
        {
            get
            {
                return matrixScaling * matrixRotation * matrixTranslation;
            }
        }
        public bool matrixNeedUpdate;
        public Dictionary<object, object> userData;
        public StrawberryNode()
        {
            Id = ++_objectCount;
            Guid = System.Guid.NewGuid().ToString();
            up = new Vector3();
            translation = new Vector3();
            rotation = new Vector3();
            scaling = new Vector3(1,1,1);
            matrixTranslation = new Matrix4();
            matrixRotation = new Matrix4();
            matrixScaling = new Matrix4();
            matrixWorld = new Matrix4();
            Parent = null;
            Children = new List<StrawberryNode>();
            matrixNeedUpdate = true;
            userData = new Dictionary<object, object>();
            Components = new List<EmptyComponent>();
        }
        public void Translate(Vector3 value)
        {
            translation += value;
        }
        public void TranslateX(float x)
        {
            Translate(new Vector3(x, 0, 0));
        }
        public void TranslateY(float y)
        {
            Translate(new Vector3(0, y, 0));
        }
        public void TranslateZ(float z)
        {
            Translate(new Vector3(0,0,z));
        }
        public void Rotate(Vector3 value)
        {
            rotation += value;
        }
        public void RotateX(float x)
        {
            Rotate(new Vector3(x, 0, 0));
        }
        public void RotateY(float y)
        {
            Rotate(new Vector3(0, y, 0));
        }
        public void RotateZ(float z)
        {
            Rotate(new Vector3(0, 0, z));
        }
        public void Scale(Vector3 value)
        {
            scaling += value;
        }
        public void ScaleX(float x)
        {
            Scale(new Vector3(x, 0, 0));
        }
        public void ScaleY(float y)
        {
            Scale(new Vector3(0, y, 0));
        }
        public void ScaleZ(float z)
        {
            Scale(new Vector3(0, 0, z));
        }
        public void UpdateLocalMatrix()
        {
            matrixTranslation = Matrix4.CreateTranslation(translation);
            matrixRotation = Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z);
            matrixScaling = Matrix4.CreateScale(scaling);
        }
        public void UpdateWorldMatrix()
        {
            if (!matrixNeedUpdate)
                return;
            UpdateLocalMatrix();
            if (Parent != null)
            {
                Parent.UpdateWorldMatrix();
                matrixWorld =  MatrixLocal * Parent.matrixWorld;
            }
            else
                matrixWorld = MatrixLocal;
            //TODO
            //_matrixNeedUpdate = false;
        }
        public void AddComponent<T>(T component)where T : EmptyComponent
        {
            if (!Components.Contains(component))
            {
                Components.Add(component);
                component.Node = this;
            }
        }
        public void RemoveComponent<T>(T component)where T:EmptyComponent
        {
            Components.Remove(component);
        }
        public void AddChild(StrawberryNode child)
        {
            if (!Children.Contains(child))
            {
                Children.Add(child);
                child.Parent = this;
            }
        }
        public void RemoveChild(StrawberryNode child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.Parent = null;
            }
        }
        public StrawberryNode CreateChild()
        {
            var child = new StrawberryNode();
            AddChild(child);
            return child;
        }
        public T GetComponent<T>() where T : EmptyComponent
        {

            foreach (var component in Components)
            {
                if (component.GetType() == typeof(T))
                    return (T)component;
            }

            return null;
        }
        public T[] getComponents<T>() where T : EmptyComponent
        {
            List<T> returnComponents = new List<T>();
            foreach (var component in Components)
            {
                if (component.GetType() == typeof(T))
                    returnComponents.Add((T)component);
            }
            return returnComponents.ToArray();
        }
        public List<StrawberryNode> GetAll()
        {
            List<StrawberryNode> list = new List<StrawberryNode>();
            _AddSelfAndChildren(ref list);
            return list;
        }
        private void _AddSelfAndChildren(ref List<StrawberryNode> list)
        {
            list.Add(this);
            foreach(var node in Children)
            {
                node._AddSelfAndChildren(ref list);
            }
        }
    }
}
