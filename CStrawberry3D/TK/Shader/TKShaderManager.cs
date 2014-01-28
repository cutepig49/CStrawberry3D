using OpenTK.Graphics.OpenGL;
using OpenTK;
using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using CStrawberry3D.Core;


namespace CStrawberry3D.TK
{
    public enum UniformIdentifer
    {
        uPMatrix,
        uMVMatrix,
        uVMatrix,
        uNMatrix,
        uGlobalColor,
        uSamplers,
        uNumSamplers,
        uAmbientColor,
        uDiffuseColor,
        uSpecularColor,
        uSpecularPower,
        uAmbientLight,
        uDirections,
        uDirectionalLights,
        uNumDirections,
        uPoints,
        uPointLights,
        uNumPoints,
        uPositionIndex,
        uDiffuseIndex,
        uNormalIndex,
        uDeferredPosition,
        uDeferredDiffuse,
        uDeferredNormal,
        uClearColor
    }
    public enum AttributeIdentifer
    {
        aVertexPosition,
        aTextureCoord,
        aVertexColor,
        aVertexNormal
    }

    public class TKShaderManager
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
        public const string deferredVertexShaderPath = shaderDir + "DeferredVertexShader.glsl";
        public const string deferredFragmentShaderPath = shaderDir + "DeferredFragmentShader.glsl";


        //public readonly static Shader GlobalColorVertexShader;
        //public readonly static Shader GlobalColorFragmentShader;
        //public readonly static Shader BasicColorVertexShader;
        //public readonly static Shader BasicColorFragmentShader;
        //public readonly static Shader TexturedVertexShader;
        //public readonly static Shader TexturedFragmentShader;

        public TKProgram GlobalColorProgram { get; private set; }
        public TKProgram BasicColorProgram { get; private set; }
        public TKProgram TexturedProgram { get; private set; }
        public TKProgram TexturedPhongProgram { get; private set; }

        public TKEffect GlobalColorEffect{get;private set;}
        public TKEffect BasicColorEffect{get;private set;}
        public TKEffect TexturedEffect{get;private set;}
        public TKEffect TexturedPhongEffect{get;private set;}
        public TKEffect DeferredEffect { get; private set; }

        static FileSystemWatcher _fileWatcher;
        public TKShaderManager()
        {
            _fileWatcher = new FileSystemWatcher();
            CompileAllShader();
            _AddFileWatcher();
        }
        public void RecompileAllShader()
        {
            GlobalColorEffect.Dispose();
            BasicColorEffect.Dispose();
            TexturedEffect.Dispose();
            TexturedPhongEffect.Dispose();
            DeferredEffect.Dispose();
            CompileAllShader();
        }
        public void CompileAllShader()
        {
            GlobalColorProgram = new TKProgram(globalColorVertexShaderPath, globalColorFragmentShaderPath);
            GlobalColorEffect = new TKEffect(new TKProgram[] { GlobalColorProgram });

            BasicColorProgram = new TKProgram(basicColorVertexShaderPath, basicColorFragmentShaderPath);
            BasicColorEffect = new TKEffect(new TKProgram[] { BasicColorProgram });

            TexturedProgram = new TKProgram(texturedVertexShaderPath, texturedFragmentShaderPath);
            TexturedEffect = new TKEffect(new TKProgram[] { TexturedProgram });

            TexturedPhongProgram = new TKProgram(texturedPhongVertexShaderPath, texturedPhongFragmentShaderPath);
            TexturedPhongEffect = new TKEffect(new TKProgram[] { TexturedPhongProgram });

            var DeferredProgram = new TKProgram(deferredVertexShaderPath, deferredFragmentShaderPath);
            DeferredEffect = new TKEffect(new TKProgram[] { DeferredProgram });
        }
        void _AddFileWatcher()
        {
            _fileWatcher.Path = shaderDir;
            _fileWatcher.Filter = "*.glsl";
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Changed += new FileSystemEventHandler(_ShaderChanged);
            _fileWatcher.EnableRaisingEvents = true;
        }
        void _ShaderChanged(object source, FileSystemEventArgs e)
        {
            //等待片刻，防止发生IO独占错误。
            Thread.Sleep(200);
            TKRenderer.Singleton.ShaderChanged(e.FullPath);
            //Hack method, to avoid this event fired twice. 
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.EnableRaisingEvents = true;
        }
    }
}