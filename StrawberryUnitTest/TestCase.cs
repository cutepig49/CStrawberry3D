using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CStrawberry3D.core;
using CStrawberry3D.component;
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
            _renderer.init("UnitTest", 800, 600);
        }
        [TestMethod]
        public void TestStrawberryNodeGuidAndId()
        {
            var node1 = _renderer.scene.root;
            var node2 = node1.createChild();

            Assert.AreEqual(true, node1.id != node2.id);
            Assert.AreEqual(true, node1.guid != node2.guid);
        }
        [TestMethod]
        public void TestComponentGuid()
        {
            Component component1 = new Component();
            Component component2 = new Component();

            Assert.AreEqual(false, component1.guid == component2.guid);
        }

        [TestMethod]
        public void TestTranslationWithMatrix()
        {
            StrawberryNode parent = new StrawberryNode();
            var child = parent.createChild();
            var child2 = child.createChild();

            parent.translate(new Vector3(5, 5, 10));

            child.translate(new Vector3(0, 0, 10));

            child2.translate(new Vector3(10, 0, 0));
            child2.updateWorldMatrix();

            Assert.AreEqual(new Vector3(5, 5, 20), child.matrixWorld.ExtractTranslation());
            Assert.AreEqual(new Vector3(15, 5, 20), child2.matrixWorld.ExtractTranslation());
        }

        [TestMethod]
        public void TestStrawberryNodeSetAndGet()
        {
            StrawberryNode node = new StrawberryNode();
            node.x = 10;
            node.y = 10;
            node.z = 10;
            node.rx = 10;
            node.ry = 10;
            node.rz = 10;

            Assert.AreEqual(10, node.translation.X);
            Assert.AreEqual(true, node.translation.X == node.x);
            Assert.AreEqual(10, node.translation.Y);
            Assert.AreEqual(true, node.translation.Y == node.y);
            Assert.AreEqual(10, node.translation.Z);
            Assert.AreEqual(true, node.translation.Z == node.z);
            Assert.AreEqual(10, node.rotation.X);
            Assert.AreEqual(true, node.rotation.X == node.rx);
            Assert.AreEqual(10, node.rotation.Y);
            Assert.AreEqual(true, node.rotation.Y == node.ry);
            Assert.AreEqual(10, node.rotation.Z);
            Assert.AreEqual(true, node.rotation.Z == node.rz);
        }
        [TestMethod]
        public void TestDegreeAndRadianConverter()
        {
            Assert.AreEqual(CStrawberry3D.core.Mathf.PI, CStrawberry3D.core.Mathf.degreeToRadian(180));
            Assert.AreEqual(180, CStrawberry3D.core.Mathf.radianToDegree(CStrawberry3D.core.Mathf.PI));

        }
        [TestMethod]
        public void TestShadersLoading()
        {
            Assert.AreNotEqual(null, ShaderManager.GlobalColorFragmentShader);
            Assert.AreNotEqual(null, ShaderManager.GlobalColorVertexShader);
            Assert.AreNotEqual(null, ShaderManager.BasicColorFragmentShader);
            Assert.AreNotEqual(null, ShaderManager.BasicColorVertexShader);
            Assert.AreNotEqual(null, ShaderManager.TexturedFragmentShader);
            Assert.AreNotEqual(null, ShaderManager.TexturedVertexShader);
        }
    }
}
