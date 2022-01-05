using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;
using System.IO;
using VDS.RDF;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a send function
    /// </summary>
    public class SendFunction : CommunicationAct, ISendFunction
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "SendFunction";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendFunction();
        }

       protected SendFunction() { }
        public SendFunction(ISubjectBehavior behavior, string labelForID = null, string toolSpecificDefinition = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, labelForID, toolSpecificDefinition, comment, additionalLabel, additionalAttribute) { }


    }
}