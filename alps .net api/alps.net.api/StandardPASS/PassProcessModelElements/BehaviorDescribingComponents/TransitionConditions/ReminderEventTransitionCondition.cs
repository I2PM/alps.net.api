using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;
using System.IO;
using VDS.RDF;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a reminder event transition condition
    /// </summary>
    public class ReminderEventTransitionCondition : TimeTransitionCondition, IReminderEventTransitionCondition
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "ReminderEventTransitionCondition";

        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ReminderEventTransitionCondition();
        }

       protected ReminderEventTransitionCondition() { }
        public ReminderEventTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null, string timeValue = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, timeValue, comment, additionalLabel, additionalAttribute)
        { }

    }
}
