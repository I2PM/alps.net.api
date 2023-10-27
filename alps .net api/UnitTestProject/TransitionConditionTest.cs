using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestProject
{
    [TestClass]
    public class TransitionConditionTest
    {
        [TestMethod]
        public void testHasTransitionCondition()
        {
            IPASSProcessModel model = new PASSProcessModel("http://www.testuri.de");
            IFullySpecifiedSubject subject = new FullySpecifiedSubject(model.getBaseLayer());
            IFullySpecifiedSubject subject2 = new FullySpecifiedSubject(model.getBaseLayer());
            IReceiveState receiveState1 = new ReceiveState(subject.getSubjectBaseBehavior());
            IDoState doState2 = new DoState(subject.getSubjectBaseBehavior());
            IMessageExchange messageEx2 = new MessageExchange(model.getBaseLayer());
            messageEx2.setSender(subject);
            messageEx2.setReceiver(subject2);

            IReceiveTransition recTran1 = new ReceiveTransition(receiveState1, doState2, "von empfang zu essen2");
            IReceiveTransitionCondition recTranCon1 = new ReceiveTransitionCondition(recTran1, "Receive Transition Condition", null,
                messageEx2, 0, 0, IReceiveTransitionCondition.ReceiveTypes.STANDARD, messageEx2.getSender(), messageEx2.getMessageType());
            recTran1.setTransitionCondition(recTranCon1);
            if (recTran1 is IParseablePASSProcessModelElement elem)
            {
                bool found = false;
                foreach (var triple in elem.getTriples())
                {
                    if (triple.Predicate.ToString().Contains("hasTransitionCondition"))
                    {
                        found = true;
                        break;
                    }
                }
                Assert.IsTrue(found);
            }
        }


        [TestMethod]
        public void checkDifferentTimeValues()
        {
            IPASSProcessModel model = new PASSProcessModel("http://www.testuri.de");
            IFullySpecifiedSubject subject = new FullySpecifiedSubject(model.getBaseLayer());
            ITimeTransition trans = new TimeTransition(subject.getSubjectBaseBehavior());
            trans.setTimeTransitionType(ITimeTransition.TimeTransitionType.DayTimeTimer);
            ITimeTransitionCondition cond = new TimeTransitionCondition(trans);
            cond.setTimeTransitionConditionType(ITimeTransitionCondition.TimeTransitionConditionType.DayTimeTimerTC);
            cond.setTimeValue("TimeValue");
            bool contains = false;
            if (cond is IParseablePASSProcessModelElement elem)
                foreach (var t in elem.getTriples())
                {
                    if (t.Predicate.ToString().Contains("hasDayTimeDurationTimeOutTime"))
                    {
                        contains = true;
                    }
                }
            Assert.IsTrue(contains);
        }
    }
}
