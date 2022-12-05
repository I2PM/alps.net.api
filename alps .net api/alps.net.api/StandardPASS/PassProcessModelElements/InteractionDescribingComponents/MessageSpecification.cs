using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that contains a certain message specification
    /// </summary>

    public class MessageSpecification : InteractionDescribingComponent, IMessageSpecification
    {
        protected IPayloadDescription payloadDescription;
        protected string message;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "MessageSpecification";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MessageSpecification();
        }

       protected MessageSpecification() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="payloadDescription"></param>
        /// <param name="additionalAttribute"></param>
        public MessageSpecification(IModelLayer layer, string labelForID = null, IPayloadDescription payloadDescription = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setContainedPayloadDescription(payloadDescription);
        }


        public void setContainedPayloadDescription(IPayloadDescription payloadDescription, int removeCascadeDepth = 0)
        {
            IPayloadDescription oldDescription = this.payloadDescription;
            // Might set it to null
            this.payloadDescription = payloadDescription;

            if (oldDescription != null)
            {
                if (oldDescription.Equals(payloadDescription)) return;
                oldDescription.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdContainsPayloadDescription, oldDescription.getUriModelComponentID()));
            }

            if (!(payloadDescription is null))
            {
                publishElementAdded(payloadDescription);
                payloadDescription.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdContainsPayloadDescription, payloadDescription.getUriModelComponentID()));
            }
        }


        public IPayloadDescription getContainedPayloadDescription()
        {
            return payloadDescription;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.containsPayloadDescription) && element is IPayloadDescription description)
                {
                    setContainedPayloadDescription(description);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getContainedPayloadDescription() != null) baseElements.Add(getContainedPayloadDescription());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IPayloadDescription payload && payload.Equals(getContainedPayloadDescription()))
                {
                    // Try to remove the incoming exchange
                    setContainedPayloadDescription(null, removeCascadeDepth);
                }
            }
        }
    }
}