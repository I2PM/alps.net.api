
using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class SomeTest
    {
        [TestMethod]
        public void main()
        {

            var models = Env.getIoHandler().loadModels(new List<string> { Env.getTestResourcePath() + "Drawing3.owl" });
            Env.getIoHandler().exportModel(models[0], Env.getTestResourceGeneratePath(this) + "Drawing3", out VDS.RDF.IGraph graph);

            // TODO doppelte Hashtags

        }
        [TestMethod]
        public void anotherMethod()
        {

            IPASSProcessModel model = new PASSProcessModel("http://subjective-me.jimdo.com/s-bpm/processmodels/2022-05-18/tim/", "Drawing1");
            //model.createUniqueModelComponentID();
            Console.WriteLine(Env.getIoHandler().exportModel(model, Env.getTestResourceGeneratePath(this) + "Drawing1"));
        }

    }
}
