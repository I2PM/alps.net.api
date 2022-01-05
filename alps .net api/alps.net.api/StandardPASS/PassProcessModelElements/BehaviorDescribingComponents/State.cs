using alps.net.api.util;
using System.Collections.Generic;
using alps.net.api.StandardPASS.SubjectBehaviors;
using alps.net.api.parsing;
using alps.net.api.src;
using static alps.net.api.StandardPASS.BehaviorDescribingComponents.IState;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a state
    /// </summary>
    public class State : BehaviorDescribingComponent, IStateReference
    {
        protected readonly IDictionary<string, ITransition> incomingTransitions = new Dictionary<string, ITransition>();
        protected readonly IDictionary<string, ITransition> outgoingTransitions = new Dictionary<string, ITransition>();
        protected readonly IDictionary<string, IState> implementedInterfaces = new Dictionary<string, IState>();
        protected readonly ISet<StateType> stateTypes = new HashSet<StateType>();
        protected IFunctionSpecification functionSpecification;
        protected IGuardBehavior guardBehavior;
        protected IAction action;
        protected IState referenceState;

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "State";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new State();
        }

        protected State() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="action"></param>
        /// <param name="guardBehavior"></param>
        /// <param name="functionSpecification"></param>
        /// <param name="incomingTransition"></param>
        /// <param name="outgoingTransition"></param>
        /// <param name="additionalAttribute"></param>
        public State(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null, ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null) : base(behavior, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setGuardBehavior(guardBehavior);
            setFunctionSpecification(functionSpecification);
            setIncomingTransitions(incomingTransition);
            setOutgoingTransitions(outgoingTransition);
            generateAction();
        }


        public virtual void addIncomingTransition(ITransition transition)
        {
            if (transition is null) { return; }
            if (incomingTransitions.TryAdd(transition.getModelComponentID(), transition))
            {
                transition.setTargetState(this);
                publishElementAdded(transition);
                transition.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasIncomingTransition, transition.getUriModelComponentID()));
            }
        }

        /// <summary>
        /// Method that returns the incoming transition attribute of the instance
        /// </summary>
        /// <returns>The incoming transition attribute of the instance</returns>
        public IDictionary<string, ITransition> getIncomingTransitions()
        {
            return new Dictionary<string, ITransition>(incomingTransitions);
        }


        public virtual void addOutgoingTransition(ITransition transition)
        {
            if (transition is null) { return; }
            if (outgoingTransitions.TryAdd(transition.getModelComponentID(), transition))
            {
                transition.setSourceState(this);
                publishElementAdded(transition);
                transition.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasOutgoingTransition, transition.getUriModelComponentID()));
            }
        }

        public void setOutgoingTransitions(ISet<ITransition> transitions, int removeCascadeDepth = 0)
        {
            foreach (ITransition transition in getOutgoingTransitions().Values)
            {
                removeOutgoingTransition(transition.getModelComponentID());
            }
            if (transitions is null) return;
            foreach (ITransition transition in transitions)
            {
                addOutgoingTransition(transition);
            }
        }

        public void setIncomingTransitions(ISet<ITransition> transitions, int removeCascadeDepth = 0)
        {
            foreach (ITransition transition in getIncomingTransitions().Values)
            {
                removeIncomingTransition(transition.getModelComponentID());
            }
            if (transitions is null) return;
            foreach (ITransition transition in transitions)
            {
                addIncomingTransition(transition);
            }
        }

        public void removeOutgoingTransition(string modelCompID, int removeCascadeDepth = 0)
        {
            if (modelCompID is null) return;
            if (outgoingTransitions.TryGetValue(modelCompID, out ITransition transition))
            {
                outgoingTransitions.Remove(modelCompID);
                transition.unregister(this);
                transition.setSourceState(null);
                action.updateRemoved(transition, this);
                removeTriple(new IncompleteTriple(OWLTags.stdHasOutgoingTransition, transition.getUriModelComponentID()));
            }
        }

        public void removeIncomingTransition(string modelCompID, int removeCascadeDepth = 0)
        {
            if (modelCompID is null) return;
            if (incomingTransitions.TryGetValue(modelCompID, out ITransition transition))
            {
                incomingTransitions.Remove(modelCompID);
                transition.unregister(this);
                transition.setTargetState(null);
                removeTriple(new IncompleteTriple(OWLTags.stdHasIncomingTransition, transition.getUriModelComponentID()));
            }
        }

        /// <summary>
        /// Method that returns the outgoing transition attribute of the instance
        /// </summary>
        /// <returns>The outgoing transition attribute of the instance</returns>
        public IDictionary<string, ITransition> getOutgoingTransitions()
        {
            return new Dictionary<string, ITransition>(outgoingTransitions);
        }


        public virtual void setFunctionSpecification(IFunctionSpecification functionSpecification, int removeCascadeDepth = 0)
        {
            IFunctionSpecification oldSepc = functionSpecification;
            // Might set it to null
            this.functionSpecification = functionSpecification;

            if (oldSepc != null)
            {
                if (oldSepc.Equals(functionSpecification)) return;
                oldSepc.unregister(this);
                removeTriple(new IncompleteTriple(OWLTags.stdHasFunctionSpecification, oldSepc.getUriModelComponentID()));
            }

            if (!(functionSpecification is null))
            {
                publishElementAdded(functionSpecification);
                functionSpecification.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasFunctionSpecification, functionSpecification.getUriModelComponentID()));
            }
        }


        public IFunctionSpecification getFunctionSpecification()
        {
            return functionSpecification;
        }


        public void setGuardBehavior(IGuardBehavior guardBehavior, int removeCascadeDepth = 0)
        {
            IGuardBehavior oldBehavior = guardBehavior;
            // Might set it to null
            this.guardBehavior = guardBehavior;

            if (oldBehavior != null)
            {
                if (oldBehavior.Equals(guardBehavior)) return;
                oldBehavior.unregister(this, removeCascadeDepth);
                oldBehavior.removeGuardedState(getModelComponentID());
                removeTriple(new IncompleteTriple(OWLTags.stdGuardedBy, oldBehavior.getUriModelComponentID()));
            }

            if (!(guardBehavior is null))
            {
                publishElementAdded(guardBehavior);
                guardBehavior.register(this);
                guardBehavior.addGuardedState(this);
                addTriple(new IncompleteTriple(OWLTags.stdGuardedBy, guardBehavior.getUriModelComponentID()));
            }
        }


        public IGuardBehavior getGuardBehavior()
        {
            return guardBehavior;
        }


        protected void generateAction(IAction newGeneratedAction = null)
        {
            IAction oldAction = action;
            // Might set it to null
            string label = (getModelComponentLabelsAsStrings().Count > 0) ? getModelComponentLabelsAsStrings()[0] : getModelComponentID();
            this.action = (newGeneratedAction is null) ? new Action(this, "ActionFor" + label) : newGeneratedAction;

            if (oldAction != null)
            {
                if (oldAction.Equals(action)) return;
                oldAction.unregister(this);
                oldAction.removeFromEverything();
                removeTriple(new IncompleteTriple(OWLTags.stdBelongsTo, oldAction.getUriModelComponentID()));
            }

            if (!(action is null))
            {
                publishElementAdded(action);
                action.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdBelongsTo, action.getUriModelComponentID()));
            }
        }


        public IAction getAction()
        {
            return action;
        }

        public bool isStateType(StateType stateType)
        {
            return stateTypes.Contains(stateType);
        }

        public virtual void setIsStateType(StateType stateType)
        {
            switch (stateType)
            {
                case StateType.InitialStateOfBehavior:
                    if (stateTypes.Add(stateType))
                        addTriple(new IncompleteTriple(OWLTags.rdfType, OWLTags.std + "InitialStateOfBehavior"));
                    break;
                case StateType.InitialStateOfChoiceSegmentPath:
                    if (stateTypes.Add(stateType))
                        addTriple(new IncompleteTriple(OWLTags.rdfType, OWLTags.std + "InitialStateOfChoiceSegmentPath"));
                    break;
                case StateType.EndState:
                    if (getContainedBy(out ISubjectBehavior behavior) && !(behavior is ISubjectBaseBehavior)) break;
                    if (stateTypes.Add(stateType))
                    {
                        addTriple(new IncompleteTriple(OWLTags.rdfType, OWLTags.std + "EndState"));
                        if (getContainedBy(out ISubjectBehavior behavior2) && behavior2 is IParseablePASSProcessModelElement parseable)
                        {
                            parseable.addTriple(new IncompleteTriple(OWLTags.stdHasEndState, getUriModelComponentID()));
                        }
                    }
                    break;
            }

        }

        public virtual void removeStateType(StateType stateType)
        {
            if (stateTypes.Remove(stateType))
            {
                switch (stateType)
                {
                    case StateType.InitialStateOfBehavior:
                        removeTriple(new IncompleteTriple(OWLTags.rdfType, OWLTags.std + "InitialStateOfBehavior"));
                        break;
                    case StateType.InitialStateOfChoiceSegmentPath:
                        removeTriple(new IncompleteTriple(OWLTags.rdfType, OWLTags.std + "InitialStateOfChoiceSegmentPath"));
                        break;
                    case StateType.EndState:
                        removeTriple(new IncompleteTriple(OWLTags.rdfType, OWLTags.std + "EndState"));
                        if (getContainedBy(out ISubjectBehavior behavior) && behavior is IParseablePASSProcessModelElement parseable)
                        {
                            parseable.removeTriple(new IncompleteTriple(OWLTags.stdHasEndState, getUriModelComponentID()));
                        }
                        break;
                }

            }
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (element is ITransition transition)
                {
                    if (predicate.Contains(OWLTags.hasIncomingTransition))
                    {
                        addIncomingTransition(transition);
                        return true;
                    }
                    else if (predicate.Contains(OWLTags.hasOutgoingTransition))
                    {
                        addOutgoingTransition(transition);
                        return true;
                    }

                }
                else if (predicate.Contains(OWLTags.guardedBy) && element is IGuardBehavior guard)
                {
                    setGuardBehavior(guard);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasFunctionSpecification) && element is IFunctionSpecification specification)
                {
                    setFunctionSpecification(specification);
                    return true;
                }

                else if (predicate.Contains(OWLTags.belongsTo) && element is IAction action)
                {
                    generateAction(action);
                    return true;
                }
                else if (predicate.Contains(OWLTags.type))
                {
                    if (objectContent.ToLower().Contains("endstate"))
                    {
                        setIsStateType(StateType.EndState);
                        return true;
                    }
                    else if (objectContent.ToLower().Contains("initialstateofbehavior"))
                    {
                        setIsStateType(StateType.InitialStateOfBehavior);
                        return true;
                    }
                    else if (objectContent.ToLower().Contains("initialstateofchoicesegmentpath"))
                    {
                        setIsStateType(StateType.InitialStateOfChoiceSegmentPath);
                        return true;
                    }
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (getAction() != null)
                baseElements.Add(getAction());
            if (getGuardBehavior() != null && specification == ConnectedElementsSetSpecification.ALL)
                baseElements.Add(getGuardBehavior());
            if (getFunctionSpecification() != null && (specification == ConnectedElementsSetSpecification.ALL || specification == ConnectedElementsSetSpecification.TO_ADD))
                baseElements.Add(getFunctionSpecification());
            if (specification == ConnectedElementsSetSpecification.ALL || specification == ConnectedElementsSetSpecification.TO_ADD ||
                specification == ConnectedElementsSetSpecification.TO_REMOVE_DIRECTLY_ADJACENT || specification == ConnectedElementsSetSpecification.TO_REMOVE_ADJACENT_AND_MORE)
            {
                foreach (ITransition transition in getOutgoingTransitions().Values)
                    baseElements.Add(transition);

                if (specification != ConnectedElementsSetSpecification.TO_REMOVE_DIRECTLY_ADJACENT)
                    foreach (ITransition transition in getIncomingTransitions().Values)
                        baseElements.Add(transition);
            }
            return baseElements;
        }


        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller);
            if (update is null) return;
            if (update.Equals(action))
            {
                // TODO what to do here?
            }
            else if (update.Equals(guardBehavior))
            {
                setGuardBehavior(null, removeCascadeDepth);
            }
            else if (update.Equals(functionSpecification))
            {
                setFunctionSpecification(null, removeCascadeDepth);
            }
            else
            {
                foreach (ITransition trans in incomingTransitions.Values)
                {
                    if (update.Equals(trans))
                    {
                        removeIncomingTransition(trans.getModelComponentID(), removeCascadeDepth);
                        return;
                    }
                }
                foreach (ITransition trans in outgoingTransitions.Values)
                {
                    if (update.Equals(trans))
                    {
                        removeOutgoingTransition(trans.getModelComponentID(), removeCascadeDepth);
                        return;
                    }
                }
            }
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (incomingTransitions.ContainsKey(oldID))
            {
                ITransition element = incomingTransitions[oldID];
                incomingTransitions.Remove(oldID);
                incomingTransitions.Add(element.getModelComponentID(), element);
            }

            if (outgoingTransitions.ContainsKey(oldID))
            {
                ITransition element = outgoingTransitions[oldID];
                outgoingTransitions.Remove(oldID);
                outgoingTransitions.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

        public void setImplementedInterfaces(ISet<IState> implementedInterface, int removeCascadeDepth = 0)
        {
            foreach (IState implInterface in getImplementedInterfaces().Values)
            {
                removeImplementedInterfaces(implInterface.getModelComponentID(), removeCascadeDepth);
            }
            if (implementedInterface is null) return;
            foreach (IState implInterface in implementedInterface)
            {
                addImplementedInterface(implInterface);
            }
        }

        public void addImplementedInterface(IState implementedInterface)
        {
            if (implementedInterface is null) { return; }
            if (implementedInterfaces.TryAdd(implementedInterface.getModelComponentID(), implementedInterface))
            {
                publishElementAdded(implementedInterface);
                implementedInterface.register(this);
                addTriple(new IncompleteTriple(OWLTags.abstrImplements, implementedInterface.getUriModelComponentID()));
            }
        }

        public void removeImplementedInterfaces(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (implementedInterfaces.TryGetValue(id, out IState implInterface))
            {
                implementedInterfaces.Remove(id);
                implInterface.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.abstrImplements, implInterface.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IState> getImplementedInterfaces()
        {
            return new Dictionary<string, IState>(implementedInterfaces);
        }

        protected static readonly string STATE_REF_CLASS_NAME = "StateReference";

        /// <summary>
        /// Sets a state that is referenced by this state.
        /// According to the PASS standard, this functionality belongs to the "StateReference" class.
        /// Here, this functionality is inside the state class and should be used if the current instance should be a StateReference
        /// </summary>
        /// <param name="state">The referenced state</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        public virtual void setReferencedState(IState state, int removeCascadeDepth = 0)
        {
            if (!(state.GetType().Equals(this.GetType()))) return;
            IState oldReference = referenceState;
            // Might set it to null
            this.referenceState = state;

            if (oldReference != null)
            {
                if (oldReference.Equals(state)) return;
                oldReference.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdReferences, oldReference.getUriModelComponentID()));
                addTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + getClassName()));
                removeTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + STATE_REF_CLASS_NAME));
            }

            if (!(state is null))
            {
                state.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdReferences, state.getUriModelComponentID()));
                removeTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + getClassName()));
                addTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + STATE_REF_CLASS_NAME));
            }
        }

        /// <summary>
        /// Gets the state that is referenced by this state.
        /// According to the PASS standard, this functionality belongs to the "StateReference" class.
        /// Here, this functionality is inside the state class and should be used if the current instance should be a StateReference
        /// </summary>
        /// <returns>The referenced state</returns>
        public IState getReferencedState()
        {
            return referenceState;
        }

        public bool isReference()
        {
            return !(referenceState is null);
        }

        public override bool register(IValueChangedObserver<IPASSProcessModelElement> observer) { 
            bool added = base.register(observer); 
            // Special case: State is parsed, action knows about state, state does not know action -> action registeres at the state, state sets reference to action
            if (added && observer is IAction action && getAction() is null)
            {
                generateAction(action);
            }
            return added;
        }

        public override bool unregister(IValueChangedObserver<IPASSProcessModelElement> observer, int removeCascadeDepth = 0)
        {
            bool added = base.unregister(observer, removeCascadeDepth);
            // Special case: State is parsed, action knows about state, state does not know action -> action registeres at the state, state sets reference to action
            if (added && observer is IAction action && getAction().Equals(action))
            {
                generateAction(null);
            }
            return added;
        }
    }
}

