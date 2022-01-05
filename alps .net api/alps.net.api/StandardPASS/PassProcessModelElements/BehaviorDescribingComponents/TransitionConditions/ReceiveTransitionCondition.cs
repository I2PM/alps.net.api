using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS.InteractionDescribingComponents;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{

    /// <summary>
    /// Class that represents a receive transition condition
    /// </summary>
    public class ReceiveTransitionCondition : MessageExchangeCondition, IReceiveTransitionCondition
    {
        protected int lowerBound;
        protected int upperBound;
        protected IReceiveType receiveType;
        protected ISubject messageSentFromSubject;
        protected IMessageSpecification receptionOfMessage;

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "ReceiveTransitionCondition";
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ReceiveTransitionCondition();
        }

       protected ReceiveTransitionCondition() { }

        public override string getClassName()
        {
            return className;
        }

        public ReceiveTransitionCondition(ITransition transition, string labelForID = null,  string toolSpecificDefintion = null, IMessageExchange messageExchange = null,
            int upperBound = 0, int lowerBound = 0, IReceiveType receiveType = null, ISubject requiredMessageSendFromSubject = null,
            IMessageSpecification requiresReceptionOfMessage = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(transition, labelForID,  toolSpecificDefintion, messageExchange, comment, additionalLabel, additionalAttribute)
        {
            setMultipleReceiveLowerBound(lowerBound);
            setMultipleReceiveUpperBound(upperBound);
            setReceiveType(receiveType);
            setMessageSentFrom(requiredMessageSendFromSubject);
            setReceptionOfMessage(requiresReceptionOfMessage);

            // TODO specification, subject, exchange redundand
        }


        public void setMultipleReceiveLowerBound(int lowerBound)
        {
            if (this.lowerBound == lowerBound) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasMultiReceiveLowerBound, this.lowerBound.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));
            this.lowerBound = (lowerBound > 0) ? lowerBound : 1;
            addTriple(new IncompleteTriple(OWLTags.stdHasMultiReceiveLowerBound, lowerBound.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));
        }


        public void setMultipleReceiveUpperBound(int upperBound)
        {
            if (this.upperBound == upperBound) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasMultiReceiveUpperBound, this.upperBound.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));
            this.upperBound = (upperBound > 0) ? upperBound : 1;
            addTriple(new IncompleteTriple(OWLTags.stdHasMultiReceiveUpperBound, upperBound.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));
        }


        public void setReceiveType(IReceiveType receiveType, int removeCascadeDepth = 0)
        {
            IReceiveType oldType = this.receiveType;
            // Might set it to null
            this.receiveType = receiveType;

            if (oldType != null)
            {
                if (oldType.Equals(receiveType)) return;
                oldType.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdHasReceiveType, oldType.getUriModelComponentID()));
            }

            if (!(receiveType is null))
            {
                publishElementAdded(receiveType);
                receiveType.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasReceiveType, receiveType.getUriModelComponentID()));
            }
        }


        public void setMessageSentFrom(ISubject subject, int removeCascadeDepth = 0)
        {
            ISubject oldSubj = this.messageSentFromSubject;
            // Might set it to null
            this.messageSentFromSubject = subject;

            if (oldSubj != null)
            {
                if (oldSubj.Equals(subject)) return;
                oldSubj.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdRequiresMessageSentFrom, oldSubj.getUriModelComponentID()));
            }

            if (!(subject is null))
            {
                publishElementAdded(subject);
                subject.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdRequiresMessageSentFrom, subject.getUriModelComponentID()));
            }
        }


        public void setReceptionOfMessage(IMessageSpecification messageSpecification, int removeCascadeDepth = 0)
        {
            IMessageSpecification oldSpec = this.receptionOfMessage;
            // Might set it to null
            this.receptionOfMessage = messageSpecification;

            if (oldSpec != null)
            {
                if (oldSpec.Equals(messageSpecification)) return;
                oldSpec.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdRequiresReceptionOfMessage, oldSpec.getUriModelComponentID()));
            }

            if (!(messageSpecification is null))
            {
                publishElementAdded(messageSpecification);
                messageSpecification.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdRequiresReceptionOfMessage, messageSpecification.getUriModelComponentID()));
            }
        }


        public int getMultipleLowerBound()
        {
            return lowerBound;
        }


        public int getMultipleUpperBound()
        {
            return upperBound;
        }


        public IReceiveType getReceiveType()
        {
            return receiveType;
        }


        public ISubject getMessageSentFrom()
        {
            return messageSentFromSubject;
        }


        public IMessageSpecification getReceptionOfMessage()
        {
            return receptionOfMessage;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasMultiReceiveLowerBound))
            {
                string tmpLowerBound = objectContent;
                setMultipleReceiveLowerBound(int.Parse(tmpLowerBound));
                return true;
            }
            else if (predicate.Contains(OWLTags.hasMultiReceiveUpperBound))
            {
                string tmpUpperBound = objectContent;
                setMultipleReceiveUpperBound(int.Parse(tmpUpperBound));
                return true;
            }
            else if (element != null)
            {
                if (predicate.Contains(OWLTags.hasReceiveType) && element is IReceiveType receiveType)
                {
                    setReceiveType(receiveType);
                    return true;
                }

                if (predicate.Contains(OWLTags.requiresMessageSentFrom) && element is ISubject subject)
                {
                    setMessageSentFrom(subject);
                    return true;
                }
                if (predicate.Contains(OWLTags.requiresReceptionOfMessage) && element is IMessageSpecification messageSpecification)
                {
                    setReceptionOfMessage(messageSpecification);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getReceiveType() != null)
                baseElements.Add(getReceiveType());
            if (getMessageSentFrom() != null)
                baseElements.Add(getMessageSentFrom());
            if (getReceptionOfMessage() != null)
                baseElements.Add(getReceptionOfMessage());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IReceiveType type && type.Equals(getReceiveType())) setReceiveType(null, removeCascadeDepth);
                if (update is ISubject subj && subj.Equals(getMessageSentFrom())) setMessageSentFrom(null, removeCascadeDepth);
                if (update is IMessageSpecification specification && specification.Equals(getReceptionOfMessage())) setReceptionOfMessage(null, removeCascadeDepth);
            }
        }
    }
}