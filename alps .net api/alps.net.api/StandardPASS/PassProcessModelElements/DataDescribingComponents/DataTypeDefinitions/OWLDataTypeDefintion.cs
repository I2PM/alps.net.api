using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS.DataDescribingComponents
{
    /// <summary>
    /// Class that represents a OWL data type definition
    /// </summary>
    public class OWLDataTypeDefintion : CustomOrExternalDataTypeDefinition, IOWLDataTypeDefintion
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "OWLDataTypeDefinition";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new OWLDataTypeDefintion();
        }

       protected OWLDataTypeDefintion() { }
        public OWLDataTypeDefintion(IPASSProcessModel model, string labelForID = null, ISet<IDataObjectDefinition> dataObjectDefiniton = null,
            string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(model, labelForID, dataObjectDefiniton, comment, additionalLabel, additionalAttribute)
        { }

    }
}