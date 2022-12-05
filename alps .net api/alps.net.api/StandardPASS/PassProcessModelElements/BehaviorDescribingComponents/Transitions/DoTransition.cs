using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents a DoTransition
    /// </summary>

    public class DoTransition : Transition, IDoTransition
    {
        protected int priorityNumber = 0;
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DoTransition";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DoTransition();
        }

       protected DoTransition() { }
        public DoTransition(IState sourceState, IState targetState, string labelForID = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard, int priorityNumber = 0, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null) : base(sourceState, targetState, labelForID, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) {
            setPriorityNumber(priorityNumber);
        }

        public DoTransition(ISubjectBehavior behavior, string label = null,
            IState sourceState = null, IState targetState = null, ITransitionCondition transitionCondition = null,
            ITransition.TransitionType transitionType = ITransition.TransitionType.Standard,
            int priorityNumber = 0, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, label, sourceState, targetState, transitionCondition, transitionType, comment, additionalLabel, additionalAttribute) {
            setPriorityNumber(priorityNumber);
        }

        public new void setSourceState(IState state, int removeCascadeDepth = 0)
        {
            if (state is IDoState)
            {
                base.setSourceState(state);
            }
        }


        public void setPriorityNumber(int positiveInteger)
        {
            if (positiveInteger == this.priorityNumber) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasPriorityNumber, this.priorityNumber.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));
            priorityNumber = (positiveInteger > 0) ? positiveInteger : 1;
            addTriple(new IncompleteTriple(OWLTags.stdHasPriorityNumber, priorityNumber.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypePositiveInteger));
        }


        public int getPriorityNumber()
        {
            return priorityNumber;
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasPriorityNumber))
            {
                string prio = objectContent;
                setPriorityNumber(int.Parse(prio));
                return true;
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

    }
}
