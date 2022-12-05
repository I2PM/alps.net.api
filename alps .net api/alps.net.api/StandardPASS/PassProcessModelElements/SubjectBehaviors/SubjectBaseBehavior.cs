using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;
using System.Linq;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a subject base behavior class
    /// According to standard pass 1.1.0
    /// </summary>
    public class SubjectBaseBehavior : SubjectBehavior, ISubjectBaseBehavior
    {
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "SubjectBaseBehavior";

        protected IDictionary<string, IState> endStates = new Dictionary<string, IState>();
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new SubjectBaseBehavior();
        }

        protected SubjectBaseBehavior() { }

        public SubjectBaseBehavior(IModelLayer layer, string labelForID = null, ISubject subject = null, ISet<IBehaviorDescribingComponent> components = null,
            ISet<IState> endStates = null, IState initialStateOfBehavior = null, int priorityNumber = 0, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, subject, components, initialStateOfBehavior, priorityNumber, comment, additionalLabel, additionalAttribute)
        {
            if (endStates != null)
                foreach (IState state in endStates)
                {
                    addBehaviorDescribingComponent(state);
                    state.setIsStateType(IState.StateType.EndState);
                }
        }

        public override string getClassName()
        {
            return className;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasEndState) && element is IState endState)
                {
                    addBehaviorDescribingComponent(endState);
                    endState.setIsStateType(IState.StateType.EndState);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public IDictionary<string, IState> getEndStates()
        {
            IDictionary<string, IState> endStates = new Dictionary<string, IState>();
            foreach (IState state in getBehaviorDescribingComponents().Values.OfType<IState>())
            {
                if (state.isStateType(IState.StateType.EndState))
                    endStates.Add(state.getModelComponentID(), state);
            }
            return new Dictionary<string, IState>(endStates);
        }


        public override bool addBehaviorDescribingComponent(IBehaviorDescribingComponent component)
        {
            // As described in standard, no state references might be contained
            if (component is IStateReference) return false;
            return base.addBehaviorDescribingComponent(component);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IState endState in getEndStates().Values) baseElements.Add(endState);
            return baseElements;
        }

        public void setEndStates(ISet<IState> endStates, int removeCascadeDepth = 0)
        {
            foreach (IState state in endStates)
            {
                registerEndState(state);
            }
        }

        public void registerEndState(IState state)
        {
            addBehaviorDescribingComponent(state);
            state.setIsStateType(IState.StateType.EndState);
            addTriple(new IncompleteTriple(OWLTags.stdHasEndState, state.getUriModelComponentID()));
        }

        public void unregisterEndState(string id, int removeCascadeDepth = 0)
        {
            getBehaviorDescribingComponents().TryGetValue(id, out IBehaviorDescribingComponent component);
            if (component is IState state)
            {
                state.removeStateType(IState.StateType.EndState);
                removeTriple(new IncompleteTriple(OWLTags.stdHasEndState, getUriModelComponentID()));
            }
            
        }
    }
}
