using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;
using System.IO;
using VDS.RDF;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a sending failed condition
    /// </summary>
    public class SendingFailedCondition : TransitionCondition, ISendingFailedCondition
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "SendingFailedCondition";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendingFailedCondition();
        }

       protected SendingFailedCondition() { }
        public SendingFailedCondition(ITransition transition, string labelForID = null,  string toolSpecificDefintion = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(transition, labelForID,  toolSpecificDefintion, comment, additionalLabel, additionalAttribute) { }


    }
}
