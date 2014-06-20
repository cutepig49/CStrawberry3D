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
        static float distanceToGround = 500;
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
        static GameState state = GameState.Gaming;
        static float speed = 1000;

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
        enum GameState
        {
            Gaming,
            Eating
        }
        static void GenFlower()
        {
            var flower = createFlower();
            flowers.Add(flower);
            flower.X = random.Next(0, (int)terrain.TerrainWidth);
            flower.Z = random.Next(0, (int)terrain.TerrainLength);
            flower.Y = terrain.SampleHeight(flower.X, flower.Z) + distanceToGround + random.Next(0, 100);
            flower.MoveForce = new Vector3();
            renderer.Scene.Root.AddChild(flower);
        }
        [STAThread]
        static void Main()
        {
            renderer = TKRenderer.Create("CStrawberry3D - 鼠标左键移动右键镜头，飞过去吃花瓣", 1024, 768);
            flowers = new List<Flower>();

            terrain = TerrainComponent.Create(TKTexture.CreateFromFile("heightmap.jpg"), 100, 3, 100, 100, 100);
            terrain._terrainMesh.FirstEntry.Material = TKTexturedMaterial.Create(renderer.ShaderManager, TKTexture.CreateFromFile("grass.jpg"));

            renderer.Scene.Root.CreateChild().AddComponent(terrain);

            loadFlower();
            renderer.Loader.LoadAsset(file);
            core = Flower.Create(flowerMesh);
            core.X = terrain.TerrainWidth / 2;
            core.Z = terrain.TerrainLength / 2;
            core.RotationForce = new Vector3(0.5f, 0.5f, 0.5f);
            core.rotation = new Vector3(0.1f, 0.1f, 0.1f);
            renderer.Scene.Root.AddChild(core);


            for (int i = 0; i < 5; i++)
            {
                var flower = createFlower();
                flowers.Add(flower);
                core.AddChild(flower);
            }

            for (int i = 0; i < 10; i ++)
            {
                GenFlower();
            }

                renderer.Loader.LoadAsset("default.skybox");
            var skybox = renderer.Loader.GetSkyBox("default.skybox");

            renderer.Scene.Camera.CameraComponent.SetSkybox(skybox);
            renderer.Scene.Camera.Z = -1500;
            //renderer.Scene.Camera.Ry = 180;

            light = renderer.Scene.AddDirectionalLight();
            light.Rx = Mathf.DegreeToRadian(45);

            //renderer.Scene.AmbientLight = new Vector4();

            //renderer.Scene.Root.AddChild(test);

            renderer.UpdateFrame = Update;
            renderer.Sound.Open("bgm.mp3");
            renderer.Sound.Play(true);


            //renderer.PostProcessManager.Push(TKMotionBlurPostProcess.Create(renderer.ShaderManager));
            //renderer.PostProcessManager.Push(TKNegativePostProcess.Create(renderer.ShaderManager));




            renderer.Run();
        }
        static void LoadAsset()
        {
        }
        static void EatingUpdate(float dt)
        {
            pauseStart += dt;
            if (pauseStart > 0.2)
            {
                state = GameState.Gaming;
                pauseStart = 0;
                renderer.PostProcessManager.Pop();
                renderer.PostProcessManager.Push(TKMotionBlurPostProcess.Create(renderer.ShaderManager));
            }
        }
        static void GamingUpdate(float dt)
        {
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
                core.MoveForce += -renderer.Scene.Camera.Forward * dt * speed;
                core.RotationForce *= 2;
            }
            if (renderer.Input.KeyDown(Key.Space))
            {
                //GenFlower();
                //renderer.GBuffer.NormalTexture.SaveToPng("uDeferredNormal.png");
            }

            core.Update(dt);
            var tmpFlowers = flowers.ToArray();
            foreach (var f in tmpFlowers)
            {
                f.Update(dt);
                if ((f.translation - core.translation).Length < 100)
                {
                    state = GameState.Eating;
                    renderer.PostProcessManager.Pop();
                    renderer.PostProcessManager.Push(TKNegativePostProcess.Create(renderer.ShaderManager));
                    renderer.Scene.Root.RemoveChild(f);
                    core.AddChild(f);
                    f.translation = new Vector3();
                    f.MoveForce = new Vector3(random.Next(-100, 100), random.Next(-100, 100), random.Next(-100, 100));
                    GenFlower();
                }
            }

            renderer.Scene.Camera.translation = core.translation - -renderer.Scene.Camera.Forward * 1000;

            core.RotationForce = originalRotationForce;

            core.X = core.X < 0 ? 0 : core.X;
            core.X = core.X > terrain.TerrainWidth ? terrain.TerrainWidth : core.X;
            core.Z = core.Z < 0 ? 0 : core.Z;
            core.Z = core.Z > terrain.TerrainLength ? terrain.TerrainLength : core.Z;

            if ((core.Y - terrain.SampleHeight(core.X,core.Z))<distanceToGround)
            {
                core.Y = terrain.SampleHeight(core.X, core.Z) + distanceToGround;
            }

        }
        static void Update(float dt)
        {
            switch(state)
            {
                case GameState.Gaming:
                    GamingUpdate(dt);
                    break;
                case GameState.Eating:
                    EatingUpdate(dt);
                    break;
            }
        }
    }
}
