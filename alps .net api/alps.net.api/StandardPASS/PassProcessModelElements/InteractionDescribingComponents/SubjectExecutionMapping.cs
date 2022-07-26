using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    public class SubjectExecutionMapping : InteractionDescribingComponent, ISubjectExecutionMapping
    {
        protected string executionMapping;

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "SubjectExecutionMapping";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SubjectExecutionMapping();
        }

       protected SubjectExecutionMapping() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="messageSpecification"></param>
        /// <param name="senderSubject"></param>
        /// <param name="receiverSubject"></param>
        /// <param name="additionalAttribute"></param>
        public SubjectExecutionMapping(IModelLayer layer, string labelForID = null, string executionMapping = null, string comment = null,
            string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setExecutionMapping(executionMapping);
        }

        public string getExecutionMapping()
        {
            return executionMapping;
        }

        public void setExecutionMapping(string mapping)
        {
            if (mapping != null && mapping.Equals(this.executionMapping)) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasExecutionMappingDefinition, mapping, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));
            executionMapping = (mapping is null || mapping.Equals("")) ? null : mapping;
            if (mapping != null && !mapping.Equals(""))
                addTriple(new IncompleteTriple(OWLTags.stdBelongsTo, mapping, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasExecutionMappingDefinition))
            {
                setExecutionMapping(objectContent);
                return true;
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

    }
}
