using Microsoft.VisualStudio.TestTools.UnitTesting;
using alps.net.api.StandardPASS;
using alps.net.api.StandardPASS.BehaviorDescribingComponents;
using alps.net.api.StandardPASS.InteractionDescribingComponents;
using System.Linq;
using alps.net.api.ALPS.ALPSModelElements;
using alps.net.api.ALPS.ALPSModelElements.ALPSSIDComponents;

namespace UnitTestProject
{
    [TestClass]
    public class ExtensionTest
    {
        private IPASSProcessModel model;

        [TestInitialize]
        public void init()
        {
            model = new PASSProcessModel("http://www.exampleTestUri.com");
        }

        [TestMethod]
        public void testSimpleExtend()
        {
            IFullySpecifiedSubject subj = new FullySpecifiedSubject(model.getBaseLayer(), "Jens");
            IModelLayer extLayer = new ModelLayer(model, "ExtensionLayerForJens");
            ISubjectExtension ext = new SubjectExtension(extLayer, "ExtensionForJens", subj);
            IExtensionBehavior extBehavior = new ExtensionBehavior(extLayer, "someExtension", ext);
            Assert.IsTrue(extLayer.getLayerType().Equals(IModelLayer.LayerType.EXTENSION));
            Assert.IsTrue(subj.getBehaviors().Values.Contains(extBehavior));
        }
    }
}
