using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    public class DynamicSubjectExecutionMapping : SubjectExecutionMapping, IDynamicSubjectExecutionMapping
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DynamicSubjectExecutionMapping";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DynamicSubjectExecutionMapping();
        }

       protected DynamicSubjectExecutionMapping() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="messageSpecification"></param>
        /// <param name="senderSubject"></param>
        /// <param name="receiverSubject"></param>
        /// <param name="additionalAttribute"></param>
        public DynamicSubjectExecutionMapping(IModelLayer layer, string labelForID = null, string executionMapping = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, executionMapping, additionalLabel, additionalAttribute)
        { }
    }
}
