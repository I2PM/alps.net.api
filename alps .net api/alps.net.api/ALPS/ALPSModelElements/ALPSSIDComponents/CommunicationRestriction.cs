using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;
using System.IO;
using VDS.RDF;

namespace alps.net.api.ALPS.ALPSModelElements.ALPSSIDComponents
{
    class CommunicationRestriction : ALPSSIDComponent, ICommunicationRestriction
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "CommunicationRestriction";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new CommunicationRestriction();
        }

       protected CommunicationRestriction() { }
        public CommunicationRestriction(IModelLayer layer, string labelForID = null, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
    }
}
