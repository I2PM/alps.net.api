using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class GroupState : State, IGroupState
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "GroupState";
        protected ICompatibilityDictionary<string, IBehaviorDescribingComponent> groupedComponents = new CompatibilityDictionary<string, IBehaviorDescribingComponent>();

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new GroupState();
        }

       protected GroupState() { }
        /// <summary>
        /// Constructor that creates a new fully specified instance of the group state class
        /// </summary>
        public GroupState(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null, ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, labelForID, guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }


        public virtual bool addGroupedComponent(IBehaviorDescribingComponent component)
        {
            if (component is null) { return false; }
            if (groupedComponents.TryAdd(component.getModelComponentID(), component))
            {
                publishElementAdded(component);
                component.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdContains, component.getUriModelComponentID()));
                return true;
            }
            return false;
        }


        public void setGroupedComponents(ISet<IBehaviorDescribingComponent> components, int removeCascadeDepth = 0)
        {
            foreach (IBehaviorDescribingComponent component in this.getGroupedComponents().Values)
            {
                removeGroupedComponent(component.getModelComponentID(), removeCascadeDepth);
            }
            if (components is null) return;
            foreach (IBehaviorDescribingComponent component in components)
            {
                addGroupedComponent(component);
            }
        }

        public virtual bool removeGroupedComponent(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return false;
            if (groupedComponents.TryGetValue(id, out IBehaviorDescribingComponent component))
            {
                groupedComponents.Remove(id);
                component.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdContains, component.getUriModelComponentID()));
                return true;
            }
            return false;
        }
        public IDictionary<string, IBehaviorDescribingComponent> getGroupedComponents()
        {
            return new Dictionary<string, IBehaviorDescribingComponent>(groupedComponents);
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.contains) && element is IBehaviorDescribingComponent component)
                {
                    addGroupedComponent(component);
                    return true;

                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }
    }
}
