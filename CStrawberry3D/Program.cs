using System;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CStrawberry3D.component;
using CStrawberry3D.renderer;
using CStrawberry3D.scene;
using CStrawberry3D.loader;
using CStrawberry3D.shader;

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

            Loader.getSingleton().loadAsset("cube.nff");

            var cubeNode = renderer.scene.root.createChild();
            cubeNode.translateZ(-10);

            var cubeMesh = Loader.getSingleton().getMesh("Cube");
            var colorMaterial = new BasicColorMaterial();
            var component = new MeshComponent(cubeMesh, colorMaterial);

            var child = cubeNode.createChild();
            child.translateX(5);
            child.addComponent(component);

            cubeNode.addComponent(component);


            renderer.run();
        }
    }
}
