using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class ExtensionBehavior : SubjectBehavior, IExtensionBehavior
    {

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "ExtensionBehavior";
        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ExtensionBehavior();
        }

       protected ExtensionBehavior() { }

        public ExtensionBehavior(IModelLayer layer, string labelForID = null, ISubject subject = null, ISet<IBehaviorDescribingComponent> behaviorDescribingComponents = null,
            IState initialStateOfBehavior = null, int priorityNumber = 0, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, subject, behaviorDescribingComponents, initialStateOfBehavior, priorityNumber, comment, additionalLabel, additionalAttribute)
        {
        }

        public override void setSubject(ISubject subj, int removeCascadeDepth = 0)
        {
            if (subj is ISubjectExtension extension)
            {
                ISubject oldSubj = this.subj;

                // Might set it to null
                this.subj = subj;

                if (oldSubj != null)
                {
                    if (oldSubj.Equals(subj)) return;
                    if (oldSubj is IParseablePASSProcessModelElement parseable)
                        removeTriple(new IncompleteTriple(OWLTags.stdHasInitialStateOfBehavior, parseable.getUriModelComponentID()));
                    if (oldSubj is ISubjectExtension oldExtension)
                    {
                        oldExtension.removeExtensionBehavior(getModelComponentID());
                    }
                }

                if (!(extension is null))
                {
                    if (extension is IParseablePASSProcessModelElement parseable)
                        addTriple(new IncompleteTriple(OWLTags.stdHasInitialStateOfBehavior, parseable.getUriModelComponentID()));
                    extension.addExtensionBehavior(this);
                }
            }
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

    }
}
