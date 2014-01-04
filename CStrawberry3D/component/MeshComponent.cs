using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using CStrawberry3D.shader;
using CStrawberry3D.loader;
using OpenTK;

namespace CStrawberry3D.component
{
    public class Shader
    {
        public const string U_PMATRIX_IDENTIFER = "uPMatrix";
        public const string U_MVMATRIX_IDENTIFER = "uMVMatrix";
        public const string U_SAMPLER_IDENTIFER = "uSampler";
        public const string A_VERTEXPOSITION_IDENTIFER = "aVertexPosition";
        public const string A_TEXTURECOORD_IDENTIFER = "aTextureCoord";
        public const string A_VERTEXCOLOR_IDENTIFER = "aVertexColor";

        private string _shaderScript;
        private ShaderType _shaderType;
        private int _shaderObject;
        public int shaderObject
        {
            get
            {
                return _shaderObject;
            }
        }
        public Shader(string shaderScript, ShaderType shaderType)
        {
            _shaderScript = shaderScript;
            _shaderType = shaderType;
            _shaderObject = GL.CreateShader(_shaderType);
            GL.ShaderSource(_shaderObject, _shaderScript);
            GL.CompileShader(_shaderObject);

            int result;
            GL.GetShader(_shaderObject, ShaderParameter.CompileStatus, out result);
            if (result != 1)
            {
                Console.WriteLine(GL.GetShaderInfoLog(_shaderObject));
            }
        }
    }
    public class Material
    {
        private Dictionary<string, int> _uniformIdentifers = new Dictionary<string, int>(){
            {Shader.U_MVMATRIX_IDENTIFER, -1},
            {Shader.U_PMATRIX_IDENTIFER, -1},
            {Shader.U_SAMPLER_IDENTIFER, -1}
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
        protected bool _hasPosition = false;
        public bool hasPosition
        {
            get
            {
                return _hasPosition;
            }
        }
        protected bool _hasIndex = false;
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
        protected bool _hasColor = false;
        public bool hasColor
        {
            get
            {
                return _hasColor;
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
            if (_hasPosition)
                GL.EnableVertexAttribArray(_attribIdentifers[Shader.A_VERTEXPOSITION_IDENTIFER]);
            if (_hasColor)
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

    public class BasicColorMaterial : Material
    {
        public BasicColorMaterial()
            : base(DefaultShader.BasicColorVertexShader, DefaultShader.BasicColorFragmentShader)
        {
            _hasColor = true;
            _hasPosition = true;
            _hasIndex = true;
        }
    }

    public class MeshComponent : Component
    {
        private Material _material;

        private Mesh _mesh;

        public MeshComponent(Mesh mesh, Material material)
            : base()
        {
            _componentName = "MeshComponent";
            _mesh = mesh;
            _material = material;
        }
        public void draw(bool isTransparentPass, Matrix4 pMatrix, Matrix4 mvMatrix)
        {
            if (isTransparentPass == _material.isTransparent)
            {
                _material.ready();
                _mesh.ready(_material, pMatrix, mvMatrix);
                _mesh.draw();
            }
        }
    }
}