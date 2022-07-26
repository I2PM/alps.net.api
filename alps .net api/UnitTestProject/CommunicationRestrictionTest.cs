using alps.net.api.ALPS;
using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class CommunicationRestrictionTest
    {
        [TestMethod]
        public void parsingTest()
        {
            IList<IPASSProcessModel> models = Env.getIOHandler().loadModels(new List<string> { Env.getTestResourcePath() + "communicationRestrictionTest.owl" });
            Assert.IsTrue(models.Count > 0);
            IPASSProcessModel model = models[0];
            IList<ICommunicationRestriction> restrictions = model.getAllElements().Values.OfType<ICommunicationRestriction>().ToList();
            Assert.IsTrue(restrictions.Count > 0);
            ICommunicationRestriction restr = restrictions[0];
            Tuple<ISubject, ISubject> correspondents = restr.getCorrespondents();
            Assert.IsNotNull(correspondents.Item1);
            Assert.IsNotNull(correspondents.Item2);
        }
    }
}
