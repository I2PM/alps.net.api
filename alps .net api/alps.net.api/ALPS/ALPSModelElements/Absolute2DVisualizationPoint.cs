
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class Absolute2DVisualizationPoint : ALPSModelElement, IAbsolute2DVisualizationPoint
    {
        protected int absolutePosX;

        protected int absolutePosY;

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "Absolute2DVisualizationPoint";

        public Absolute2DVisualizationPoint(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element == null)
            {

                if (predicate.Contains(OWLTags.hasAbsolute2D_PosX))
                {
                    string x = objectContent;
                    x = x.Split('^')[0];
                    setAbsolute2D_PosX(int.Parse(x));
                    return true;
                }
                else if (predicate.Contains(OWLTags.hasAbsolute2D_PosY))
                {
                    string y = objectContent;
                    y = y.Split('^')[0];
                    setAbsolute2D_PosY(int.Parse(y));
                    return true;
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setAbsolute2D_PosX(int x)
        {
            if (x == absolutePosX || x < 0) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsolute2D_PosX, this.absolutePosX.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            absolutePosX = x;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsolute2D_PosX, absolutePosX.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }


        public void setAbsolute2D_PosY(int y)
        {
            if (y == absolutePosY || y < 0) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsolute2D_PosY, this.absolutePosY.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            absolutePosY = y;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsolute2D_PosY, absolutePosY.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));

        }



        public int getAbsolute2D_PosX()
        {
            return absolutePosX;
        }

        public int getAbsolute2D_PosY()
        {
            return absolutePosY;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new Absolute2DVisualizationPoint();
        }

        protected Absolute2DVisualizationPoint() { }
    }
}
