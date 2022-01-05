using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS.InteractionDescribingComponents;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS.ALPSModelElements.ALPSSIDComponents
{
    /// <summary>
    /// Method that represents an abstract message exchange class
    /// </summary>
    class AbstractMessageExchange : MessageExchange, IAbstractMessageExchange
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "AbstractMessageExchange";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new AbstractMessageExchange();
        }

       protected AbstractMessageExchange() { }
        /// <summary>
        /// Constructor that creates a fully specified empty instance of the abstract message exchange class
        /// </summary>
        /// <param name="additionalAttribute">list of additional attributes</param>
        /// <param name="modelComponentID">the id inside the model</param>
        /// <param name="modelComponentLabel">the label of the component</param>
        /// <param name="comment">the comment</param>
        /// <param name="messageSpecification">the type of message</param>
        /// <param name="receiver">the receiver of the message</param>
        /// <param name="sender">the sender of the message</param>
        public AbstractMessageExchange(IModelLayer layer, string labelForID = null, IMessageSpecification messageSpecification = null, ISubject senderSubject = null,
            ISubject receiverSubject = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, messageSpecification, senderSubject, receiverSubject, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
    }
}
