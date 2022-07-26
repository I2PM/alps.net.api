using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class CommunicationRestriction : ALPSSIDComponent, ICommunicationRestriction
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "CommunicationRestriction";
        private ISubject correspondentA;
        private ISubject correspondentB;

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new CommunicationRestriction();
        }

        protected CommunicationRestriction() { }

        public CommunicationRestriction(IModelLayer layer, string labelForID = null, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
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
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setCorrespondents(ISubject correspondentA, ISubject correspondentB, int removeCascadeDepth = 0)
        {
            setCorrespondentA(correspondentA, removeCascadeDepth);
            setCorrespondentB(correspondentB, removeCascadeDepth);
        }

        public void setCorrespondentA(ISubject correspondentA, int removeCascadeDepth = 0)
        {
            ISubject oldCorrespondentA = this.correspondentA;
            // Might set it to null
            this.correspondentA = correspondentA;

            if (oldCorrespondentA != null)
            {
                if (oldCorrespondentA.Equals(correspondentA)) return;
                oldCorrespondentA.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdHasCorrespondent, correspondentA.getUriModelComponentID()));
            }
            if (!(correspondentA is null))
            {
                publishElementAdded(correspondentA);
                correspondentA.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasCorrespondent, correspondentA.getUriModelComponentID()));
            }
        }

        public void setCorrespondentB(ISubject correspondentB, int removeCascadeDepth = 0)
        {
            ISubject oldCorrespondentB = this.correspondentB;
            // Might set it to null
            this.correspondentB = correspondentB;

            if (oldCorrespondentB != null)
            {
                if (oldCorrespondentB.Equals(correspondentB)) return;
                oldCorrespondentB.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdHasCorrespondent, correspondentB.getUriModelComponentID()));
            }
            if (!(correspondentB is null))
            {
                publishElementAdded(correspondentB);
                correspondentB.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasCorrespondent, correspondentB.getUriModelComponentID()));
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
    }
}
