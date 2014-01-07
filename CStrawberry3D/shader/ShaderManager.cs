using OpenTK.Graphics.OpenGL;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using CStrawberry3D.loader;
using CStrawberry3D.renderer;

using Buffer = CStrawberry3D.loader.Buffer;

namespace CStrawberry3D.shader
{
    public static class ShaderManager
    {
        public const string globalColorVertexShaderPath = "..\\..\\..\\Shaders\\GlobalColorVertexShader.glsl";
        public const string globalColorFragmentShaderPath = "..\\..\\..\\Shaders\\GlobalColorFragmentShader.glsl";
        public const string basicColorVertexShaderPath = "..\\..\\..\\Shaders\\BasicColorVertexShader.glsl";
        public const string basicColorFragmentShaderPath = "..\\..\\..\\Shaders\\BasicColorFragmentShader.glsl";
        public const string texturedVertexShaderPath = "..\\..\\..\\Shaders\\TexturedVertexShader.glsl";
        public const string texturedFragmentShaderPath = "..\\..\\..\\Shaders\\TexturedFragmentShader.glsl";

        public readonly static Shader GlobalColorVertexShader;
        public readonly static Shader GlobalColorFragmentShader;
        public readonly static Shader BasicColorVertexShader;
        public readonly static Shader BasicColorFragmentShader;
        public readonly static Shader TexturedVertexShader;
        public readonly static Shader TexturedFragmentShader;

        private static Program _globalColorProgram;
        public static Program globalColorProgram
        {
            get
            {
                return _globalColorProgram;
            }
            set
            {
                if (value != null)
                {
                    _globalColorProgram = value;
                }
            }
        }
        private static Program _basicColorProgram;
        public static Program basicColorProgram
        {
            get
            {
                return _basicColorProgram;
            }
            set
            {
                if (value != null)
                {
                    _basicColorProgram = value;
                }
            }
        }
        private static Program _texturedProgram;
        public static Program texturedProgram
        {
            get
            {
                return _texturedProgram;
            }
            set
            {
                if (value != null)
                {
                    _texturedProgram = value;
                }
            }

        }

        public readonly static Effect GlobalColorEffect;
        public readonly static Effect BasicColorEffect;
        public readonly static Effect TexturedEffect;

        private static FileSystemWatcher _fileWatcher = new FileSystemWatcher();
        static ShaderManager()
        {
            _globalColorProgram = new Program(globalColorVertexShaderPath, globalColorFragmentShaderPath);
            GlobalColorEffect = new Effect(new Program[]{_globalColorProgram});

            _basicColorProgram = new Program(basicColorVertexShaderPath, basicColorFragmentShaderPath);
            BasicColorEffect = new Effect(new Program[] { _basicColorProgram });

            _texturedProgram = new Program(texturedVertexShaderPath, texturedFragmentShaderPath);
            TexturedEffect = new Effect(new Program[] { _texturedProgram });

            _fileWatcher.Path = "..\\..\\..\\Shaders";
            _fileWatcher.Filter = "*.glsl";
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Changed += new FileSystemEventHandler(shaderChanged);
            _fileWatcher.EnableRaisingEvents = true;
        }
        private static void shaderChanged(object source, FileSystemEventArgs e)
        {
            OpenGLRenderer.getSingleton().isChanged = true;
            OpenGLRenderer.getSingleton().filePath = e.FullPath;
        }
    }
    public class Program
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
        private int _program;
        public int program
        {
            get
            {
                return _program;
            }
        }

        public Program(string vertexShaderPath, string fragmentShaderPath)
        {
            var fileReader = new StreamReader(vertexShaderPath);
            var vScript = fileReader.ReadToEnd();
            fileReader.Close();
            fileReader = new StreamReader(fragmentShaderPath);
            var fScript = fileReader.ReadToEnd();
            fileReader.Close();
            var vs = new Shader(vScript, ShaderType.VertexShader);
            var fs = new Shader(fScript, ShaderType.FragmentShader);
            var program = GL.CreateProgram();
            GL.AttachShader(program, vs.shaderObject);
            GL.AttachShader(program, fs.shaderObject);
            GL.LinkProgram(program);
            GL.DeleteShader(vs.shaderObject);
            GL.DeleteShader(fs.shaderObject);

            int result;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out result);
            if (result != 1)
            {
                Console.WriteLine("Could not initialise shaders");
                return;
            }

            _program = program;
            _cacheUniformLocation();
            _cacheAttribLocation();
        }
        public void enable(Dictionary<string, object> shaderValues)
        {
            GL.UseProgram(_program);
            foreach (string key in _attribIdentifers.Keys)
            {
                if (_attribIdentifers[key] != -1)
                {
                    Buffer buffer;
                    string identifer;
                    switch(key)
                    {
                        //TODO enable vertex array and set attribute value.
                        case Shader.A_VERTEXPOSITION_IDENTIFER:
                            identifer = Shader.A_VERTEXPOSITION_IDENTIFER;
                            GL.EnableVertexAttribArray(_attribIdentifers[identifer]);
                            buffer = (Buffer)shaderValues[identifer];
                            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.bufferObject);
                            GL.VertexAttribPointer(_attribIdentifers[key], buffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
                            break;
                        case Shader.A_VERTEXCOLOR_IDENTIFER:
                            identifer = Shader.A_VERTEXCOLOR_IDENTIFER;
                            GL.EnableVertexAttribArray(_attribIdentifers[identifer]);
                            buffer = (Buffer)shaderValues[identifer];
                            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.bufferObject);
                            GL.VertexAttribPointer(_attribIdentifers[key], buffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
                            break;
                        case Shader.A_TEXTURECOORD_IDENTIFER:
                            identifer = Shader.A_TEXTURECOORD_IDENTIFER;
                            GL.EnableVertexAttribArray(_attribIdentifers[identifer]);
                            buffer = (Buffer)shaderValues[identifer];
                            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.bufferObject);
                            GL.VertexAttribPointer(_attribIdentifers[key], buffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
                            break;
                    }
                }
            }
            foreach (string key in _uniformIdentifers.Keys)
            {
                if (_uniformIdentifers[key] != -1)
                {
                    //TODO the same.
                    string identifer;
                    switch (key)
                    {
                        case Shader.U_GLOBALCOLOR_IDENTIFER:
                            identifer = Shader.U_GLOBALCOLOR_IDENTIFER;
                            GL.Uniform4(_uniformIdentifers[identifer], (Vector4)shaderValues[identifer]);
                            break;
                        case Shader.U_PMATRIX_IDENTIFER:
                            identifer = Shader.U_PMATRIX_IDENTIFER;
                            var pMatrix = (Matrix4)shaderValues[identifer];
                            GL.UniformMatrix4(_uniformIdentifers[identifer], false, ref pMatrix);
                            break;
                        case Shader.U_MVMATRIX_IDENTIFER:
                            identifer = Shader.U_MVMATRIX_IDENTIFER;
                            var mvMatrix = (Matrix4)shaderValues[identifer];
                            GL.UniformMatrix4(_uniformIdentifers[identifer], false, ref mvMatrix);
                            break;

                    }
                }
            }
        }
        public void disable()
        {
            GL.UseProgram(0);
        }
        public void changeProgram(Program program)
        {
            GL.DeleteProgram(_program);
            _program = program.program;
            _cacheUniformLocation();
            _cacheAttribLocation();
        }
        private void _cacheAttribLocation()
        {
            foreach (string key in new List<string>(_attribIdentifers.Keys))
            {
                _attribIdentifers[key] = GL.GetAttribLocation(_program, key);
            }
        }
        private void _cacheUniformLocation()
        {
            foreach (string key in new List<string>(_uniformIdentifers.Keys))
            {
                _uniformIdentifers[key] = GL.GetUniformLocation(_program, key);
            }
        }
    }
    public class Effect
    {
        public int numPasses
        {
            get
            {
                return _programs.Count;
            }
        }
        private List<Program> _programs;
        public List<Program> programs
        {
            get
            {
                return _programs;
            }
        }
        private int _currIndex = -1;
        private Dictionary<string, object> _shaderValues = new Dictionary<string, object>();

        public Effect(Program[] programs)
        {
                _programs = new List<Program>(programs);
        }
        public void setShaderValue(string identifer, object value)
        {
            _shaderValues[identifer] = value;
        }

        public void beginPass(int index)
        {
            _currIndex = index;
            _programs[index].enable(_shaderValues);
        }
        public void endPass()
        {
            _programs[_currIndex].disable();
            _currIndex = -1;
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