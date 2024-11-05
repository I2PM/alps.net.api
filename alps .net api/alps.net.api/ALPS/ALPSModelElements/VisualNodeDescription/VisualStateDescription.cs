
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class VisualStateDescription : VisualNodeDescription, IVisualStateDescription
    {
        protected IState describedState;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "VisualStateDescription";

        public VisualStateDescription(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {

                if (predicate.Contains(OWLTags.visuallyDescribes) && element is IState state)
                {
                    setDescribedState(state);
                    return true;
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setDescribedState(IState state, int removeCascadeDepth = 0)
        {
            IState oldState = this.describedState;
            // Might set it to null
            this.describedState = state;

            if (oldState != null)
            {
                if (oldState.Equals(state)) return;
                oldState.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.visuallyDescribes, oldState.getUriModelComponentID()));
            }

            if (!(state is null))
            {
                publishElementAdded(state);
                state.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.visuallyDescribes, state.getUriModelComponentID()));
            }
        }

        public IState getDescribedState()
        {
            return describedState;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new VisualStateDescription();
        }

        protected VisualStateDescription() { }
    }
}
