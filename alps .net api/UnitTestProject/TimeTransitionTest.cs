using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestProject
{
    [TestClass]
    public class TimeTransitionTest
    {
        [TestMethod]
        public void testDifferentTimeTransitionSubtypes()
        {
            IPASSProcessModel model = Env.getGenericModel();
            ISubjectBehavior behavior = model.getAllElements().Values.OfType<IFullySpecifiedSubject>().First().getSubjectBaseBehavior();
            ITimeTransition transition = new TimeTransition(behavior);
            transition.setTimeTransitionType(ITimeTransition.TimeTransitionType.BusinessDayTimer);
            
        }
    }
}
