using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an macro state
    /// </summary>

    public class MacroState : State, IMacroState
    {
        protected IMacroBehavior referenceMacroBehavior;
        protected readonly ICompatibilityDictionary<string, IStateReference> stateReferences = new CompatibilityDictionary<string, IStateReference>();
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "Macrostate";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MacroState();
        }

       protected MacroState() { }

        public MacroState(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null, ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            ISet<IStateReference> stateReferences = null, IMacroBehavior macroBehavior = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, labelForID, guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        {
            setReferencedMacroBehavior(macroBehavior);
            setStateReferences(stateReferences);
        }


        public void setReferencedMacroBehavior(IMacroBehavior macroBehavior, int removeCascadeDepth = 0)
        {
            IMacroBehavior oldBehavior = this.referenceMacroBehavior;
            // Might set it to null
            this.referenceMacroBehavior = macroBehavior;

            if (oldBehavior != null)
            {
                if (oldBehavior.Equals(macroBehavior)) return;
                oldBehavior.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdReferencesMacroBehavior, oldBehavior.getUriModelComponentID()));
            }

            if (!(macroBehavior is null))
            {
                publishElementAdded(macroBehavior);
                macroBehavior.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdReferencesMacroBehavior, macroBehavior.getUriModelComponentID()));
            }
        }


        public IMacroBehavior getReferencedMacroBehavior()
        {
            return referenceMacroBehavior;
        }


        public void addStateReference(IStateReference stateReference)
        {
            if (stateReference is null) { return; }
            if (stateReferences.TryAdd(stateReference.getModelComponentID(), stateReference))
            {
                publishElementAdded(stateReference);
                stateReference.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdContains, stateReference.getUriModelComponentID()));
            }
        }

        public void removeStateReference(string stateRefID, int removeCascadeDepth = 0)
        {
            if (stateRefID is null) return;
            if (stateReferences.TryGetValue(stateRefID, out IStateReference reference))
            {
                stateReferences.Remove(stateRefID);
                reference.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdContains, reference.getUriModelComponentID()));
            }
        }

        public void setStateReferences(ISet<IStateReference> references, int removeCascadeDepth = 0)
        {
            foreach (IStateReference reference in getStateReferences().Values)
            {
                removeStateReference(reference.getModelComponentID(), removeCascadeDepth);
            }
            if (references is null) return;
            foreach (IStateReference reference in references)
            {
                addStateReference(reference);
            }
        }

        public IDictionary<string, IStateReference> getStateReferences()
        {
            return new Dictionary<string, IStateReference>(stateReferences);
        }



        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.referencesMacroBehavior) && element is IMacroBehavior behavior)
                {
                    setReferencedMacroBehavior(behavior);
                    return true;
                }
                if (predicate.Contains(OWLTags.contains) && element is IStateReference reference)
                {
                    addStateReference(reference);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IStateReference component in getStateReferences().Values)
                baseElements.Add(component);
            if (getReferencedMacroBehavior() != null)
                baseElements.Add(getReferencedMacroBehavior());
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IStateReference reference)
                    removeStateReference(reference.getModelComponentID(), removeCascadeDepth);
                if (update is IMacroBehavior behavior && behavior.Equals(getReferencedMacroBehavior()))
                    setReferencedMacroBehavior(null, removeCascadeDepth);
            }
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (stateReferences.ContainsKey(oldID))
            {
                IStateReference element = stateReferences[oldID];
                stateReferences.Remove(oldID);
                stateReferences.Add(element.getModelComponentID(), element);
            }

            base.notifyModelComponentIDChanged(oldID, newID);
        }
    }
}

