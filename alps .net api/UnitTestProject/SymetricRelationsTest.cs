using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class SymetricRelationsTest
    {

        [TestMethod]
        public void testEualsOperator()
        {
            IPASSReaderWriter io = Env.getIoHandler();
            IList<IPASSProcessModel> model =  io.loadModels(new List<string> { Env.getTestResourcePath() + "TestDrawingWithStates.owl" });
            Assert.IsTrue(model.Count > 0);
        }
    }
}
