
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class VisualTransitionDescription : VisualConnectionDescription, IVisualTransitionDescription
    {
        protected ITransition describedTransition;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "VisualTransitionDescription";

        public VisualTransitionDescription(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {

                if (predicate.Contains(OWLTags.visuallyDescribes) && element is ITransition exchange)
                {
                    setDescribedTransition(exchange);
                    return true;
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setDescribedTransition(ITransition transition, int removeCascadeDepth = 0)
        {
            ITransition oldTransition = this.describedTransition;
            // Might set it to null
            this.describedTransition = transition;

            if (oldTransition != null)
            {
                if (oldTransition.Equals(transition)) return;
                oldTransition.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.visuallyDescribes, oldTransition.getUriModelComponentID()));
            }

            if (!(transition is null))
            {
                publishElementAdded(transition);
                transition.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.visuallyDescribes, transition.getUriModelComponentID()));
            }
        }

        public ITransition getDescribedTransition()
        {
            return describedTransition;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new VisualTransitionDescription();
        }

        protected VisualTransitionDescription() { }
    }
}
