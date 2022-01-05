using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{

    /// <summary>
    /// Class that represents a day time timer transition condition 
    /// </summary>
    public class DayTimeTimerTransitionCondition : TimerTransitionCondition, IDayTimeTimerTransitionCondition
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "DayTimeTimerTransitionCondition";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DayTimeTimerTransitionCondition();
        }

       protected DayTimeTimerTransitionCondition() { }
        public DayTimeTimerTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null, string timeValue = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, timeValue, comment, additionalLabel, additionalAttribute)
        { }
    }
}
