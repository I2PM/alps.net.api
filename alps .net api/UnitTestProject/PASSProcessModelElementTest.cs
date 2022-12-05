using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using VDS.RDF;

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
            model.addStartSubject(subj);
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

        [TestMethod]
        public void testChangeModelComponentID()
        {
            string testID = "meineTestID";
            model.setModelComponentID(testID);
            Assert.AreEqual(testID, model.getModelComponentID());
            string testIDWithSpaces = "meine Test ID";
            model.setModelComponentID(testIDWithSpaces);
            Assert.AreEqual(testIDWithSpaces.Replace(" ", "_"), model.getModelComponentID());
            model.createUniqueModelComponentID();
            Assert.AreNotEqual(testIDWithSpaces.Replace(" ", "_"), model.getModelComponentID());
            Env.getIoHandler().exportModel(model, Env.getTestResourceGeneratePath(this) + "ModelWithID", out IGraph graph);
            Triple modelComponentIDTriple = null;
            int count = 0;
            foreach (Triple t in graph.Triples)

                if (t.Predicate.ToString().Contains("hasModelComponentID") && t.Subject.ToString().Contains(model.getModelComponentID()))
                {
                    count++;
                    modelComponentIDTriple = t;
                }
            Assert.AreEqual(count, 1);
            if (modelComponentIDTriple.Object is ILiteralNode litNode)
                Assert.AreEqual(litNode.Value, model.getModelComponentID());
        }

        public void testCreateWithLabel()
        {
            string testID = "meineTestID";
            PASSProcessModelElement elem = new PASSProcessModelElement(testID);
            Assert.AreEqual(testID, elem.getModelComponentID());
            Assert.AreEqual(testID, elem.getModelComponentLabels()[0]);
        }


        [TestMethod]
        public void testChangeBaseUri()
        {
            // TODO Uri change does not affect triples 
            model.setBaseURI("http://www.newbaseuri.com");

        }
    }
}
