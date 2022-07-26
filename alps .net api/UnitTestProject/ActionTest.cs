using Microsoft.VisualStudio.TestTools.UnitTesting;
using alps.net.api.StandardPASS;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class ActionTest
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
        public void testEualsOperator()
        {
            IState state = new State(behavior);
            Assert.IsNotNull(state.getAction());
            ITransition transition = new Transition(behavior);
            state.addOutgoingTransition(transition);
            Assert.IsTrue(state.getAction().getContainedTransitions().ContainsKey(transition.getModelComponentID()));
            transition.setSourceState(null);
            Assert.IsFalse(state.getAction().getContainedTransitions().ContainsKey(transition.getModelComponentID()));
        }
    }
}
