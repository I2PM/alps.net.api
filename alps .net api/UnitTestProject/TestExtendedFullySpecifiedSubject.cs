using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TestExtendedFullySpecifiedSubject : FullySpecifiedSubject
    {

        protected TestExtendedFullySpecifiedSubject() { }

        public TestExtendedFullySpecifiedSubject(IModelLayer layer, string labelForID = null, ISet<IMessageExchange> incomingMessageExchange = null,
            ISubjectBaseBehavior subjectBaseBehavior = null, ISet<ISubjectBehavior> subjectBehaviors = null,
            ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1, ISubjectDataDefinition subjectDataDefinition = null,
            ISet<IInputPoolConstraint> inputPoolConstraints = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, incomingMessageExchange, subjectBaseBehavior, subjectBehaviors, outgoingMessageExchange, maxSubjectInstanceRestriction,
                  subjectDataDefinition, inputPoolConstraints, comment, additionalLabel, additionalAttribute)
        { }

        

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new TestExtendedFullySpecifiedSubject();
        }
    }
}
