using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestProject
{
    [TestClass]
    public class TestModelTim
    {
        [TestMethod]
        public void main()
        {
            var models = Env.getIoHandler().loadModels(new List<string> { Env.getTestResourcePath() + "DrawingTimExportfehlerKopie.owl" });
            Env.getIoHandler().exportModel(models[0], Env.getTestResourceGeneratePath(this) + "DrawingTimExportfehler", out VDS.RDF.IGraph graph);
        }
    }
}
