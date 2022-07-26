using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    class BiDirectionalCommunicationChannel : CommunicationChannel, IBiDirectionalCommunicationChannel
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "BiDirectionalCommunicationChannel";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new BiDirectionalCommunicationChannel();
        }

       protected BiDirectionalCommunicationChannel() { }
        /// <summary>
        /// Constructor that creates a new fully specified instance of the BiDirectionalCommunicationChannel class
        /// </summary>
        public BiDirectionalCommunicationChannel(IModelLayer layer, string labelForID = null, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
    }
}
