
using Microsoft.VisualStudio.TestTools.UnitTesting;
using alps.net.api.StandardPASS;
using alps.net.api.StandardPASS.BehaviorDescribingComponents;
using alps.net.api.StandardPASS.InteractionDescribingComponents;
using System.Linq;
using alps.net.api.StandardPASS.SubjectBehaviors;
using alps.net.api.parsing;
using VDS.RDF;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class StateExtensionTest
    {
        private IPASSProcessModel model;
        private IFullySpecifiedSubject subj;
        private ISubjectBehavior behavior;

        [TestMethod]
        public void testSimpleExtends()
        {
            model = new PASSProcessModel("http://www.exampleTestUri.com");
            subj = new FullySpecifiedSubject(model.getBaseLayer());
            behavior = subj.getBehaviors().Values.First();
            IDoState state = new DoState(behavior);
            IMacroBehavior newBehavior = new MacroBehavior(model.getBaseLayer(), "secondLayer", subj);

            IDoState reference = new DoState(newBehavior);
            if (reference is IStateReference refState)
                refState.setReferencedState(state);


            Env.getIOHandler().exportModel(model, Env.getTestResourceGeneratePath(this) + "testSimpleExtends", out VDS.RDF.IGraph graph);
            bool found = false;
            foreach (Triple t in graph.Triples)
            {
                if (t.Subject.ToString().Contains(reference.getModelComponentID()))
                    if (t.Predicate.ToString().Contains("type"))
                    {
                        if (t.Object.ToString().Contains("StateReference"))
                        {
                            found = true;
                        }
                    }
            }
            Assert.IsTrue(found);
        }

        /// <summary>
        /// Tests if a StateReference is parsed correctly
        /// A StateReference is being converted to the kind of state it extends.
        /// In this scenario, the extended state is a DoState
        /// </summary>
        [TestMethod]
        public void testImportExtends()
        {
            IPASSReaderWriter io = Env.getIOHandler();
            model = io.loadModels(new List<string> { Env.getTestResourcePath() + "StateExtensionTest.owl" })[0];
            io.exportModel(model, Env.getTestResourceGeneratePath(this) + "testImportExtends", out VDS.RDF.IGraph graph);
            if (!model.getAllElements().TryGetValue("SBD_9_StateReference_3", out IPASSProcessModelElement referencingState)) Assert.Fail();
            if (!model.getAllElements().TryGetValue("SBD_4_DoState", out IPASSProcessModelElement initialDoState)) Assert.Fail();
            Assert.IsTrue(referencingState is IDoState);
            IState refState = (IState)referencingState;
            if (refState is IStateReference reference)
                Assert.IsTrue(reference.getReferencedState().Equals(initialDoState));
            else Assert.Fail();
        }
    }
}