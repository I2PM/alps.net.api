using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS.SubjectBehaviors;
using alps.net.api.util;
using System.Collections.Generic;
using static alps.net.api.StandardPASS.BehaviorDescribingComponents.IState;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a receive state
    /// </summary>
    public class ReceiveState : StandardPASSState, IReceiveState
    {

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "ReceiveState";
        protected string exportTag = OWLTags.std;
        protected string exportClassname = className;

        public override string getClassName()
        {
            return exportClassname;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ReceiveState();
        }

       protected ReceiveState() { }
        protected override string getExportTag()
        {
            return exportTag;
        }

        public ReceiveState(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IReceiveFunction functionSpecification = null,
            ISet<ITransition> incomingTransition = null, ISet<IReceiveTransition> outgoingTransition = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, labelForID, guardBehavior, functionSpecification, incomingTransition, (ISet<ITransition>)outgoingTransition, comment, additionalLabel, additionalAttribute)
        { }

        public new IReceiveFunction getFunctionSpecification()
        {
            return (IReceiveFunction)base.getFunctionSpecification();
        }



        public new void setFunctionSpecification(IFunctionSpecification specification, int removeCascadingDepth = 0)
        {
            if (specification is IReceiveFunction)
            {
                base.setFunctionSpecification(specification, removeCascadingDepth);
            }
            else
            {
                base.setFunctionSpecification(null);
            }
        }

        public override void addOutgoingTransition(ITransition transition)
        {
            if (transition is IReceiveTransition)
                base.addOutgoingTransition(transition);
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasFunctionSpecification) && element is IReceiveFunction receiveFunction)
                {
                    setFunctionSpecification(receiveFunction);
                    return true;
                }
            }
            else if (predicate.Contains(OWLTags.type))
            {
                if (objectContent.Contains("AbstractReceiveState"))
                {
                    setIsStateType(StateType.Abstract);
                    return true;
                }
                else if (objectContent.Contains("FinalizedReceiveState"))
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
                        if (!stateType.Equals(StateType.EndState)) { base.setIsStateType(stateType); }
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


    }
}