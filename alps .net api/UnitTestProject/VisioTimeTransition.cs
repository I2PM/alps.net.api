using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestProject
{
    public class VisioTimeTransition : TimeTransition
    {

        public VisioTimeTransition() { }

        public VisioTimeTransition(IState sourceState, IState targetState, string labelForID = null, ITimeTransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard,
            ITimeTransition.TimeTransitionType timeTransitionType = ITimeTransition.TimeTransitionType.DayTimeTimer,
            string comment = null,
            string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(sourceState, targetState, labelForID, transitionCondition, transitionType, timeTransitionType, comment, additionalLabel, additionalAttribute)
        { }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new VisioTimeTransition();
        }

        public void exportToVisio(ISimple2DVisualizationBounds bounds = null)
        {
            // Empty
        }
    }
}
