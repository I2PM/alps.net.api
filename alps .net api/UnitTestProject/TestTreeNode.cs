using alps.net.api;
using alps.net.api.ALPS;
using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class TestTreeNode
    {
        [TestMethod]
        public void testNodeContains()
        {
            IPASSProcessModel myModel = new PASSProcessModel("http://www.foo.bar");
            ITreeNode<IPASSProcessModelElement> node = new TreeNode<IPASSProcessModelElement>();
            node.setChildNodes(new System.Collections.Generic.List<ITreeNode<IPASSProcessModelElement>> { new TreeNode<IPASSProcessModelElement>(myModel) });
            Assert.IsTrue(node.containsContent(myModel, out ITreeNode<IPASSProcessModelElement> outNode));
        }

        [TestMethod]
        public void testNodeTreeHeight()
        {
            IPASSProcessModel myModel = new PASSProcessModel("http://www.foo.bar");
            IModelLayer layer = new ModelLayer(myModel);
            ITreeNode<IPASSProcessModelElement> node = new TreeNode<IPASSProcessModelElement>();
            ITreeNode<IPASSProcessModelElement> modelNode = new TreeNode<IPASSProcessModelElement>(myModel);
            ITreeNode<IPASSProcessModelElement> layerNode = new TreeNode<IPASSProcessModelElement>(layer);
            ITreeNode<IPASSProcessModelElement> subjectNode = new TreeNode<IPASSProcessModelElement>(new FullySpecifiedSubject(layer));
            node.setChildNodes(new System.Collections.Generic.List<ITreeNode<IPASSProcessModelElement>> { modelNode });
            
            node.getChild(0).setChildNodes(new System.Collections.Generic.List<ITreeNode<IPASSProcessModelElement>> { layerNode });
            node.getChild(0).getChild(0).setChildNodes(new System.Collections.Generic.List<ITreeNode<IPASSProcessModelElement>> { subjectNode });
            Assert.IsTrue(node.getHeigthToLastLeaf() == 3);
            Assert.IsTrue(modelNode.getHeigthToLastLeaf() == 2);
            Assert.IsTrue(layerNode.getHeigthToLastLeaf() == 1);
            Assert.IsTrue(subjectNode.getHeigthToLastLeaf() == 0);
        }
    }
}
