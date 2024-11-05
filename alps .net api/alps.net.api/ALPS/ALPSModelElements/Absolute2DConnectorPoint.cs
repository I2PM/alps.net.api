
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class Absolute2DConnectorPoint : Absolute2DVisualizationPoint, IAbsolute2DConnectorPoint
    {
        protected bool isOccupied;


        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "Absolute2DConnectorPoint";

        public Absolute2DConnectorPoint(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element == null)
            {

                if (predicate.Contains(OWLTags.isConnectorPointOccupied))
                {
                    string x = objectContent;
                    x = x.Split('^')[0];
                    setIsConnectorPointOccupied(bool.Parse(x));
                    return true;
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setIsConnectorPointOccupied(bool isOccupied)
        {
            if (isOccupied == this.isOccupied) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.isConnectorPointOccupied, (this.isOccupied ? 1 : 0).ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeBoolean)));
            this.isOccupied = isOccupied;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.isConnectorPointOccupied, (this.isOccupied ? 1 : 0).ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeBoolean)));
        }

        public bool isConnectorPointOccupied()
        {
            return this.isOccupied;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new Absolute2DConnectorPoint();
        }

        protected Absolute2DConnectorPoint() { }
    }
}
