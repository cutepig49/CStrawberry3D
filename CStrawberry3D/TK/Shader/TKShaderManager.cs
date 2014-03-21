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
        uDepthIndex,
        uDeferredPosition,
        uDeferredDiffuse,
        uDeferredNormal,
        uDeferredDepth,
        uClearColor,
        uCubeSamplers,
        uMaterialID,
        uSamplerRects
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
        public const string shaderDir = @"../../Shaders/";
        //public const string shaderDir = @"Shaders/";
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
        public const string skyboxVertexShaderPath = shaderDir + "SkyboxVertexShader.glsl";
        public const string skyboxFragmentShaderPath = shaderDir + "SkyboxFragmentShader.glsl";
        public const string screenVertexShaderPath = shaderDir + "ScreenVertexShader.glsl";
        public const string screenFragmentShaderPath = shaderDir + "ScreenFragmentShader.glsl";
        public const string greyPostProcessFragmentShaderPath = shaderDir + "GreyPostProcessFragmentShader.glsl";
        public const string negativePostProcessFragmentShaderPath = shaderDir + "NegativePostProcessFragmentShader.glsl";
        public const string reliefPostProcessFragmentShaderPath = shaderDir + "ReliefPostProcessFragmentShader.glsl";
        public const string scaleCopyPostProcessFragmentShader = shaderDir + "ScaleCopyPostProcessFragmentShader.glsl";
        public const string motionBlurPostProcessFragmentShader = shaderDir + "MotionBlurPostProcessFragmentShader.glsl";

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
        public TKEffect SkyboxEffect { get; private set; }
        public TKEffect ScreenEffect { get; private set; }
        public TKEffect GreyPostProcessEffect { get; private set; }
        public TKEffect NegativePostProcessEffect { get; private set; }
        public TKEffect ReliefPostProcessEffect { get; private set; }
        public TKEffect MotionBlurPostProcessEffect { get; private set; }
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
            SkyboxEffect.Dispose();
            ScreenEffect.Dispose();
            GreyPostProcessEffect.Dispose();
            NegativePostProcessEffect.Dispose();
            //ReliefPostProcessEffect.Dispose();
            MotionBlurPostProcessEffect.Dispose();
            CompileAllShader();
        }
        public void CompileAllShader()
        {
            GlobalColorProgram = TKProgram.Create(globalColorVertexShaderPath, globalColorFragmentShaderPath);
         
            GlobalColorEffect = TKEffect.Create(new TKProgram[] { GlobalColorProgram });

            BasicColorProgram = TKProgram.Create(basicColorVertexShaderPath, basicColorFragmentShaderPath);
            BasicColorEffect = TKEffect.Create(new TKProgram[] { BasicColorProgram });

            TexturedProgram = TKProgram.Create(texturedVertexShaderPath, texturedFragmentShaderPath);
            TexturedEffect = TKEffect.Create(new TKProgram[] { TexturedProgram });

            TexturedPhongProgram = TKProgram.Create(texturedPhongVertexShaderPath, texturedPhongFragmentShaderPath);
            TexturedPhongEffect = TKEffect.Create(new TKProgram[] { TexturedPhongProgram });

            var DeferredProgram = TKProgram.Create(deferredVertexShaderPath, deferredFragmentShaderPath);
            DeferredEffect = TKEffect.Create(new TKProgram[] { DeferredProgram });

            var skyboxProgram = TKProgram.Create(skyboxVertexShaderPath, skyboxFragmentShaderPath);
            SkyboxEffect = TKEffect.Create(new[] { skyboxProgram });

            var screenProgram = TKProgram.Create(screenVertexShaderPath, screenFragmentShaderPath);
            ScreenEffect = TKEffect.Create(new[] { screenProgram });

            var greyPostProcessProgram = TKProgram.Create(screenVertexShaderPath, greyPostProcessFragmentShaderPath);
            GreyPostProcessEffect = TKEffect.Create(new[] { greyPostProcessProgram });

            var negativePostProcessProgram = TKProgram.Create(screenVertexShaderPath, negativePostProcessFragmentShaderPath);
            NegativePostProcessEffect = TKEffect.Create(new[] { negativePostProcessProgram });

            //var reliefPostProcessProgram = TKProgram.Create(screenVertexShaderPath, reliefPostProcessFragmentShaderPath);
            //ReliefPostProcessEffect = TKEffect.Create(new[] { reliefPostProcessProgram });

            var motionBlurPostProcessProgram = TKProgram.Create(screenVertexShaderPath, motionBlurPostProcessFragmentShader);
            MotionBlurPostProcessEffect = TKEffect.Create(new[] { motionBlurPostProcessProgram });
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