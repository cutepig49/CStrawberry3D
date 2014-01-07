using CStrawberry3D.component;
using CStrawberry3D.loader;
using CStrawberry3D.renderer;
using CStrawberry3D.shader;
using System.IO;
using OpenTK;
using System;
using System.Runtime.InteropServices;

namespace CStrawberry3D
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [DllImport("Kernel32.dll")]
        static extern bool AllocConsole();


        [STAThread]
        static void Main()
        {
            AllocConsole();

            var renderer = OpenGLRenderer.getSingleton();
            renderer.init("Test", 800, 600);

            var cubeNode = renderer.scene.root.createChild();
            cubeNode.translateZ(-10);

            var cubeMesh = Loader.getSingleton().getMesh("Cube");
            var component = new MeshComponent(cubeMesh);

            cubeNode.addComponent(component);
            //cubeMesh.changeMaterial(0, ShaderManager.BasicColorProgram);

            FileStream fileReader = new FileStream("../../../Shaders/BasicColorVertexShader.glsl", FileMode.Open);

            renderer.run();
        }
    }
}
