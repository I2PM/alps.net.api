using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class PASSProcessModelElementTest
    {
        private IPASSProcessModel model;
        private ISubjectBehavior behavior;

        [TestInitialize]
        public void init()
        {
            model = new PASSProcessModel("http://www.exampleTestUri.com");
            IFullySpecifiedSubject subj = new FullySpecifiedSubject(model.getBaseLayer());
            behavior = subj.getBehaviors().Values.First();
        }

        [TestMethod]
        public void testDeleteBehavior()
        {
            IState state = new State(behavior);
            Assert.IsTrue(model.getAllElements().Values.Contains(state));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(state));
            behavior.removeBehaviorDescribingComponent(state.getModelComponentID());
            Assert.IsFalse(model.getAllElements().Values.Contains(state));
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(state));
            behavior.addBehaviorDescribingComponent(state);
            Assert.IsTrue(model.getAllElements().Values.Contains(state));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(state));
            state.removeFromEverything();
            Assert.IsFalse(model.getAllElements().Values.Contains(state));
            Assert.IsFalse(model.getBaseLayer().getElements().Values.Contains(state));
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(state));
        }
    }
}
