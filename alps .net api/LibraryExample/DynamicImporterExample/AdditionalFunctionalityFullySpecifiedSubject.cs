
using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace LibraryExample.DynamicImporterExample
{
    /// <summary>
    /// This class extends the FullySpecifiedSubject with new functionality.
    /// It should be parsed instead of the standard FullySpecifiedSubject implementation in the library
    /// </summary>
    public class AdditionalFunctionalityFullySpecifiedSubject : FullySpecifiedSubject, IAdditionalFunctionalityElement
    {
        private string additionalFunctionality;

        /// <summary>
        /// Protected Constructor for getParsedInstance(), so no parameters will be set here or in base implementation
        /// </summary>
        protected AdditionalFunctionalityFullySpecifiedSubject() { }

        /// <summary>
        /// All parameters are nullable or define a default value, the base constructor is used
        /// </summary>
        /// <param name="additionalFunctionality">Some additional functionality</param>
        public AdditionalFunctionalityFullySpecifiedSubject(IModelLayer layer, string additionalFunctionality = null, string labelForID = null, ISet<IMessageExchange> incomingMessageExchange = null,
            ISubjectBaseBehavior subjectBaseBehavior = null, ISet<ISubjectBehavior> subjectBehaviors = null,
            ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1, ISubjectDataDefinition subjectDataDefinition = null,
            ISet<IInputPoolConstraint> inputPoolConstraints = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(layer, labelForID, incomingMessageExchange, subjectBaseBehavior, subjectBehaviors, outgoingMessageExchange, maxSubjectInstanceRestriction,
                  subjectDataDefinition, inputPoolConstraints, comment, additionalLabel, additionalAttribute)
        {
            setAdditionalFunctionality(additionalFunctionality);
        }

        /// <summary>
        /// Needed to provide fresh instances to the factory.
        /// It is called every time an instance for a 
        /// </summary>
        /// <returns></returns>
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            // Uses the protected constructor, this way we can differ between instances that were parsed (and should not contain any pre-set values)
            // And instances that are created while modeling, and should possibly contain pre-set values.
            return new AdditionalFunctionalityFullySpecifiedSubject();
        }

        public string getAdditionalFunctionality()
        {
            return additionalFunctionality;
        }

        public void setAdditionalFunctionality(string additionalFunctionality)
        {
            this.additionalFunctionality = additionalFunctionality;
        }
    }
}
