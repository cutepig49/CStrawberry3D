using CStrawberry3D.component;
using CStrawberry3D.loader;
using CStrawberry3D.renderer;
using CStrawberry3D.shader;
using CStrawberry3D.core;
using System.IO;
using OpenTK;
using System;

namespace CStrawberry3D
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>


        [STAThread]
        static void Main()
        {
            var renderer = OpenGLRenderer.getSingleton();
            renderer.init("CStrawberry3D", 1024, 768, true);


            var file = "Tiger.x";

            Loader.getSingleton().loadAsset(file);
            var cubeMesh = Loader.getSingleton().getMesh(file);
            var component = new MeshComponent(cubeMesh);
            renderer.scene.camera.z = 1;

            var cubeNode = renderer.scene.root.createChild();
            cubeNode.addComponent(component);
            cubeNode.z = -5;
            cubeNode.y = 1;

            var directionalLight = renderer.scene.root.createChild();
            directionalLight.rx = Mathf.degreeToRadian(90);
            var lightComponent = new DirectionalLightComponent();
            directionalLight.addComponent(lightComponent);

            directionalLight = renderer.scene.root.createChild();
            directionalLight.addComponent(lightComponent);

            renderer.scene.ambientColor = new Vector4(0, 0, 0, 1);

            renderer.run();
        }
    }
}
