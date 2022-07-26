
using Microsoft.VisualStudio.TestTools.UnitTesting;
using alps.net.api.StandardPASS;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class MessageExchangeTest
    {
        private IPASSProcessModel model;
        private ISubjectBehavior behaviorHans;
        private ISubjectBehavior behaviorPeter;
        private IFullySpecifiedSubject subjPeter;
        private IFullySpecifiedSubject subjHans;

        [TestInitialize]
        public void init()
        {
            model = new PASSProcessModel("http://www.exampleTestUri.com");
            subjPeter = new FullySpecifiedSubject(model.getBaseLayer(), "Peter");
            subjHans = new FullySpecifiedSubject(model.getBaseLayer(), "Hans");
            behaviorHans = subjHans.getSubjectBaseBehavior();
            behaviorPeter = subjPeter.getSubjectBaseBehavior();
        }

        [TestMethod]
        public void testSimpleExtend()
        {
            IMessageExchange exchange = new MessageExchange(model.getBaseLayer());
            IMessageSpecification spec = new MessageSpecification(model.getBaseLayer());
            spec.setContainedPayloadDescription(new PayloadDescription(model));
            exchange.setMessageType(spec);
            exchange.setReceiver(subjPeter);
            exchange.setSender(subjHans);
            ISendState sendState = new SendState(behaviorHans, "SendState");
            IDoState doState = new DoState(behaviorHans, "DoState");
            ISendTransition trans = new SendTransition(sendState, doState);
            ISendTransitionCondition cond = new SendTransitionCondition(trans, "cond", null, exchange, 0, 0, null, subjHans, spec);
            model.removeElement(exchange.getModelComponentID());
            Assert.IsTrue(cond.getRequiresPerformedMessageExchange() is null);
        }

        [TestMethod]
        public void testMessageRename()
        {
            IMessageExchange exchange = new MessageExchange(model.getBaseLayer());
            IMessageSpecification spec = new MessageSpecification(model.getBaseLayer(), "myMessage1", null, null, "Hallo");
            Assert.IsTrue(spec.getModelComponentLabelsAsStrings().Contains("Hallo"));
            spec.setModelComponentLabels(new List<string> { "Bye" });
            Assert.IsFalse(spec.getModelComponentLabelsAsStrings().Contains("Hallo"));
            Assert.IsTrue(spec.getModelComponentLabelsAsStrings().Contains("Bye"));
        }
    }
}
