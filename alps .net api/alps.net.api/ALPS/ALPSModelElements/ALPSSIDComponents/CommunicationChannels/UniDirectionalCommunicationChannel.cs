using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;
using System.IO;

namespace alps.net.api.ALPS.ALPSModelElements.ALPSSIDComponents
{
    class UniDirectionalCommunicationChannel : CommunicationChannel, IUniDirectionalCommunicationChannel
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "UniDirectionalCommunicationChannel";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new UniDirectionalCommunicationChannel();
        }

       protected UniDirectionalCommunicationChannel() { }
        /// <summary>
        /// Constructor that creates a new fully specified instance of the UniDirectionalCommunicationChannel class
        /// </summary>
        /// <param name="additionalAttribute"></param>
        /// <param name="modelComponentID"></param>
        /// <param name="modelComponentLabel"></param>
        /// <param name="comment"></param>
        public UniDirectionalCommunicationChannel(IModelLayer layer, string labelForID = null, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

    }
}
