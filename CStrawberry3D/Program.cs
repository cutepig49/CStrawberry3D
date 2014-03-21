using CStrawberry3D.Component;
using CStrawberry3D.Core;
using System.IO;
using OpenTK;
using System.Collections.Generic;
using System;
using CStrawberry3D.TK;
using CStrawberry3D.Interface;
using OpenTK.Graphics.OpenGL;


namespace CStrawberry3D
{
    class Flower : StrawberryNode
    {
        public Vector3 MoveForce { get; set; }
        public Vector3 RotationForce { get; set; }
        float moveRestoreFactor;
        public static Flower Create(TKMesh flowerMesh)
        {
            return new Flower(flowerMesh);
        }
        Flower(TKMesh flowerMesh)
            : base()
        {
            AddComponent(MeshComponent.Create(flowerMesh));
            MoveForce = new Vector3();
            RotationForce = new Vector3();
            moveRestoreFactor = 0.5f;
        }
        public void Update(float dt)
        {
            Move(MoveForce * dt);
            Rotate(RotationForce * dt);
            MoveForce += -MoveForce * moveRestoreFactor * dt;
        }
    }
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static bool isNegative = false;
        static float rx = 0;
        static float ry = 0;
        public static TKRenderer renderer;
        static List<Flower> flowers;
        static Flower core;
        static StrawberryNode light;
        static TKMesh flowerMesh;
        static string file = "flower.3DS";
        static TerrainComponent terrain;
        static float pauseStart = 0;

        static Vector3 coreLastPos;

        static Flower test;
        static Random random = new Random();
        static Flower createFlower()
        {
            var flower = Flower.Create(flowerMesh);
            flower.MoveForce = new Vector3(random.Next(-100, 100), random.Next(-100, 100), random.Next(-100, 100));
            flower.RotationForce = new Vector3(Mathf.DegreeToRadian(random.Next(-90, 90)), Mathf.DegreeToRadian(random.Next(-90, 90)), Mathf.DegreeToRadian(random.Next(-90, 90)));
            return flower;
        }
        static void loadFlower()
        {
            renderer.Loader.LoadAsset(file);
            flowerMesh = renderer.Loader.GetMesh(file);
            var texture = TKTexture.CreateFromFile("flower.jpg");
            flowerMesh.FirstEntry.Material = TKTexturedMaterial.Create(renderer.ShaderManager, texture);
        }
        [STAThread]
        static void Main()
        {
            renderer = TKRenderer.Create("CStrawberry3D", 1024, 768);
            flowers = new List<Flower>();

            terrain = TerrainComponent.Create(TKTexture.CreateFromFile("heightmap.jpg"), 100, 100, 100);
            terrain._terrainMesh.FirstEntry.Material = TKTexturedMaterial.Create(renderer.ShaderManager, TKTexture.CreateFromFile("flower.jpg"));

            renderer.Scene.Root.CreateChild().AddComponent(terrain);

            loadFlower();
            renderer.Loader.LoadAsset(file);
            core = Flower.Create(flowerMesh);
            core.Y = 10000;
            core.RotationForce = new Vector3(0.5f, 0.5f, 0.5f);
            core.rotation = new Vector3(0.1f, 0.1f, 0.1f);
            renderer.Scene.Root.AddChild(core);

            coreLastPos = new Vector3();

            for (int i = 0; i < 5; i++)
            {
                var flower = createFlower();
                flowers.Add(flower);
                core.AddChild(flower);
            }

            renderer.Loader.LoadAsset("default.skybox");
            var skybox = renderer.Loader.GetSkyBox("default.skybox");

            renderer.Scene.Camera.CameraComponent.SetSkybox(skybox);
            renderer.Scene.Camera.Z = -1500;
            //renderer.Scene.Camera.Ry = 180;

            light = renderer.Scene.AddDirectionalLight();
            light.Rx = Mathf.DegreeToRadian(45);

            //renderer.Scene.AmbientLight = new Vector4();

            test = Flower.Create(flowerMesh);
            //renderer.Scene.Root.AddChild(test);

            renderer.UpdateFrame = Update;
            renderer.Sound.Open("bgm.mp3");
            renderer.Sound.Play(true);

                //renderer.PostProcessManager.Push(TKMotionBlurPostProcess.Create(renderer.ShaderManager));
            renderer.PostProcessManager.Push(TKNegativePostProcess.Create(renderer.ShaderManager));



            renderer.Run();
        }
        static void LoadAsset()
        {
        }
        static void Update(float dt)
        {
            //core.RotationForce += (new Vector3(Convert.ToSingle(random.NextDouble() - 0.5), Convert.ToSingle(random.NextDouble() - 0.5), Convert.ToSingle(random.NextDouble() - 0.5)))*0.1f;
            var originalRotationForce = core.RotationForce;
            if (renderer.Input.KeyDown(Key.Escape))
            {
                renderer.Form.Exit();
            }
            if (renderer.Input.KeyDown(Key.W))
            {
                renderer.Scene.Camera.Move(-renderer.Scene.Camera.Forward);
            }
            if (renderer.Input.KeyDown(Key.S))
            {
                renderer.Scene.Camera.Move(renderer.Scene.Camera.Forward);
            }
            if (renderer.Input.MouseRB)
            {
                rx += renderer.Input.DeltaY * dt * 10;
                ry += renderer.Input.DeltaX * dt * 10;

                renderer.Scene.Camera.Rx = Mathf.DegreeToRadian(rx);
                renderer.Scene.Camera.Ry = Mathf.DegreeToRadian(ry);
            }
            if (renderer.Input.MouseLB)
            {
                core.MoveForce += -renderer.Scene.Camera.Forward * dt * 100;
                core.RotationForce *= 2;
                foreach (var f in flowers)
                {
                    if (f.MoveForce.Length > 50)
                    {
                        continue;
                    }
                    //f.MoveForce -= -renderer.Scene.Camera.Forward * dt * 5;
                }
            }
            if (renderer.Input.KeyDown(Key.Space))
            {
                //if (isNegative)
                //{
                //    renderer.PostProcessManager.Pop();
                //}
                //else
                //{
                //    renderer.PostProcessManager.Push(TKNegativePostProcess.Create(renderer.ShaderManager));
                //}
                //isNegative = !isNegative;

                var flower = createFlower();
                flowers.Add(flower);
                //core.AddChild(flower);

                //renderer.GBuffer.PositionTexture.SaveToPng("debug\\position.png");
                //renderer.GBuffer.DiffuseTexture.SaveToPng("debug\\diffuse.png");
                //renderer.GBuffer.NormalTexture.SaveToPng("debug\\normal.png");
                //renderer.GBuffer.DepthTexture.SaveToPng("debug\\depth.png");
            }
            core.Update(dt);
            foreach (var f in flowers)
            {
                f.Update(dt);
            }

            renderer.Scene.Camera.translation = core.translation - -renderer.Scene.Camera.Forward * 1000;

            coreLastPos = core.translation;

            core.RotationForce = originalRotationForce;

            Console.WriteLine(renderer.Scene.Camera.Forward);
        }
    }
}
