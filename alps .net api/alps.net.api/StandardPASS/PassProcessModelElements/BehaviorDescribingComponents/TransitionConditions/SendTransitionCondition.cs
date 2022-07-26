using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a send transition condition
    /// </summary>
    public class SendTransitionCondition : MessageExchangeCondition, ISendTransitionCondition
    {
        protected int lowerBound;
        protected int upperBound;
        protected ISendType sendType;
        protected ISubject messageSentTo;
        protected IMessageSpecification messageSpecification;

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "SendTransitionCondition";

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SendTransitionCondition();
        }

       protected SendTransitionCondition() { }
        public override string getClassName()
        {
            return className;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="toolSpecificDefintion"></param>
        /// <param name="messageExchange"></param>
        /// <param name="upperBound"></param>
        /// <param name="lowerBound"></param>
        /// <param name="sendType"></param>
        /// <param name="requiredMessageSendToSubject"></param>
        /// <param name="requiresSendingOfMessage"></param>
        /// <param name="additionalAttribute"></param>
        public SendTransitionCondition(ITransition transition, string labelForID = null,  string toolSpecificDefintion = null,
            IMessageExchange messageExchange = null, int upperBound = 0, int lowerBound = 0, ISendType sendType = null,
            ISubject messageSentFromSubject = null, IMessageSpecification receptionOfMessage = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(transition, labelForID,  toolSpecificDefintion, messageExchange, comment, additionalLabel, additionalAttribute)
        {
            setMultipleSendLowerBound(lowerBound);
            setMultipleSendUpperBound(upperBound);
            setSendType(sendType);
            setRequiresMessageSentTo(messageSentFromSubject);
            setRequiresSendingOfMessage(receptionOfMessage);
        }


        public void setMultipleSendLowerBound(int lowerBound)
        {
            if (lowerBound == this.lowerBound) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasMultiSendLowerBound, this.lowerBound.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));
            this.lowerBound = (lowerBound > 0) ? lowerBound : 1;
            addTriple(new IncompleteTriple(OWLTags.stdHasMultiSendLowerBound, lowerBound.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));

        }


        public void setMultipleSendUpperBound(int upperBound)
        {
            if (upperBound == this.upperBound) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasMultiSendUpperBound, this.upperBound.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));
            this.upperBound = (upperBound > 0) ? upperBound : 1;
            addTriple(new IncompleteTriple(OWLTags.stdHasMultiSendUpperBound, upperBound.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));
        }


        public void setSendType(ISendType sendType, int removeCascadeDepth = 0)
        {
            ISendType oldType = this.sendType;
            // Might set it to null
            this.sendType = sendType;

            if (oldType != null) {
                if (oldType.Equals(sendType)) return;
                oldType.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdHasSendType, oldType.getUriModelComponentID()));
            }
            
            if (!(sendType is null))
            {
                publishElementAdded(sendType);
                sendType.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasSendType, sendType.getUriModelComponentID()));
            }
        }


        public void setRequiresMessageSentTo(ISubject subject, int removeCascadeDepth = 0)
        {
            ISubject oldSubj = this.messageSentTo;
            // Might set it to null
            this.messageSentTo = subject;

            if (oldSubj != null)
            {
                if (oldSubj.Equals(subject)) return;
                oldSubj.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdRequiresMessageSentTo, oldSubj.getUriModelComponentID()));
            }
            
            if (!(subject is null))
            {
                publishElementAdded(subject);
                subject.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdRequiresMessageSentTo, subject.getUriModelComponentID()));
            }
        }


        public void setRequiresSendingOfMessage(IMessageSpecification messageSpecification, int removeCascadeDepth = 0)
        {
            IMessageSpecification oldSpec = this.messageSpecification;
            // Might set it to null
            this.messageSpecification = messageSpecification;

            if (oldSpec != null)
            {
                if (oldSpec.Equals(messageSpecification)) return;
                oldSpec.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdRequiresSendingOfMessage, oldSpec.getUriModelComponentID()));
            }
            
            if (!(messageSpecification is null))
            {
                publishElementAdded(messageSpecification);
                messageSpecification.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdRequiresSendingOfMessage, messageSpecification.getUriModelComponentID()));
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


        public ISendType getSendType()
        {
            return sendType;
        }


        public ISubject getRequiresMessageSentTo()
        {
            return messageSentTo;
        }


        public IMessageSpecification getRequiresSendingOfMessage()
        {
            return messageSpecification;
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getSendType() != null)
                baseElements.Add(getSendType());
            if (getRequiresMessageSentTo() != null)
                baseElements.Add(getRequiresMessageSentTo());
            if (getRequiresSendingOfMessage() != null)
                baseElements.Add(getRequiresSendingOfMessage());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is ISendType type && type.Equals(getSendType())) setSendType(null, removeCascadeDepth);
                if (update is ISubject subj && subj.Equals(getRequiresMessageSentTo())) setRequiresMessageSentTo(null, removeCascadeDepth);
                if (update is IMessageSpecification specification && specification.Equals(getRequiresSendingOfMessage())) setRequiresSendingOfMessage(null, removeCascadeDepth);
            }
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasMultiSendLowerBound))
            {
                string lower = objectContent;
                setMultipleSendLowerBound(int.Parse(lower));
                return true;
            }

            else if (predicate.Contains(OWLTags.hasMultiSendUpperBound))
            {
                string upper = objectContent;
                setMultipleSendUpperBound(int.Parse(upper));
                return true;
            }
            else if (element != null)
            {
                if (predicate.Contains(OWLTags.requiresMessageSentTo) && element is ISubject subject)
                {
                    setRequiresMessageSentTo(subject);
                    return true;
                }

                if (predicate.Contains(OWLTags.requiresSendingOfMessage) && element is IMessageSpecification messageSpecification)
                {
                    setRequiresSendingOfMessage(messageSpecification);
                    return true;
                }

                if (predicate.Contains(OWLTags.hasSendType) && element is ISendType sendType)
                {
                    setSendType(sendType);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }



    }
}