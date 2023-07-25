using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// Method that represents an abstract communication channel.
    /// Instances of this class are by default BiDirectional channels, but can be changed to UniDirectional channels using <see cref="setIsUniDirectional(bool)"/>
    /// <br></br><br></br>
    /// From abstract pass ont:<br></br>
    /// It defines a possible message exchange between two subjects. (a recommendation for a message). It states that there IS communication between the two but not what exactly.
    /// Usually, comunication channels are bi-directional, however the can be limited to one direction.
    ///Similar to the abstract message connector, if two abstract or interface subjects are connected via a channel they mey be joined on an implementation layer.
    /// </summary>
    public class CommunicationChannel : ALPSSIDComponent, ICommunicationChannel
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "CommunicationChannel";
        protected ISubject correspondentA, correspondentB;
        protected bool channelIsUniDirectional = false;

        // Used for internal methods
        private bool oldIsUniDirectionalValue = false;

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new CommunicationChannel();
        }

        protected CommunicationChannel() { }

        public CommunicationChannel(IModelLayer layer, string labelForID = null,
            ISubject correspondentA = null, ISubject correspondentB = null,
            bool isUniDirectional = false, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setIsUniDirectional(isUniDirectional); 
            setCorrespondents(correspondentA, correspondentB);
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

        public void setCorrespondents(ISubject correspondentA, ISubject correspondentB, int removeCascadeDepth = 0)
        {
            setCorrespondentA(correspondentA, removeCascadeDepth);
            setCorrespondentB(correspondentB, removeCascadeDepth);
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null && element is ISubject subj)
            {
                if (predicate.Contains(OWLTags.hasCorrespondent))
                {
                    if (correspondentA == null)
                    {
                        setCorrespondentA(subj);
                    }
                    else if (correspondentB == null)
                    {
                        setCorrespondentB(subj);
                    }
                }
                else if (predicate.Contains(OWLTags.hasSender))
                {
                    setCorrespondentA(subj);
                }
                else if (predicate.Contains(OWLTags.hasReceiver))
                {
                    setCorrespondentB(subj);
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public void setCorrespondentA(ISubject correspondentA, int removeCascadeDepth = 0)
        {
            // Check if the channel changed from Uni to BiDirectional or vice versa
            bool directionalChange = oldIsUniDirectionalValue != channelIsUniDirectional;
            ISubject oldCorrespondentA = this.correspondentA;
            // Might set it to null
            this.correspondentA = correspondentA;

            if (oldCorrespondentA != null)
            {
                // Return only if both are same and the channel direction did not change
                // If the direction changed, the triples must still be replaced
                if (oldCorrespondentA.Equals(correspondentA) && !directionalChange) return;

                // Only if the correspondet is new, it must be unregistered
                if (!oldCorrespondentA.Equals(correspondentA))
                    oldCorrespondentA.unregister(this, removeCascadeDepth);

                // Delete the existing triple depending on the old directional state of the channel
                removeTriple(new IncompleteTriple(oldIsUniDirectionalValue ? OWLTags.stdHasSender : OWLTags.stdHasCorrespondent, correspondentA.getUriModelComponentID()));

            }
            if (!(correspondentA is null))
            {
                // Only if the correspondet is new, it must be registered
                if ((oldCorrespondentA is null) ||(!oldCorrespondentA.Equals(correspondentA)))
                {
                    publishElementAdded(correspondentA);
                    correspondentA.register(this);
                }
                // Add the new triple depending on the new directional state of the channel
                addTriple(new IncompleteTriple(channelIsUniDirectional ? OWLTags.stdHasSender : OWLTags.stdHasCorrespondent, correspondentA.getUriModelComponentID()));
            }
        }

        public void setCorrespondentB(ISubject correspondentB, int removeCascadeDepth = 0)
        {
            // Check if the channel changed from Uni to BiDirectional or vice versa
            bool directionalChange = oldIsUniDirectionalValue != channelIsUniDirectional;
            ISubject oldCorrespondentB = this.correspondentB;
            // Might set it to null
            this.correspondentB = correspondentB;

            if (oldCorrespondentB != null)
            {
                // Return only if both are same and the channel direction did not change
                // If the direction changed, the triples must still be replaced
                if (oldCorrespondentB.Equals(correspondentB) && !directionalChange) return;

                // Only if the correspondet is new, it must be unregistered
                if (!oldCorrespondentB.Equals(correspondentB))
                    oldCorrespondentB.unregister(this, removeCascadeDepth);

                // Delete the existing triple depending on the old directional state of the channel
                removeTriple(new IncompleteTriple(oldIsUniDirectionalValue ? OWLTags.stdHasReceiver : OWLTags.stdHasCorrespondent, correspondentB.getUriModelComponentID()));
            }
            if (!(correspondentB is null))
            {
                // Only if the correspondet is new, it must be registered
                if ((oldCorrespondentB is null) || !(oldCorrespondentB.Equals(correspondentB)))
                {
                    publishElementAdded(correspondentB);
                    correspondentB.register(this);
                }
                addTriple(new IncompleteTriple(channelIsUniDirectional ? OWLTags.stdHasReceiver : OWLTags.stdHasCorrespondent, correspondentB.getUriModelComponentID()));
            }
        }

        public ISubject getCorrespondentA()
        {
            return correspondentA;
        }

        public ISubject getCorrespondentB()
        {
            return correspondentB;
        }

        public Tuple<ISubject, ISubject> getCorrespondents()
        {
            return new Tuple<ISubject, ISubject>(correspondentA, correspondentB);
        }

        public void setIsUniDirectional(bool isUniDirectional)
        {
            if (channelIsUniDirectional == isUniDirectional) return;
            oldIsUniDirectionalValue = channelIsUniDirectional;
            channelIsUniDirectional = isUniDirectional;
            setCorrespondents(correspondentA, correspondentB);
            oldIsUniDirectionalValue = channelIsUniDirectional;
        }

        public bool isUniDirectional()
        {
            return channelIsUniDirectional;
        }
    }
}
