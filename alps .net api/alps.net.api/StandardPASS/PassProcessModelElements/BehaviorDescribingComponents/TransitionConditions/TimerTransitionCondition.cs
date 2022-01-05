using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;
using System.IO;
using VDS.RDF;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a timer transition condition 
    /// </summary>
    public class TimerTransitionCondition : TimeTransitionCondition, ITimerTransitionCondition
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "TimerTransitionCondition";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new TimerTransitionCondition();
        }

       protected TimerTransitionCondition() { }
        public TimerTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null, string timeValue = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, timeValue, comment, additionalLabel, additionalAttribute)
        { }
    }
}
