using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an interface subject
    /// </summary>

    public class InterfaceSubject : Subject, IInterfaceSubject
    {
        protected IFullySpecifiedSubject referencedSubject;

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "InterfaceSubject";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new InterfaceSubject();
        }

       protected InterfaceSubject() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalAttribute"></param>
        /// <param name="fullySpecifiedSubject"></param>
        public InterfaceSubject(IModelLayer layer, string labelForID = null,  ISet<IMessageExchange> incomingMessageExchange = null,
            ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1, IFullySpecifiedSubject fullySpecifiedSubject = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID,  incomingMessageExchange, outgoingMessageExchange, maxSubjectInstanceRestriction, comment, additionalLabel, additionalAttribute)
        {
            setReferencedSubject(fullySpecifiedSubject);
        }


        public void setReferencedSubject(IFullySpecifiedSubject fullySpecifiedSubject, int removeCascadeDepth = 0)
        {
            IFullySpecifiedSubject oldSubject = this.referencedSubject;
            // Might set it to null
            this.referencedSubject = fullySpecifiedSubject;

            if (oldSubject != null)
            {
                if (oldSubject.Equals(referencedSubject)) return;
                oldSubject.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdReferences, oldSubject.getUriModelComponentID()));
            }

            // Might set it to null
            this.referencedSubject = fullySpecifiedSubject;
            if (!(fullySpecifiedSubject is null))
            {
                publishElementAdded(fullySpecifiedSubject);
                fullySpecifiedSubject.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdReferences, fullySpecifiedSubject.getUriModelComponentID()));
            }
        }


        public IFullySpecifiedSubject getReferencedSubject()
        {
            return referencedSubject;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.references) && element is IFullySpecifiedSubject subject)
                {
                    setReferencedSubject(subject);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getReferencedSubject() != null) baseElements.Add(getReferencedSubject());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is ISubject subj && subj.Equals(getReferencedSubject()))
                    setReferencedSubject(null, removeCascadeDepth);
            }
        }

    }
}