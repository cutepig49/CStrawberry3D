using CStrawberry3D.Component;
using CStrawberry3D.Core;
using System.IO;
using OpenTK;
using System;
using CStrawberry3D.TK;
using CStrawberry3D.Interface;


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
            var renderer = TKRenderer.Singleton;
            renderer.Init("CStrawberry3D", 1024, 768);

            var file = "Tiger.x";
            renderer.Loader.LoadAsset(file);
            var mesh = renderer.Loader.GetMesh(file);
            var node = renderer.Scene.Root.CreateChild();
            node.AddComponent(new MeshComponent(mesh));

            renderer.Scene.Camera.Z = 10;

            var d = renderer.Scene.AddDirectionalLight();
            d.Ry = Mathf.DegreeToRadian(90);

            //d = renderer.Scene.CreateDirectionalLight();
            d.Rx = Mathf.DegreeToRadian(90);

            renderer.UpdateFrame = Update;

            renderer.Run();
        }
        static void LoadAsset()
        {
        }
        static void Update(TKRenderer renderer, float dt)
        {
            if (renderer.Input.KeyDown(Key.Up))
            {
                renderer.Scene.Root.GetAll()[1].TranslateY(dt);
            }
            if (renderer.Input.KeyDown(Key.Down))
            {
                renderer.Scene.Root.GetAll()[1].TranslateY(-dt);
            }
            if (renderer.Input.KeyDown(Key.Left))
            {
                renderer.Scene.Root.GetAll()[1].ScaleX(dt);
            }
            if (renderer.Input.KeyDown(Key.Right))
            {
               renderer.Scene.Root.GetAll()[1].ScaleX(-dt);
            }
            if (renderer.Input.KeyDown(Key.PageDown))
            {
                renderer.Scene.Root.GetAll()[1].TranslateZ(dt);
            }
            if (renderer.Input.KeyDown(Key.PageUp))
            {
                renderer.Scene.Root.GetAll()[1].TranslateZ(-dt);
            }

            if (renderer.Input.MouseRB)
            {
                renderer.Scene.Camera.RotateX(Mathf.DegreeToRadian(renderer.Input.DeltaY));
                renderer.Scene.Camera.RotateY(Mathf.DegreeToRadian(renderer.Input.DeltaX));
            }
            if (renderer.Input.MouseLB)
            {
                renderer.Scene.Root.GetAll()[1].RotateX(Mathf.DegreeToRadian(renderer.Input.DeltaY));
                renderer.Scene.Root.GetAll()[1].RotateY(Mathf.DegreeToRadian(renderer.Input.DeltaX));
            }
            if (renderer.Input.KeyDown(Key.Space))
            {
                renderer.GBuffer.Textures[0].SaveToPng("debug\\position.png");
                renderer.GBuffer.Textures[1].SaveToPng("debug\\diffuse.png");
                renderer.GBuffer.Textures[2].SaveToPng("debug\\normal.png");
                renderer.GBuffer.DepthTexture.SaveToPng("debug\\depth.png");
            }
        }
    }
}
