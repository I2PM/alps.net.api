
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class SomeTest
    {
        [TestMethod]
        public void main()
        {
            var models = Env.getIOHandler().loadModels(new List<string> { Env.getTestResourcePath() + "Drawing3.owl" });
            Env.getIOHandler().exportModel(models[0], Env.getTestResourceGeneratePath(this) + "Drawing3", out VDS.RDF.IGraph graph);

            // TODO doppelte Hashtags

        }

    }
}
