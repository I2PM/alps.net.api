using System.Collections.Generic;
using alps.net.api.util;
using alps.net.api.StandardPASS;
using alps.net.api.parsing;
using alps.net.api.src;

namespace alps.net.api.ALPS
{
    class FinalizedMessageExchange : MessageExchange, IFinalizedMessageExchange
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "FinalizedMessageExchange";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new FinalizedMessageExchange();
        }

       protected FinalizedMessageExchange() { }

        public FinalizedMessageExchange(IModelLayer layer, string label, IMessageSpecification messageSpecification = null, ISubject senderSubject = null,
            ISubject receiverSubject = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, label, messageSpecification, senderSubject, receiverSubject, comment, additionalLabel, additionalAttribute)
        {}

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
    }

    
}
