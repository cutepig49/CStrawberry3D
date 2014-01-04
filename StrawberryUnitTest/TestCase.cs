using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CStrawberry3D.core;
using CStrawberry3D.component;
using CStrawberry3D.renderer;
using OpenTK;

namespace StrawberryUnitTest
{
    [TestClass]
    public class TestCase
    {
        [TestInitialize]
        public void TestInit()
        {
            var renderer = OpenGLRenderer.getSingleton();
            renderer.init("UnitTest", 800, 600);
        }
        [TestMethod]
        public void TestStrawberryNodeGuidAndId()
        {
            StrawberryNode node1 = new StrawberryNode();
            var node2 = node1.createChild();

            Assert.AreEqual(0, node1.id);
            Assert.AreEqual(1, node2.id);
            Assert.AreEqual(false, node1.id == node2.id);
            Assert.AreEqual(false, node1.guid == node2.guid);
        }
        [TestMethod]
        public void TestComponentGuid()
        {
            Component component1 = new Component();
            Component component2 = new Component();

            Assert.AreEqual(false, component1.guid == component2.guid);
        }

        [TestMethod]
        public void TestTranslationAndRotationWithMatrix()
        {
            StrawberryNode parent = new StrawberryNode();
            var child = parent.createChild();
            var child2 = child.createChild();

            parent.translate(new Vector3(5, 5, 10));
            child.translate(new Vector3(0, 0, 10));
            child2.translate(new Vector3(10, 0, 0));
            child2.updateWorldMatrix();
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

            Assert.AreEqual(true, node.translation.X == 10);
            Assert.AreEqual(true, node.translation.X == node.x);
            Assert.AreEqual(true, node.translation.Y == 10);
            Assert.AreEqual(true, node.translation.Y == node.y);
            Assert.AreEqual(true, node.translation.Z == 10);
            Assert.AreEqual(true, node.translation.Z == node.z);
            Assert.AreEqual(true, node.rotation.X == 10);
            Assert.AreEqual(true, node.rotation.X == node.rx);
            Assert.AreEqual(true, node.rotation.Y == 10);
            Assert.AreEqual(true, node.rotation.Y == node.ry);
            Assert.AreEqual(true, node.rotation.Z == 10);
            Assert.AreEqual(true, node.rotation.Z == node.rz);
        }
    }
}
