using OpenTK.Graphics.OpenGL;
using OpenTK;
using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using CStrawberry3D.loader;
using CStrawberry3D.renderer;
using CStrawberry3D.core;

using Buffer = CStrawberry3D.loader.Buffer;

namespace CStrawberry3D.shader
{
    public static class ShaderManager
    {
        public const string shaderDir = @"..\..\..\Shaders\";
        public const string globalColorVertexShaderPath = shaderDir + "GlobalColorVertexShader.glsl";
        public const string globalColorFragmentShaderPath = shaderDir + "GlobalColorFragmentShader.glsl";
        public const string basicColorVertexShaderPath = shaderDir + "BasicColorVertexShader.glsl";
        public const string basicColorFragmentShaderPath = shaderDir + "BasicColorFragmentShader.glsl";
        public const string texturedVertexShaderPath = shaderDir + "TexturedVertexShader.glsl";
        public const string texturedFragmentShaderPath = shaderDir + "TexturedFragmentShader.glsl";
        public const string texturedPhongVertexShaderPath = shaderDir + "TexturedPhongVertexShader.glsl";
        public const string texturedPhongFragmentShaderPath = shaderDir + "TexturedPhongFragmentShader.glsl";

        //public readonly static Shader GlobalColorVertexShader;
        //public readonly static Shader GlobalColorFragmentShader;
        //public readonly static Shader BasicColorVertexShader;
        //public readonly static Shader BasicColorFragmentShader;
        //public readonly static Shader TexturedVertexShader;
        //public readonly static Shader TexturedFragmentShader;

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
                    _globalColorProgram = value;
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
                    _basicColorProgram = value;
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
                    _texturedProgram = value;
            }

        }
        private static Program _texturedPhongProgram;
        public static Program texturedPhongProgram
        {
            get
            {
                return _texturedPhongProgram;
            }
            set
            {
                if (value != null)
                    _texturedPhongProgram = value;
            }
        }

        public readonly static Effect GlobalColorEffect;
        public readonly static Effect BasicColorEffect;
        public readonly static Effect TexturedEffect;
        public readonly static Effect TexturedPhongEffect;

        private static FileSystemWatcher _fileWatcher = new FileSystemWatcher();
        static ShaderManager()
        {
            _globalColorProgram = new Program(globalColorVertexShaderPath, globalColorFragmentShaderPath);
            GlobalColorEffect = new Effect(new Program[]{_globalColorProgram});

            _basicColorProgram = new Program(basicColorVertexShaderPath, basicColorFragmentShaderPath);
            BasicColorEffect = new Effect(new Program[] { _basicColorProgram });

            _texturedProgram = new Program(texturedVertexShaderPath, texturedFragmentShaderPath);
            TexturedEffect = new Effect(new Program[] { _texturedProgram });

            _texturedPhongProgram = new Program(texturedPhongVertexShaderPath, texturedPhongFragmentShaderPath);
            TexturedPhongEffect = new Effect(new Program[] { _texturedPhongProgram });

            _fileWatcher.Path = shaderDir;
            _fileWatcher.Filter = "*.glsl";
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Changed += new FileSystemEventHandler(shaderChanged);
            _fileWatcher.EnableRaisingEvents = true;
        }
        private static void shaderChanged(object source, FileSystemEventArgs e)
        {
            //等待片刻，防止发生IO独占错误。
            Thread.Sleep(100);
            OpenGLRenderer.getSingleton().isChanged = true;
            OpenGLRenderer.getSingleton().filePath = e.FullPath;
        }
    }
    public class Program
    {
        private Dictionary<string, int> _uniformIdentifers = new Dictionary<string, int>(){
            {Shader.U_MVMATRIX_IDENTIFER, -1},
            {Shader.U_PMATRIX_IDENTIFER, -1},
            {Shader.U_VMATRIX_IDENTIFER, -1},
            {Shader.U_NMATRIX_IDENTIFER, -1},
            {Shader.U_SAMPLERS_IDENTIFER, -1},
            {Shader.U_NUM_SAMPLERS_IDENTIFER, -1},
            {Shader.U_GLOBAL_COLOR_IDENTIFER, -1},
            {Shader.U_AMBIENT_COLOR_IDENTIFER, -1},
            {Shader.U_DIFFUSE_COLOR_IDENTIFER, -1},
            {Shader.U_DIRECTIONS, -1},
            {Shader.U_NUM_DIRECTIONS_IDENTIFER, -1},
            {Shader.U_DIRECTIONAL_LIGHTS_IDENTIFER, -1},
            {Shader.U_AMBIENT_LIGHT_IDENTIFER, -1}
        };
        public Dictionary<string, int> uniformIdentifer
        {
            get
            {
                return _uniformIdentifers;
            }
        }
        private Dictionary<string, int> _attribIdentifers = new Dictionary<string, int>(){
            {Shader.A_TEXTURECOORD_IDENTIFER, -1},
            {Shader.A_VERTEXCOLOR_IDENTIFER, -1},
            {Shader.A_VERTEXPOSITION_IDENTIFER, -1},
            {Shader.A_VERTEXNORMAL_IDENTIFER, -1}
        };
        public Dictionary<string, int> attribIdentifers
        {
            get
            {
                return _attribIdentifers;
            }
        }
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
                Logger.getSingleton().error("Could not initialise shaders.");
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
                        case Shader.A_VERTEXNORMAL_IDENTIFER:
                            identifer = Shader.A_VERTEXNORMAL_IDENTIFER;
                            GL.EnableVertexAttribArray(_attribIdentifers[identifer]);
                            buffer = (Buffer)shaderValues[identifer];
                            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.bufferObject);
                            GL.VertexAttribPointer(_attribIdentifers[key], buffer.itemSize, VertexAttribPointerType.Float, false, 0, 0);
                            break;
                        default:
                            Logger.getSingleton().error(string.Format("Identifer '{0}' lost!", key));
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
                    List<float> floats;
                    switch (key)
                    {
                        case Shader.U_GLOBAL_COLOR_IDENTIFER:
                            identifer = Shader.U_GLOBAL_COLOR_IDENTIFER;
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
                        case Shader.U_VMATRIX_IDENTIFER:
                            identifer = Shader.U_VMATRIX_IDENTIFER;
                            var vMatrix = (Matrix4)shaderValues[identifer];
                            GL.UniformMatrix4(_uniformIdentifers[identifer], false, ref vMatrix);
                            break;
                        case Shader.U_SAMPLERS_IDENTIFER:
                            identifer = Shader.U_SAMPLERS_IDENTIFER;
                            var samplers = (int[])shaderValues[identifer];
                            GL.Uniform1(_uniformIdentifers[identifer], samplers.Length, samplers);
                            break;
                        case Shader.U_NUM_SAMPLERS_IDENTIFER:
                            identifer = Shader.U_NUM_SAMPLERS_IDENTIFER;
                            var numSamplers = (int)shaderValues[identifer];
                            GL.Uniform1(_uniformIdentifers[identifer], numSamplers);
                            break;
                        case Shader.U_NUM_DIRECTIONS_IDENTIFER:
                            identifer = Shader.U_NUM_DIRECTIONS_IDENTIFER;
                            var numDirections = (int)shaderValues[identifer];
                            GL.Uniform1(_uniformIdentifers[identifer], numDirections);
                            break;
                        case Shader.U_DIRECTIONS:
                            identifer = Shader.U_DIRECTIONS;
                            var directions = (List<Vector3>)shaderValues[identifer];
                            floats = new List<float>();
                            foreach (var vec in directions)
                            {
                                floats.Add(vec.X);
                                floats.Add(vec.Y);
                                floats.Add(vec.Z);
                            }
                            GL.Uniform3(_uniformIdentifers[identifer], floats.Count/3, floats.ToArray());
                            break;
                        case Shader.U_NMATRIX_IDENTIFER:
                            identifer = Shader.U_NMATRIX_IDENTIFER;
                            var nMatrix = (Matrix4)shaderValues[Shader.U_NMATRIX_IDENTIFER];
                            GL.UniformMatrix4(_uniformIdentifers[Shader.U_NMATRIX_IDENTIFER], false, ref nMatrix);
                            break;
                        case Shader.U_AMBIENT_LIGHT_IDENTIFER:
                            identifer = Shader.U_AMBIENT_LIGHT_IDENTIFER;
                            var ambientLight = (Vector4)shaderValues[Shader.U_AMBIENT_LIGHT_IDENTIFER];
                            GL.Uniform4(_uniformIdentifers[Shader.U_AMBIENT_LIGHT_IDENTIFER], ambientLight);
                            break;
                        case Shader.U_DIRECTIONAL_LIGHTS_IDENTIFER:
                            identifer = Shader.U_DIRECTIONAL_LIGHTS_IDENTIFER;
                            var directionalLights = (List<Vector4>)shaderValues[Shader.U_DIRECTIONAL_LIGHTS_IDENTIFER];
                            floats = new List<float>();
                            foreach (var vec in directionalLights)
                            {
                                floats.Add(vec.X);
                                floats.Add(vec.Y);
                                floats.Add(vec.Z);
                                floats.Add(vec.W);
                            }
                            GL.Uniform4(_uniformIdentifers[identifer], floats.Count/4, floats.ToArray());
                            break;
                        default:
                            Logger.getSingleton().error(string.Format("Identifer '{0}' lost!", key));
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
            foreach (var key in _shaderValues.Keys)
            {
                if (key[0] == 'a')
                {
                    if (!_programs[_currIndex].attribIdentifers.ContainsKey(key))
                    {
                        Logger.getSingleton().error("Identifer " + key + " not cached!");
                    }
                }
                else if (key[0] == 'u')
                {
                    if (!_programs[_currIndex].uniformIdentifer.ContainsKey(key))
                    {
                        Logger.getSingleton().error("Identifer " + key + " not cached!");
                    }
                }
            }
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
        //投影矩阵
        public const string U_PMATRIX_IDENTIFER = "uPMatrix";
        //世界矩阵
        public const string U_MVMATRIX_IDENTIFER = "uMVMatrix";
        //观察矩阵
        public const string U_VMATRIX_IDENTIFER = "uVMatrix";
        //法线变换矩阵
        public const string U_NMATRIX_IDENTIFER = "uNMatrix";
        //全局颜色
        public const string U_GLOBAL_COLOR_IDENTIFER = "uGlobalColor";
        //贴图数组
        public const string U_SAMPLERS_IDENTIFER = "uSamplers";
        //贴图个数
        public const string U_NUM_SAMPLERS_IDENTIFER = "uNumSamplers";
        //材质环境光反射颜色
        public const string U_AMBIENT_COLOR_IDENTIFER = "uAmbientColor";
        //材质漫反射颜色
        public const string U_DIFFUSE_COLOR_IDENTIFER = "uDiffuseColor";
        //材质镜面反射颜色
        public const string U_SPECULAR_COLOR_IDENTIFER = "uSpecularColor";
        //环境光颜色
        public const string U_AMBIENT_LIGHT_IDENTIFER = "uAmbientLight";
        //方向光方向数组
        public const string U_DIRECTIONS = "uDirections";
        //方向光颜色数组
        public const string U_DIRECTIONAL_LIGHTS_IDENTIFER = "uDirectionalLights";
        //方向光个数
        public const string U_NUM_DIRECTIONS_IDENTIFER = "uNumDirections";
        //顶点位置
        public const string A_VERTEXPOSITION_IDENTIFER = "aVertexPosition";
        //UV坐标
        public const string A_TEXTURECOORD_IDENTIFER = "aTextureCoord";
        //顶点颜色
        public const string A_VERTEXCOLOR_IDENTIFER = "aVertexColor";
        //顶点法线
        public const string A_VERTEXNORMAL_IDENTIFER = "aVertexNormal";
        //点光源位置数组
        public const string U_POINTS_IDENTIFER = "uPoints";
        //点光源颜色数组
        public const string U_POINT_LIGHTS_IDENTIFER = "uPointLights";
        //点光源个数
        public const string U_NUM_POINTS_IDENTIFER = "uNumPoints";

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
                Logger.getSingleton().error(GL.GetShaderInfoLog(_shaderObject));
            }
        }
    }

}