using CStrawberry3D.loader;
using CStrawberry3D.shader;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

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
                if (value!=null)
                    _effect = value;
            }
        }
        protected bool _hasPosition = true;
        public bool hasPosition
        {
            get
            {
                return _hasPosition;
            }
        }
        protected bool _hasIndex = true;
        public bool hasIndex
        {
            get
            {
                return _hasIndex;
            }
        }
        protected bool _hasTexture = false;
        public bool hasTexture
        {
            get
            {
                return _hasTexture;
            }
        }
        protected bool _hasGlobalColor = false;
        public bool hasGlobalColor
        {
            get
            {
                return _hasGlobalColor;
            }
        }
        protected bool _hasVertexColor = false;
        public bool hasColor
        {
            get
            {
                return _hasVertexColor;
            }
        }
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

        public static Material createCustomMaterial(Assimp.ShadingMode shadingMode, bool hasColorAmbient, bool hasColorDiffuse, bool hasColorSpecular, Vector4 colorAmbient = new Vector4(), Vector4 colorDiffuse=new Vector4(), Vector4 colorSpecular=new Vector4())
        {
            Material material = new GlobalColorMaterial(new Vector4(1, 0, 1, 1));
            return material;

            int checkAmbient = 0x00000001;
            int checkDiffuse = 0x00000010;
            int checkSpecular = 0x00000100;

            int checkGlobalColorShader = 0;

            int checkBasicColorShader = 0;

            int checkPhongShader = checkAmbient | checkDiffuse |checkSpecular;

            int checkFlatShader = checkAmbient | checkDiffuse; 

            int checkFlag = 0;
            if (hasColorAmbient)
                checkFlag |= checkAmbient;

            if (hasColorDiffuse)
                checkFlag |= checkDiffuse;

            if (hasColorSpecular)
                checkFlag |= checkSpecular;


            return new Material(null);
        }

        public Material(Effect effect)
        {
            _effect = effect;
        }
        public void changeEffect(Effect effect)
        {
            _effect = effect;
        }
    }

    public class GlobalColorMaterial : Material
    {
        public GlobalColorMaterial(Vector4 globalColor) :
            base(ShaderManager.GlobalColorEffect)
        {
            _hasGlobalColor = true;
            _globalColor = globalColor;
        }
    }

    public class BasicColorMaterial : Material
    {
        public BasicColorMaterial()
            : base(ShaderManager.BasicColorEffect)
        {
            _hasVertexColor = true;
        }
    }



    public class MeshComponent : Component
    {
        private Mesh _mesh;

        public MeshComponent(Mesh mesh)
            : base()
        {
            _name = "MeshComponent";
            _mesh = mesh;
        }
        public void draw(bool isTransparentPass, Matrix4 pMatrix, Matrix4 mvMatrix)
        {
            _mesh.draw(isTransparentPass, pMatrix, mvMatrix);
        }
    }
}