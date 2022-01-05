using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;
using System.IO;

namespace alps.net.api.ALPS.ALPSModelElements.ALPSSIDComponents
{
    /// <summary>
    /// Method that represents an abstract communication channel
    /// </summary>
    public class CommunicationChannel : ALPSSIDComponent, ICommunicationChannel
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "CommunicationChannel";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new CommunicationChannel();
        }

       protected CommunicationChannel() { }
        public CommunicationChannel(IModelLayer layer, string labelForID = null, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

    }
}
