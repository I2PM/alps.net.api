
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class VisualSubjectDescription : VisualNodeDescription, IVisualSubjectDescription
    {
        protected ISubject describedSubject;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "VisualSubjectDescription";

        public VisualSubjectDescription(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {

                if (predicate.Contains(OWLTags.visuallyDescribes) && element is ISubject subj)
                {
                    setDescribedState(subj);
                    return true;
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setDescribedState(ISubject subject, int removeCascadeDepth = 0)
        {
            ISubject oldSubject = this.describedSubject;
            // Might set it to null
            this.describedSubject = subject;

            if (oldSubject != null)
            {
                if (oldSubject.Equals(subject)) return;
                oldSubject.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.visuallyDescribes, oldSubject.getUriModelComponentID()));
            }

            if (!(subject is null))
            {
                publishElementAdded(subject);
                subject.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.visuallyDescribes, subject.getUriModelComponentID()));
            }
        }

        public ISubject getDescribedSubject()
        {
            return describedSubject;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new VisualSubjectDescription();
        }

        protected VisualSubjectDescription() { }
    }
}
