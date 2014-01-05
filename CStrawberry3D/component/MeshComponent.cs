using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using CStrawberry3D.shader;
using CStrawberry3D.loader;
using OpenTK;

namespace CStrawberry3D.component
{
    public class Material
    {
        private Dictionary<string, int> _uniformIdentifers = new Dictionary<string, int>(){
            {Shader.U_MVMATRIX_IDENTIFER, -1},
            {Shader.U_PMATRIX_IDENTIFER, -1},
            {Shader.U_SAMPLER_IDENTIFER, -1},
            {Shader.U_GLOBALCOLOR_IDENTIFER, -1},
            {Shader.U_NMATRIX_IDENTIFER, -1},
            {Shader.U_ACOLOR_IDENTIFER, -1},
            {Shader.U_DCOLOR_IDENTIFER, -1},
            {Shader.U_LIGHTDIR_IDENTIFER, -1}
        };
        private Dictionary<string, int> _attribIdentifers = new Dictionary<string, int>(){
            {Shader.A_TEXTURECOORD_IDENTIFER, -1},
            {Shader.A_VERTEXCOLOR_IDENTIFER, -1},
            {Shader.A_VERTEXPOSITION_IDENTIFER, -1}
        };
        public Dictionary<string, int> uniformIdentifers
        {
            get
            {
                return _uniformIdentifers;
            }
        }
        public Dictionary<string, int> attribIdentifers
        {
            get
            {
                return _attribIdentifers;
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

        Shader _vertexShader;
        Shader _fragmentShader;
        int _shaderProgram;

        public static Material createCustomMaterial(Assimp.ShadingMode shadingMode, bool hasColorAmbient, bool hasColorDiffuse, bool hasColorSpecular, Vector4 colorAmbient = new Vector4(), Vector4 colorDiffuse=new Vector4(), Vector4 colorSpecular=new Vector4())
        {
            Shader shader;


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

            Console.WriteLine(checkFlag == checkPhongShader);
            Console.WriteLine(checkFlag == checkFlatShader);

            return new Material("", "");
        }

        public Material(string vertexShaderScript, string fragmentShaderScript)
        {
            _vertexShader = new Shader(vertexShaderScript, ShaderType.VertexShader);
            _fragmentShader = new Shader(fragmentShaderScript, ShaderType.FragmentShader);
            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, _vertexShader.shaderObject);
            GL.AttachShader(_shaderProgram, _fragmentShader.shaderObject);
            GL.LinkProgram(_shaderProgram);

            int result;
            GL.GetProgram(_shaderProgram, GetProgramParameterName.LinkStatus, out result);
            if (result != 1)
            {
                Console.WriteLine("Could not initialise shaders");
            }
            _cacheAttribLocation();
            _cacheUniformLocation();

        }
        public void ready()
        {
            GL.UseProgram(_shaderProgram);

            if (_hasGlobalColor)
                GL.Uniform4(_uniformIdentifers[Shader.U_GLOBALCOLOR_IDENTIFER], _globalColor);
            if (_hasPosition)
                GL.EnableVertexAttribArray(_attribIdentifers[Shader.A_VERTEXPOSITION_IDENTIFER]);
            if (_hasVertexColor)
                GL.EnableVertexAttribArray(_attribIdentifers[Shader.A_VERTEXCOLOR_IDENTIFER]);
            if (_hasTexture)
                GL.EnableVertexAttribArray(_attribIdentifers[Shader.A_TEXTURECOORD_IDENTIFER]);
        }
        private void _cacheAttribLocation()
        {
            foreach (string key in new List<string>(_attribIdentifers.Keys))
            {
                _attribIdentifers[key] = GL.GetAttribLocation(_shaderProgram, key);
            }
        }
        private void _cacheUniformLocation()
        {
            foreach (string key in new List<string>(_uniformIdentifers.Keys))
            {
                _uniformIdentifers[key] = GL.GetUniformLocation(_shaderProgram, key);
            }
        }
    }

    public class GlobalColorMaterial : Material
    {
        public GlobalColorMaterial(Vector4 globalColor) :
            base(DefaultShaders.GlobalColorVertexShader, DefaultShaders.GlobalColorFragmentShader)
        {
            _hasGlobalColor = true;
            _globalColor = globalColor;
        }
    }

    public class BasicColorMaterial : Material
    {
        public BasicColorMaterial()
            : base(DefaultShaders.BasicColorVertexShader, DefaultShaders.BasicColorFragmentShader)
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
            _componentName = "MeshComponent";
            _mesh = mesh;
        }
        public void draw(bool isTransparentPass, Matrix4 pMatrix, Matrix4 mvMatrix)
        {
            _mesh.draw(isTransparentPass, pMatrix, mvMatrix);
        }
    }
}