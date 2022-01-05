using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;
using VDS.RDF;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a time transition condition
    /// </summary>
    public class TimeTransitionCondition : TransitionCondition, ITimerTransitionCondition
    {
        protected string timeValue = "";
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "TimeTransitionCondition";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new TimeTransitionCondition();
        }

       protected TimeTransitionCondition() { }
        public TimeTransitionCondition(ITransition transition, string labelForID = null, string toolSpecificDefintion = null, string timeValue = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(transition, labelForID, toolSpecificDefintion, comment, additionalLabel, additionalAttribute)
        {
            setTimeValue(timeValue);
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasTimeValue))
            {
                setTimeValue(objectContent);
                return true;
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public void setTimeValue(string timeValue)
        {
            if (timeValue != null && timeValue.Equals(this.timeValue)) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasTimeValue, this.timeValue, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));
            this.timeValue = (timeValue is null || timeValue.Equals("")) ? null : timeValue;
            if (toolSpecificDefinition != null)
            {
                addTriple(new IncompleteTriple(OWLTags.stdHasTimeValue, timeValue, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));
            }

        }


        public string getTimeValue()
        {
            return timeValue;
        }

    }
}