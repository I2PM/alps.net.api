using alps.net.api.ALPS;
using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestProject
{
    /// <summary>
    /// Test class that checks the import of a model which does not contain layers, but subjects with multiple behaviors.
    /// According to ALPS, these behaviors must be split up in multiple layers
    /// </summary>
    [TestClass]
    public class TestImportNoLayers
    {
        [TestMethod]
        public void test()
        {
            var models = Env.getIOHandler().loadModels(new List<string> { Env.getTestResourcePath() + "TestNoLayers.owl" });
            var model = models[0];
            Assert.IsTrue(model.getAllElements().Values.OfType<IModelLayer>().Count() == 2);

            // Check if the layers are created successfully
            foreach (var layer in model.getAllElements().Values.OfType<IModelLayer>())
            {
                if (layer.getLayerType() == IModelLayer.LayerType.EXTENSION)
                {
                    Assert.IsTrue(layer.getElements().Values.OfType<ISubjectExtension>().Count() == 1);
                    Assert.IsTrue(layer.getElements().Values.OfType<ISubjectBehavior>().Count() == 1);
                    Assert.IsTrue(layer.getElements().Values.Count() == 2);
                }
                else if (layer.getLayerType() == IModelLayer.LayerType.STANDARD)
                {
                    Assert.IsTrue(layer.getElements().Values.OfType<ISubject>().Count() == 1);
                    Assert.IsTrue(layer.getElements().Values.OfType<ISubjectBehavior>().Count() == 1);
                    Assert.IsTrue(layer.getElements().Values.Count() == 2);
                }
            }
        }
    }
}
