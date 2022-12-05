using System.Collections.Generic;
using alps.net.api.util;
using alps.net.api.StandardPASS;
using alps.net.api.parsing;
using alps.net.api.src;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// From abstract pass ont:<br></br>
    /// Exclusive Message Exchange is an abstract concept to be used on abstract layers
    /// A finalized message exchange defines that a subject is not allowed to comunicate with the corresponding subject in any other way than this message exchange or similiar messages in the same model in any other way.
    /// If an finalized message connection is used on a subject no other normal or abstract Message Exchange is allowed(while Communication Restrictions are not necessary).
    /// </summary>
    public class FinalizedMessageExchange : MessageExchange, IFinalizedMessageExchange
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string CLASS_NAME = "FinalizedMessageExchange";

        public override string getClassName()
        {
            return CLASS_NAME;
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
