using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CStrawberry3D.Core;
using CStrawberry3D.Component;
using CStrawberry3D.renderer;
using CStrawberry3D.shader;
using OpenTK;

namespace StrawberryUnitTest
{
    [TestClass]
    public class TestCase
    {
        private OpenGLRenderer _renderer;
        [TestInitialize]
        public void TestInit()
        {
            _renderer = OpenGLRenderer.getSingleton();
            _renderer.init("UnitTest", 800, 600, true);
        }
        [TestMethod]
        public void TestStrawberryNodeGuidAndId()
        {
            var node1 = _renderer.scene.root;
            var node2 = node1.CreateChild();

            Assert.AreEqual(true, node1.id != node2.id);
            Assert.AreEqual(true, node1.guid != node2.guid);
        }
        [TestMethod]
        public void TestComponentGuid()
        {
            EmptyComponent component1 = new EmptyComponent();
            EmptyComponent component2 = new EmptyComponent();

            Assert.AreEqual(false, component1.guid == component2.guid);
        }

        [TestMethod]
        public void TestTranslationWithMatrix()
        {
            var parent = new StrawberryNode();
            var child = parent.CreateChild();
            var grandchild = child.CreateChild();

            parent.TranslateX(10);
            child.TranslateY(5);
            grandchild.TranslateZ(-10);

            grandchild.UpdateWorldMatrix();

            Assert.AreEqual(new Vector3(10, 5, -10), grandchild.matrixWorld.ExtractTranslation());
            Assert.AreEqual(new Vector3(10, 5, 0), child.matrixWorld.ExtractTranslation());
            Assert.AreEqual(new Vector3(10, 0, 0), parent.matrixWorld.ExtractTranslation());
        }
        [TestMethod]
        public void TestComponentName()
        {
            var emptyComponent = new EmptyComponent();
            var cameraComponent = new CameraComponent();
            var directionalLightComponent = new DirectionalLightComponent();

            Assert.AreEqual("EmptyComponent", emptyComponent.name);
            Assert.AreEqual("CameraComponent", cameraComponent.name);
            Assert.AreEqual("DirectionalLightComponent", directionalLightComponent.name);
        }
        [TestMethod]
        public void TestStrawberryNodeSetAndGet()
        {
            var node = new StrawberryNode();
            node.X = 10;
            node.Y = 10;
            node.Z = 10;
            node.Rx = 10;
            node.Ry = 10;
            node.Rz = 10;

            Assert.AreEqual(10, node.translation.X);
            Assert.AreEqual(true, node.translation.X == node.X);
            Assert.AreEqual(10, node.translation.Y);
            Assert.AreEqual(true, node.translation.Y == node.Y);
            Assert.AreEqual(10, node.translation.Z);
            Assert.AreEqual(true, node.translation.Z == node.Z);
            Assert.AreEqual(10, node.rotation.X);
            Assert.AreEqual(true, node.rotation.X == node.Rx);
            Assert.AreEqual(10, node.rotation.Y);
            Assert.AreEqual(true, node.rotation.Y == node.Ry);
            Assert.AreEqual(10, node.rotation.Z);
            Assert.AreEqual(true, node.rotation.Z == node.Rz);
        }
        [TestMethod]
        public void TestDegreeAndRadianConverter()
        {
            Assert.AreEqual(CStrawberry3D.Core.Mathf.PI, CStrawberry3D.Core.Mathf.DegreeToRadian(180));
            Assert.AreEqual(180, CStrawberry3D.Core.Mathf.RadianToDegree(CStrawberry3D.Core.Mathf.PI));

        }
        [TestMethod]
        public void TestShadersCompiling()
        {
            Assert.AreNotEqual(null, ShaderManager.GlobalColorEffect);
            Assert.AreNotEqual(null, ShaderManager.BasicColorEffect);
            Assert.AreNotEqual(null, ShaderManager.TexturedEffect);
        }
    }
}
