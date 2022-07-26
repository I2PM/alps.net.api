using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a year month timer transition condition
    /// </summary>
    public class YearMonthTimerTransitionCondition : TimerTransitionCondition, IYearMonthTimerTransitionCondition
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "YearMonthTimerTransitionCondition";

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new YearMonthTimerTransitionCondition();
        }

       protected YearMonthTimerTransitionCondition() { }
        public override string getClassName()
        {
            return className;
        }
        public YearMonthTimerTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null, string timeValue = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, timeValue, comment, additionalLabel, additionalAttribute)
        { }

    }
}