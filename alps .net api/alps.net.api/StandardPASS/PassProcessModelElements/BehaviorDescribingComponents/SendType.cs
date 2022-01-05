using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;
using System.IO;
using VDS.RDF;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a send type
    /// </summary>
    public class SendType : BehaviorDescribingComponent, ISendType
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "SendType";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendType();
        }

       protected SendType() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="additionalAttribute"></param>
        public SendType(ISubjectBehavior behavior, string labelForID = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, labelForID, comment, additionalLabel, additionalAttribute)
        { }


    }
}
