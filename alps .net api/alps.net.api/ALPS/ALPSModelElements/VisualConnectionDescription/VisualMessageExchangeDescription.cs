
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class VisualMessageExchangeDescription : VisualConnectionDescription, IVisualMessageExchangeDescription
    {
        protected IMessageExchange describedMessageExchange;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "VisualMessageExchangeDescription";

        public VisualMessageExchangeDescription(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {

                if (predicate.Contains(OWLTags.visuallyDescribes) && element is IMessageExchange exchange)
                {
                    setDescribedExchange(exchange);
                    return true;
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setDescribedExchange(IMessageExchange exchange, int removeCascadeDepth = 0)
        {
            IMessageExchange oldExchange = this.describedMessageExchange;
            // Might set it to null
            this.describedMessageExchange = exchange;

            if (oldExchange != null)
            {
                if (oldExchange.Equals(exchange)) return;
                oldExchange.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.visuallyDescribes, oldExchange.getUriModelComponentID()));
            }

            if (!(exchange is null))
            {
                publishElementAdded(exchange);
                exchange.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.visuallyDescribes, exchange.getUriModelComponentID()));
            }
        }

        public IMessageExchange getDescribedExchange()
        {
            return describedMessageExchange;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new VisualMessageExchangeDescription();
        }

        protected VisualMessageExchangeDescription() { }
    }
}
