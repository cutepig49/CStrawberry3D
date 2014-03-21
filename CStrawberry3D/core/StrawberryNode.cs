using System;
using CStrawberry3D.Component;
using OpenTK;
using System.Collections.Generic;

namespace CStrawberry3D.Core
{
    public class StrawberryNode
    {
        static int _objectCount = 0;
        public static StrawberryNode Create()
        {
            return new StrawberryNode();
        }
        public static StrawberryNode Create(float x, float y, float z)
        {
            var node = new StrawberryNode();
            node.X = x;
            node.Y = y;
            node.Z = z;
            return node;
        }
        public static StrawberryNode Create(Vector3 translation)
        {
            return Create(translation.X, translation.Y, translation.Z);
        }
        public Vector3 OriginalUnitX { get; set; }
        public Vector3 OriginalUnitZ { get; set; }
        public Vector3 OriginalUnitY { get; set; }
        public int ID { get; private set; }
        public string Guid { get; private set; }
        public List<IComponent> Components { get; private set; }
        public StrawberryNode Parent { get; private set; }
        public List<StrawberryNode> Children { get; private set; }
        public Vector3 translation;
        public CameraComponent CameraComponent
        {
            get
            {
                return GetComponent<CameraComponent>();
            }
        }
        public Quaternion RotationQuaternion
        {
            get
            {
                return WorldMatrix.ExtractRotation();
            }
        }
        public Vector3 Forward
        {
            get
            {
                var forward = Mathf.QuaternionMultiplyVector3(RotationQuaternion, OriginalUnitZ);
                return forward.Normalized();
            }
        }
        public Vector3 Up
        {
            get
            {
                var up = Mathf.QuaternionMultiplyVector3(RotationQuaternion, OriginalUnitY);
                return up.Normalized();
            }
        }
        public Vector3 Right
        {
            get
            {
                var right = Mathf.QuaternionMultiplyVector3(RotationQuaternion, OriginalUnitX);
                return right.Normalized();
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
        public Matrix4 TranslationMatrix
        {
            get
            {
                return Matrix4.CreateTranslation(translation);
            }
        }
        public Matrix4 RotationMatrix
        {
            get
            {
                return Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateRotationY(rotation.Y);
            }
        }
        public Matrix4 ScalingMatrix
        {
            get
            {
                return Matrix4.CreateScale(scaling);
            }
        }
        public Matrix4 LocalMatrix
        {
            get
            {
                return ScalingMatrix * RotationMatrix * TranslationMatrix;
            }
        }
        Matrix4 _worldMatrix;
        public Matrix4 WorldMatrix
        {
            get
            {
                if (matrixNeedUpdate)
                {
                    if (Parent != null)
                    {
                        _worldMatrix = LocalMatrix * Parent.WorldMatrix;
                    }
                    else
                    {
                        _worldMatrix = LocalMatrix;
                    }
                }
                return _worldMatrix;
            }
        }
        public bool matrixNeedUpdate;
        public Dictionary<object, object> userData;
        protected StrawberryNode()
        {
            ID = ++_objectCount;
            Guid = System.Guid.NewGuid().ToString();
            translation = new Vector3();
            rotation = new Vector3();
            scaling = new Vector3(1,1,1);
            Parent = null;
            Children = new List<StrawberryNode>();
            matrixNeedUpdate = true;
            userData = new Dictionary<object, object>();
            Components = new List<IComponent>();
            OriginalUnitX = Vector3.UnitX;
            OriginalUnitY = Vector3.UnitY;
            OriginalUnitZ = Vector3.UnitZ;
        }
        public void Move(Vector3 value)
        {
            translation += value;
        }
        public void MoveX(float x)
        {
            Move(new Vector3(x, 0, 0));
        }
        public void MoveY(float y)
        {
            Move(new Vector3(0, y, 0));
        }
        public void MoveZ(float z)
        {
            Move(new Vector3(0,0,z));
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
        public void Pitch(float radian)
        {
            var quat = Quaternion.FromAxisAngle(Right, radian) * RotationMatrix.ExtractRotation();
            rotation = Mathf.QuaternionToEuler(quat);
        }
        public void AddComponent<T>(T component)where T : IComponent
        {
            if (!Components.Contains(component))
            {
                Components.Add(component);
                component.Node = this;
            }
        }
        public void RemoveComponent<T>(T component)where T:IComponent
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
        public void RemoveAllChildren()
        {
            Children.Clear();
        }
        public StrawberryNode CreateChild()
        {
            var child = new StrawberryNode();
            AddChild(child);
            return child;
        }
        public T GetComponent<T>() where T : IComponent
        {

            foreach (var component in Components)
            {
                if (component.GetType() == typeof(T))
                    return (T)component;
            }

            return null;
        }
        public T[] GetComponents<T>() where T : IComponent
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
