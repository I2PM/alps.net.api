using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a BusinessDayTimerTransitionCondition
    /// </summary>

    public class BusinessDayTimerTransitionCondition : TimerTransitionCondition, IBusinessDayTimerTransitionCondition
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "BusinessDayTimerTransitionCondition";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new BusinessDayTimerTransitionCondition();
        }

       protected BusinessDayTimerTransitionCondition() { }
        public BusinessDayTimerTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null, string timeValue = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, timeValue, comment, additionalLabel, additionalAttribute)
        { }

    }
}
