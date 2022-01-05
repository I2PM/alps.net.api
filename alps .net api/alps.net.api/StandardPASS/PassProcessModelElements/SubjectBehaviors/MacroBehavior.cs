using System.Collections.Generic;
using alps.net.api.util;
using alps.net.api.StandardPASS.BehaviorDescribingComponents;
using System.Linq;
using alps.net.api.StandardPASS.InteractionDescribingComponents;
using alps.net.api.ALPS.ALPSModelElements;
using alps.net.api.parsing;

namespace alps.net.api.StandardPASS.SubjectBehaviors
{
    /// <summary>
    /// Class that represents an Macro behavior of a Subject
    /// According to standard pass 1.1.0
    /// </summary>

    public class MacroBehavior : SubjectBehavior, IMacroBehavior
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "MacroBehavior";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MacroBehavior();
        }

       protected MacroBehavior() { }
        public MacroBehavior(IModelLayer layer, string labelForID = null, ISubject subject = null, ISet<IBehaviorDescribingComponent> components = null,
            ISet<IStateReference> stateReferences = null, IState initialStateOfBehavior = null, int priorityNumber = 0, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, subject, components, initialStateOfBehavior, priorityNumber, comment, additionalLabel, additionalAttribute)
        {
            if (stateReferences != null)
                foreach (IStateReference reference in stateReferences)
                    addBehaviorDescribingComponent(reference);
        }

        public IDictionary<string, IStateReference> getStateReferences()
        {
            return new Dictionary<string, IStateReference>(getBehaviorDescribingComponents().OfType<KeyValuePair<string, IStateReference>>());
        }


        public IDictionary<string, IGenericReturnToOriginReference> getReturnReferences()
        {
            return new Dictionary<string, IGenericReturnToOriginReference>(getBehaviorDescribingComponents().OfType<KeyValuePair<string, IGenericReturnToOriginReference>>());
        }



    }
}
