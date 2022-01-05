using alps.net.api.parsing;
using alps.net.api.StandardPASS.SubjectBehaviors;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents an GenericReturnToOriginReference
    /// </summary>


    public class GenericReturnToOriginReference : State, IGenericReturnToOriginReference
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "GenericReturnToOriginReference";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new GenericReturnToOriginReference();
        }

       protected GenericReturnToOriginReference() { }
        public GenericReturnToOriginReference(ISubjectBehavior behavior, string labelForID = null, IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null,
            ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, labelForID, guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        { }

    }
}
