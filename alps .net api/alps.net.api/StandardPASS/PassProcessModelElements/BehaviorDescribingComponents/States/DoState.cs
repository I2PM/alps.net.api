using alps.net.api.util;
using System.Collections.Generic;
using alps.net.api.parsing;
using alps.net.api.src;
using static alps.net.api.StandardPASS.IState;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a DoState
    /// </summary>

    public class DoState : StandardPASSState, IDoState
    {
        protected readonly ICompatibilityDictionary<string, IDataMappingIncomingToLocal> dataMappingIncomingToLocalDict = new CompatibilityDictionary<string, IDataMappingIncomingToLocal>();
        protected readonly ICompatibilityDictionary<string, IDataMappingLocalToOutgoing> dataMappingLocalToOutgoingDict = new CompatibilityDictionary<string, IDataMappingLocalToOutgoing>();

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DoState";
        protected string exportTag = OWLTags.std;
        protected string exportClassname = className;


        public override string getClassName()
        {
            return exportClassname;
        }


        protected override string getExportTag()
        {
            return exportTag;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DoState();
        }

       protected DoState() { }
        public DoState(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IDoFunction doFunction = null, ISet<ITransition> incomingTransitions = null, ISet<ITransition> outgoingTransitions = null,
            ISet<IDataMappingIncomingToLocal> dataMappingIncomingToLocal = null, ISet<IDataMappingLocalToOutgoing> dataMappingLocalToOutgoing = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttributes = null)
            : base(behavior, labelForID, guardBehavior, doFunction, incomingTransitions, null, comment, additionalLabel, additionalAttributes)
        {
            // Set those attributes locally and pass null to base (so no wrong attributes will be set)
            setDataMappingFunctionsIncomingToLocal(dataMappingIncomingToLocal);
            setDataMappingFunctionsLocalToOutgoing(dataMappingLocalToOutgoing);
            setOutgoingTransitions(outgoingTransitions);
        }


        // #################### DataMappingFunctions ####################

        public void setDataMappingFunctionsIncomingToLocal(ISet<IDataMappingIncomingToLocal> dataMappingIncomingToLocal, int removeCascadeDepth = 0)
        {
            foreach (IDataMappingIncomingToLocal mapping in getDataMappingFunctionsIncomingToLocal().Values)
            {
                removeDataMappingFunctionIncomingToLocal(mapping.getModelComponentID(), removeCascadeDepth);
            }
            if (dataMappingIncomingToLocal is null) return;
            foreach (IDataMappingIncomingToLocal mapping in dataMappingIncomingToLocal)
            {
                addDataMappingFunctionIncomingToLocal(mapping);
            }
        }

        public void addDataMappingFunctionIncomingToLocal(IDataMappingIncomingToLocal dataMappingIncomingToLocal)
        {
            if (dataMappingIncomingToLocal is null) { return; }
            if (dataMappingIncomingToLocalDict.TryAdd(dataMappingIncomingToLocal.getModelComponentID(), dataMappingIncomingToLocal))
            {
                publishElementAdded(dataMappingIncomingToLocal);
                dataMappingIncomingToLocal.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasDataMappingFunction, dataMappingIncomingToLocal.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IDataMappingIncomingToLocal> getDataMappingFunctionsIncomingToLocal()
        {
            return new Dictionary<string, IDataMappingIncomingToLocal>(dataMappingIncomingToLocalDict);
        }

        public void removeDataMappingFunctionIncomingToLocal(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (dataMappingIncomingToLocalDict.TryGetValue(id, out IDataMappingIncomingToLocal mapping))
            {
                dataMappingIncomingToLocalDict.Remove(id);
                mapping.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdHasDataMappingFunction, mapping.getUriModelComponentID()));
            }
        }


        public void setDataMappingFunctionsLocalToOutgoing(ISet<IDataMappingLocalToOutgoing> dataMappingLocalToOutgoing, int removeCascadeDepth = 0)
        {
            foreach (IDataMappingLocalToOutgoing mapping in getDataMappingFunctionsLocalToOutgoing().Values)
            {
                removeDataMappingFunctionLocalToOutgoing(mapping.getModelComponentID(), removeCascadeDepth);
            }
            if (dataMappingLocalToOutgoing is null) return;
            foreach (IDataMappingLocalToOutgoing mapping in dataMappingLocalToOutgoing)
            {
                addDataMappingFunctionLocalToOutgoing(mapping);
            }
        }

        public void addDataMappingFunctionLocalToOutgoing(IDataMappingLocalToOutgoing dataMappingLocalToOutgoing)
        {
            if (dataMappingLocalToOutgoing is null) { return; }
            if (dataMappingLocalToOutgoingDict.TryAdd(dataMappingLocalToOutgoing.getModelComponentID(), dataMappingLocalToOutgoing))
            {
                publishElementAdded(dataMappingLocalToOutgoing);
                dataMappingLocalToOutgoing.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasDataMappingFunction, dataMappingLocalToOutgoing.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IDataMappingLocalToOutgoing> getDataMappingFunctionsLocalToOutgoing()
        {
            return new Dictionary<string, IDataMappingLocalToOutgoing>(dataMappingLocalToOutgoingDict);
        }

        public void removeDataMappingFunctionLocalToOutgoing(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (dataMappingLocalToOutgoingDict.TryGetValue(id, out IDataMappingLocalToOutgoing mapping))
            {
                dataMappingLocalToOutgoingDict.Remove(id);
                mapping.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdHasDataMappingFunction, mapping.getUriModelComponentID()));
            }
        }


        // ########################################


        public override void addOutgoingTransition(ITransition transition)
        {
            if (!(transition is ISendTransition || transition is IReceiveTransition))
                base.addOutgoingTransition(transition);
        }


        public new void setFunctionSpecification(IFunctionSpecification specification, int removeCascadeDepth = 0)
        {
            if (specification is IDoFunction)
            {
                base.setFunctionSpecification(specification, removeCascadeDepth);
            }
            else
            {
                base.setFunctionSpecification(null, removeCascadeDepth);
            }
        }

        public new IDoFunction getFunctionSpecification()
        {
            return (IDoFunction)base.getFunctionSpecification();
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasDataMappingFunction) && element is IDataMappingIncomingToLocal incomingMapping)
                {
                    addDataMappingFunctionIncomingToLocal(incomingMapping);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasDataMappingFunction) && element is IDataMappingLocalToOutgoing outgoingMapping)
                {
                    addDataMappingFunctionLocalToOutgoing(outgoingMapping);
                    return true;
                }
                else if (predicate.Contains(OWLTags.hasFunctionSpecification) && element is IDoFunction function)
                {
                    setFunctionSpecification(function);
                    return true;
                }
            }
            else if (predicate.Contains(OWLTags.type))
            {
                if (objectContent.Contains("AbstractDoState"))
                {
                    setIsStateType(StateType.Abstract);
                    return true;
                }
                else if (objectContent.Contains("FinalizedDoState"))
                {
                    setIsStateType(StateType.Finalized);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override void setIsStateType(StateType stateType)
        {
            if (!stateTypes.Contains(stateType))
            {
                switch (stateType)
                {
                    case StateType.Abstract:
                        removeTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + getClassName()));
                        exportTag = OWLTags.abstr;
                        exportClassname = "Abstract" + className;
                        addTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + getClassName()));
                        if (isStateType(StateType.Finalized))
                            removeStateType(StateType.Finalized);
                        break;
                    case StateType.Finalized:
                        removeTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + getClassName()));
                        exportTag = OWLTags.abstr;
                        exportClassname = "Finalized" + className;
                        addTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + getClassName()));
                        if (isStateType(StateType.Abstract))
                            removeStateType(StateType.Abstract);
                        break;
                    default:
                        base.setIsStateType(stateType);
                        break;
                }
            }
        }



        public override void removeStateType(StateType stateType)
        {
            if (stateTypes.Contains(stateType))
            {
                switch (stateType)
                {
                    case StateType.Abstract:
                        removeTriple(new IncompleteTriple(OWLTags.rdfType, OWLTags.std + "Abstract" + getExportTag() + getClassName()));
                        stateTypes.Remove(stateType);
                        exportTag = OWLTags.std;
                        exportClassname = className;
                        addTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + getClassName()));
                        break;
                    case StateType.Finalized:
                        removeTriple(new IncompleteTriple(OWLTags.rdfType, OWLTags.std + "Finalized" + getExportTag() + getClassName()));
                        stateTypes.Remove(stateType);
                        exportTag = OWLTags.std;
                        exportClassname = className;
                        addTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + getClassName()));
                        break;
                    default:
                        base.removeStateType(stateType);
                        break;
                }
            }
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IDataMappingIncomingToLocal component in getDataMappingFunctionsIncomingToLocal().Values)
                baseElements.Add(component);
            foreach (IDataMappingLocalToOutgoing component in getDataMappingFunctionsLocalToOutgoing().Values)
                baseElements.Add(component);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IDataMappingIncomingToLocal mappingIn)
                    removeDataMappingFunctionIncomingToLocal(mappingIn.getModelComponentID(), removeCascadeDepth);
                if (update is IDataMappingLocalToOutgoing mappingOut)
                    removeDataMappingFunctionLocalToOutgoing(mappingOut.getModelComponentID(), removeCascadeDepth);
            }
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (dataMappingIncomingToLocalDict.ContainsKey(oldID))
            {
                IDataMappingIncomingToLocal element = dataMappingIncomingToLocalDict[oldID];
                dataMappingIncomingToLocalDict.Remove(oldID);
                dataMappingIncomingToLocalDict.Add(element.getModelComponentID(), element);
            }

            if (dataMappingLocalToOutgoingDict.ContainsKey(oldID))
            {
                IDataMappingLocalToOutgoing element = dataMappingLocalToOutgoingDict[oldID];
                dataMappingLocalToOutgoingDict.Remove(oldID);
                dataMappingLocalToOutgoingDict.Add(element.getModelComponentID(), element);
            }

            base.notifyModelComponentIDChanged(oldID, newID);
        }
    }
}
