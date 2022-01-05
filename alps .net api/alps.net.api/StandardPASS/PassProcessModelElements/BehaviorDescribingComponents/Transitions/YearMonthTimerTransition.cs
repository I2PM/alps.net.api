using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a year month timer transition 
    /// </summary>
    public class YearMonthTimerTransition : TimerTransition, IYearMonthTimerTransition
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "YearMonthTimerTransition";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new YearMonthTimerTransition();
        }

       protected YearMonthTimerTransition() { }
        public YearMonthTimerTransition(IState sourceState, IState targetState, string labelForID = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null) : base(sourceState, targetState, labelForID, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) { }

        public YearMonthTimerTransition(ISubjectBehavior behavior, string label = null,
            IState sourceState = null, IState targetState = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, label, sourceState, targetState, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) { }
    }

}