using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using alps.net.api.util;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TestPASSProcessModel : PASSProcessModel
    {


        public TestPASSProcessModel(string baseURI, string labelForID = null, ISet<IMessageExchange> messageExchanges = null, ISet<ISubject> relationsToModelComponent = null,
            ISet<ISubject> startSubject = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(baseURI, labelForID, messageExchanges, relationsToModelComponent, startSubject, comment, additionalLabel, additionalAttribute) { }

        protected TestPASSProcessModel() { }


        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new TestPASSProcessModel();
        }
    }
}
