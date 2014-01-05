using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CStrawberry3D.shader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CStrawberry3D.shader
{
    public static class ShaderManager
    {
        public readonly static Shader GlobalColorVertexShader;
        public readonly static Shader GlobalColorFragmentShader;
        public readonly static Shader BasicColorVertexShader;
        public readonly static Shader BasicColorFragmentShader;
        public readonly static Shader TexturedVertexShader;
        public readonly static Shader TexturedFragmentShader;
        
        static ShaderManager()
        {
            GlobalColorVertexShader = new Shader(DefaultShaders.GlobalColorVertexShader, ShaderType.VertexShader);
            GlobalColorFragmentShader = new Shader(DefaultShaders.GlobalColorFragmentShader, ShaderType.FragmentShader);
            BasicColorVertexShader = new Shader(DefaultShaders.BasicColorVertexShader, ShaderType.VertexShader);
            BasicColorFragmentShader = new Shader(DefaultShaders.BasicColorFragmentShader, ShaderType.FragmentShader);
            TexturedVertexShader = new Shader(DefaultShaders.TexturedVertexShader, ShaderType.VertexShader);
            TexturedFragmentShader = new Shader(DefaultShaders.TexturedFragmentShader, ShaderType.FragmentShader);
        }

        public static void chooseDefaultShader()
        {

        }

    }

    public class Shader
    {
        public const string U_PMATRIX_IDENTIFER = "uPMatrix";
        public const string U_MVMATRIX_IDENTIFER = "uMVMatrix";
        public const string U_GLOBALCOLOR_IDENTIFER = "uGlobalColor";
        public const string U_SAMPLER_IDENTIFER = "uSampler";
        public const string U_NMATRIX_IDENTIFER = "uNMatrix";
        public const string U_ACOLOR_IDENTIFER = "uAmbientColor";
        public const string U_DCOLOR_IDENTIFER = "uDirectionalColor";
        public const string U_LIGHTDIR_IDENTIFER = "uLightingDirection";
        public const string U_DLIGHTS_IDENTIFER = "uDirectionalNum";
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

}
