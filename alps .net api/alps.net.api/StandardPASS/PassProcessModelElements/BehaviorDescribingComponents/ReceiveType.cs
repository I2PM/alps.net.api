using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;
using System.IO;
using VDS.RDF;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a receive type
    /// </summary>
    public class ReceiveType : BehaviorDescribingComponent, IReceiveType
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "ReceiveType";

        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ReceiveType();
        }

       protected ReceiveType() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="additionalAttribute"></param>
        public ReceiveType(ISubjectBehavior behavior, string label, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, label, comment, additionalLabel, additionalAttribute) { }


    }
}