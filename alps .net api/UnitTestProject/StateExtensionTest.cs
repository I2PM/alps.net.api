
using Microsoft.VisualStudio.TestTools.UnitTesting;
using alps.net.api.StandardPASS;
using System.Linq;
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


            Env.getIOHandler().exportModel(model, Env.getTestResourceGeneratePath(this) + "testSimpleExtends", out IGraph graph);
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


            IFullySpecifiedSubject Reviewer = new FullySpecifiedSubject(model.getBaseLayer());
            ISubject Gruppe1 = new FullySpecifiedSubject(model.getBaseLayer());
            ISendTransition sendTransition = new SendTransition(Reviewer.getBehaviors().Values.First());

            IMessageExchange ReviewerGruppe1 = new MessageExchange(model.getBaseLayer());
            ReviewerGruppe1.setSender(Reviewer);
            ReviewerGruppe1.setReceiver(Gruppe1);

            ReviewerGruppe1.setMessageType(new MessageSpecification(model.getBaseLayer(), "Review completed", null, null, "Review completed"));
            ISendTransitionCondition sendInvitationCompleted = new SendTransitionCondition(sendTransition, "Review completed", null, ReviewerGruppe1, 3, 1, null, Reviewer, ReviewerGruppe1.getMessageType());

            IMessageExchange ReviewerGruppe1_2 = new MessageExchange(model.getBaseLayer());
            ReviewerGruppe1_2.setSender(Reviewer);
            ReviewerGruppe1_2.setReceiver(Gruppe1);
            ReviewerGruppe1_2.setMessageType(new MessageSpecification(model.getBaseLayer(), "Time out", null, null, "Time out"));
            ISendTransitionCondition sendTimeOutMessage = new SendTransitionCondition(sendTransition, "Time out", null, ReviewerGruppe1_2, 3, 0, null, Reviewer, ReviewerGruppe1_2.getMessageType());
            
        }
    }
}