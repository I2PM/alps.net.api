
using Microsoft.VisualStudio.TestTools.UnitTesting;
using alps.net.api.StandardPASS;
using alps.net.api.StandardPASS.BehaviorDescribingComponents;
using alps.net.api.StandardPASS.InteractionDescribingComponents;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class DeleteBehaviorTest
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
        public void testSimpleDelete()
        {
            IDoState state = new DoState(behavior);
            ISendState sendState = new SendState(behavior);
            IReceiveState receiveState = new ReceiveState(behavior);
            IDoTransition doTrans = new DoTransition(state, sendState);
            ISendTransition sendTrans = new SendTransition(sendState, receiveState);
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(state));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(receiveState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(doTrans));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
            behavior.removeBehaviorDescribingComponent(sendState.getModelComponentID());
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(state));
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(receiveState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(doTrans));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
        }

        [TestMethod]
        public void testCascadeOneDeleteBehavior()
        {
            IDoState state = new DoState(behavior);
            ISendState sendState = new SendState(behavior);
            IReceiveState receiveState = new ReceiveState(behavior);
            IDoTransition doTrans = new DoTransition(state, sendState);
            ISendTransition sendTrans = new SendTransition(sendState, receiveState);
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(state));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(receiveState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(doTrans));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
            Assert.IsTrue(model.getAllElements().Values.Contains(sendState));
            Assert.IsTrue(model.getAllElements().Values.Contains(sendTrans));
            behavior.removeBehaviorDescribingComponent(sendState.getModelComponentID(), 1);
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(state));
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(receiveState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(doTrans));
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
            Assert.IsFalse(model.getAllElements().Values.Contains(sendState));
            Assert.IsFalse(model.getAllElements().Values.Contains(sendTrans));
        }

        [TestMethod]
        public void testCascadeTwoDeleteBehavior()
        {
            IDoState state = new DoState(behavior);
            ISendState sendState = new SendState(behavior);
            IReceiveState receiveState = new ReceiveState(behavior);
            IDoTransition doTrans = new DoTransition(state, sendState);
            ISendTransition sendTrans = new SendTransition(sendState, receiveState);
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
            Assert.IsTrue(model.getAllElements().Values.Contains(sendState));
            Assert.IsTrue(model.getAllElements().Values.Contains(sendTrans));
            Assert.IsTrue(model.getAllElements().Values.Contains(doTrans));
            behavior.removeBehaviorDescribingComponent(sendState.getModelComponentID(), 2);
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
            Assert.IsFalse(model.getAllElements().Values.Contains(sendState));
            Assert.IsFalse(model.getAllElements().Values.Contains(sendTrans));
            Assert.IsFalse(model.getAllElements().Values.Contains(doTrans));
            Assert.IsFalse(model.getAllElements().Values.Contains(receiveState));
            Assert.IsFalse(model.getAllElements().Values.Contains(state));
        }

        [TestMethod]
        public void testCascadeOneDeleteModel()
        {
            IDoState state = new DoState(behavior);
            ISendState sendState = new SendState(behavior);
            IReceiveState receiveState = new ReceiveState(behavior);
            IDoTransition doTrans = new DoTransition(state, sendState);
            ISendTransition sendTrans = new SendTransition(sendState, receiveState);
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
            Assert.IsTrue(model.getAllElements().Values.Contains(sendState));
            Assert.IsTrue(model.getAllElements().Values.Contains(sendTrans));
            model.removeElement(sendState.getModelComponentID(), 1);
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
            Assert.IsFalse(model.getAllElements().Values.Contains(sendState));
            Assert.IsFalse(model.getAllElements().Values.Contains(sendTrans));
        }

        [TestMethod]
        public void testCascadeTwoDelete()
        {
            IDoState state = new DoState(behavior);
            ISendState sendState = new SendState(behavior);
            IReceiveState receiveState = new ReceiveState(behavior);
            IDoTransition doTrans = new DoTransition(state, sendState);
            ISendTransition sendTrans = new SendTransition(sendState, receiveState);
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(state));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(receiveState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(doTrans));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
            behavior.removeBehaviorDescribingComponent(sendState.getModelComponentID(), 1);
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(state));
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(sendState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(receiveState));
            Assert.IsTrue(behavior.getBehaviorDescribingComponents().Values.Contains(doTrans));
            Assert.IsFalse(behavior.getBehaviorDescribingComponents().Values.Contains(sendTrans));
        }
    }
}