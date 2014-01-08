using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using CStrawberry3D.loader;
using CStrawberry3D.shader;

namespace CStrawberry3D.component
{
    public class Material
    {
        private Effect _effect;
        public Effect effect
        {
            get
            {
                return _effect;
            }
            set
            {
                if (value != null)
                    _effect = value;
            }
        }
        //protected bool _hasPosition = true;
        //public bool hasPosition
        //{
        //    get
        //    {
        //        return _hasPosition;
        //    }
        //}
        //protected bool _hasIndex = true;
        //public bool hasIndex
        //{
        //    get
        //    {
        //        return _hasIndex;
        //    }
        //}
        //protected bool _hasTexture = false;
        //public bool hasTexture
        //{
        //    get
        //    {
        //        return _hasTexture;
        //    }
        //}
        //protected bool _hasGlobalColor = false;
        //public bool hasGlobalColor
        //{
        //    get
        //    {
        //        return _hasGlobalColor;
        //    }
        //}
        //protected bool _hasVertexColor = false;
        //public bool hasColor
        //{
        //    get
        //    {
        //        return _hasVertexColor;
        //    }
        //}
        protected Vector4 _globalColor;
        public Vector4 globalColor
        {
            get
            {
                return _globalColor;
            }
            set
            {
                _globalColor = value;
            }
        }

        protected bool _isTransparent = false;
        public bool isTransparent
        {
            get
            {
                return _isTransparent;
            }
        }

        protected bool _castShadow = false;
        protected bool _receiveShadow = false;

        public Material(Effect effect)
        {
            _effect = effect;
        }
        public void changeEffect(Effect effect)
        {
            _effect = effect;
        }
        private int _textureIndex;
        public int textureIndex
        {
            get
            {
                return _textureIndex;
            }
            set
            {
                _textureIndex = value;
            }
        }
    }

    public class GlobalColorMaterial : Material
    {
        public GlobalColorMaterial(Vector4 globalColor) :
            base(ShaderManager.GlobalColorEffect)
        {
            _globalColor = globalColor;
        }
    }

    public class BasicColorMaterial : Material
    {
        public BasicColorMaterial()
            : base(ShaderManager.BasicColorEffect)
        {
        }
    }
    public class TexturedMaterial : Material
    {
        public TexturedMaterial()
            : base(ShaderManager.TexturedEffect)
        {
        }
    }
    public class TexturedPhongMaterial : Material
    {
        public TexturedPhongMaterial()
            :base(ShaderManager.TexturedPhongEffect)
        {
        }
    }
}
