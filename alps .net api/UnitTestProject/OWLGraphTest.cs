using Microsoft.VisualStudio.TestTools.UnitTesting;
using alps.net.api;
using System.Collections.Generic;
using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using alps.net.api.StandardPASS.InteractionDescribingComponents;
using alps.net.api.ALPS.ALPSModelElements;

namespace UnitTestProject
{
    [TestClass]
    public class OWLGraphTest
    {
        [TestMethod]
        public void exportTest()
        {
            List<IPASSProcessModelElement> createdElements;
            PASSReaderWriter graph = PASSReaderWriter.getInstance();
            IPASSProcessModel model = new PASSProcessModel("http://www.exapleTestUri.com");
            IFullySpecifiedSubject su = new FullySpecifiedSubject(model.getBaseLayer());
            IInterfaceSubject iS = new InterfaceSubject(model.getBaseLayer());
            IMultiSubject mu = new MultiSubject(model.getBaseLayer());
            /*model.getBaseLayer().addElement(su);
            model.getBaseLayer().addElement(iS);
            model.getBaseLayer().addElement(mu);*/
            IMessageExchange me = new MessageExchange(model.getBaseLayer(), "hallo", null, su, mu);
            /*model.getBaseLayer().addElement(me);*/
            IFullySpecifiedSubject juergen = new FullySpecifiedSubject(model.getBaseLayer(), "Juergen");
            IInterfaceSubject guenther = new InterfaceSubject(model.getBaseLayer(), "Guenther");
            IMultiSubject petra = new MultiSubject(model.getBaseLayer(), "Petra");
            /*model.getBaseLayer().addElement(juergen);
            model.getBaseLayer().addElement(guenther);
            model.getBaseLayer().addElement(petra);*/
            createdElements = new List<IPASSProcessModelElement> { su, iS, mu, me, juergen, guenther, petra };
            foreach (IPASSProcessModelElement element in createdElements)
            {
                Assert.IsTrue(model.getAllElements().Values.Contains(element));
            }

            // Using static link to the file
            //graph.exportModel(model, "C:\\Users\\Lukas\\Documents\\test");

            // Using dynamic link (saved in alps-.net-api/src/generated)
            graph.exportModel(model, Env.getTestResourceGeneratePath(this) + "exportTest");
        }

    }
}
