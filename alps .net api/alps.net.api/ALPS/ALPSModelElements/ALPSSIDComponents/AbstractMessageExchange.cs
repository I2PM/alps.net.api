using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// From abstract pass ont: <br></br>
    /// An abstract message exchange is defined on an abstract layer.
    /// It defines a possible message exchange between two subjects. (a recommendation for a message)
    /// On an implementing layer though it is allowed to unite the two subjects and thus ignore this communication. (e.g.because in the real process a human being is doing both tasks unitedly
    /// In contrast a standard MessageExchange would defined that two subjects (abstract or not) are not to be united.
    /// </summary>
    public class AbstractMessageExchange : MessageExchange, IAbstractMessageExchange
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string CLASS_NAME = "AbstractMessageExchange";

        public override string getClassName()
        {
            return CLASS_NAME;
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
       /// <param name="modelComponentId">the id inside the model</param>
       /// <param name="modelComponentLabel">the label of the component</param>
       /// <param name="comment">the comment</param>
       /// <param name="messageSpecification">the type of message</param>
       /// <param name="receiver">the receiver of the message</param>
       /// <param name="sender">the sender of the message</param>
       /// <param name="layer"></param>
       public AbstractMessageExchange(IModelLayer layer, string labelForId = null, IMessageSpecification messageSpecification = null, ISubject senderSubject = null,
            ISubject receiverSubject = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForId, messageSpecification, senderSubject, receiverSubject, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
    }
}
