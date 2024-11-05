
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class VisualConnectionDescription : ALPSModelElement, IVisualConnectionDescription
    {

        protected int absoluteBoxBorderThickness;

        protected string hexBoxBorderColor;

        protected TextAlignmentHorizontal boxTextAlignHorizontal;

        protected TextAlignmentVertical boxTextAlignVertical;


        protected string hexConnectionColor;
        protected string hexTextColor;

        protected string textFont;
        protected IAbsolute2DConnectorPoint startingConnector;

        protected IAbsolute2DConnectorPoint endingConnector;
        protected ICompDict<string, IAbsolute2DVisualizationPoint> pathDescribingPoints = new CompDict<string, IAbsolute2DVisualizationPoint>();



        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "VisualConnectionDescription";

        public VisualConnectionDescription(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasPathDescribingPoints) && element is IAbsolute2DVisualizationPoint point)
                {
                    addPathDescribingPoint(point);
                    return true;
                }
                else if (element is IAbsolute2DConnectorPoint connectorPoint)
                {
                    if (predicate.Contains(OWLTags.hasStartingConnectorPoint))
                    {
                        setStartingConnectorPoint(connectorPoint);
                        return true;
                    }
                    else if (predicate.Contains(OWLTags.hasEndingConnectorPoint))
                    {
                        setEndingConnectorPoint(connectorPoint);
                        return true;
                    }
                }

            }
            else
            {

                if (predicate.Contains(OWLTags.hasAbsoluteBoxBorderThickness))
                {
                    string y = objectContent;
                    y = y.Split('^')[0];
                    setAbsoluteBoxBorderThickness(int.Parse(y));
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasTextFont))
                {
                    setTextFont(objectContent);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasConnectionColor))
                {
                    setConnectionColor(objectContent);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasTextColor))
                {
                    setTextColor(objectContent);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasBoxBorderColor))
                {
                    setBoxBorderColor(objectContent);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasBoxTextAlignHorizontal))
                {
                    string y = objectContent;
                    y = y.Split('^')[0];
                    int parsed = int.Parse(y);
                    if (parsed > -1 && parsed < 3)
                    {
                        setBoxTextAlignmentHorizontal((TextAlignmentHorizontal)parsed);
                        return true;
                    }
                }
                else if (predicate.Contains(OWLTags.hasBoxTextAlignVertical))
                {
                    string y = objectContent;
                    y = y.Split('^')[0];
                    int parsed = int.Parse(y);
                    if (parsed > -1 && parsed < 3)
                    {
                        setBoxTextAlignmentVertical((TextAlignmentVertical)parsed);
                        return true;
                    }
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public void setAbsoluteBoxBorderThickness(int t)
        {
            if (t == absoluteBoxBorderThickness || t < 0) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsoluteBoxBorderThickness, this.absoluteBoxBorderThickness.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            absoluteBoxBorderThickness = t;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsoluteBoxBorderThickness, absoluteBoxBorderThickness.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }

        public int getAbsoluteBoxBorderThickness()
        {
            return absoluteBoxBorderThickness;
        }


        public void setBoxTextAlignmentHorizontal(TextAlignmentHorizontal align)
        {
            if (align == boxTextAlignHorizontal) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBoxTextAlignHorizontal, this.boxTextAlignHorizontal.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            boxTextAlignHorizontal = align;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBoxTextAlignHorizontal, boxTextAlignHorizontal.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }

        public TextAlignmentHorizontal getBoxTextAlignmentHorizontal()
        {
            return boxTextAlignHorizontal;
        }


        public void setBoxTextAlignmentVertical(TextAlignmentVertical align)
        {
            if (align == boxTextAlignVertical) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBoxTextAlignVertical, this.boxTextAlignVertical.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            boxTextAlignVertical = align;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBoxTextAlignVertical, boxTextAlignVertical.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }

        public TextAlignmentVertical getBoxTextAlignmentVertical()
        {
            return boxTextAlignVertical;
        }

        public void setTextFont(string font)
        {
            if (font == textFont) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextFont, this.textFont, new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeString)));
            textFont = font;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextFont, textFont, new PASSTriple.LiteralDataType(OWLTags.xsdDataTypeString)));
        }

        public string getTextFont()
        {
            return textFont;
        }

        public void setTextColor(string hexColor)
        {
            if (hexColor == hexTextColor) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextColor, hexTextColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
            hexTextColor = hexColor;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextColor, hexTextColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
        }

        public string getTextColor()
        {
            return hexTextColor;
        }

        public void setConnectionColor(string hexColor)
        {
            if (hexColor == hexConnectionColor) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasConnectionColor, hexConnectionColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
            hexConnectionColor = hexColor;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasConnectionColor, hexConnectionColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
        }

        public string getConnectionColor()
        {
            return hexConnectionColor;
        }

        public void setBoxBorderColor(string hexColor)
        {
            if (hexColor == hexBoxBorderColor) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBoxBorderColor, this.hexBoxBorderColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
            hexBoxBorderColor = hexColor;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBoxBorderColor, hexBoxBorderColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
        }

        public string getBoxBorderColor()
        {
            return hexBoxBorderColor;
        }


        public void setStartingConnectorPoint(IAbsolute2DConnectorPoint connector, int removeCascadeDepth = 0)
        {
            IAbsolute2DVisualizationPoint oldStartingConnector = this.startingConnector;
            // Might set it to null
            this.startingConnector = connector;

            if (oldStartingConnector != null)
            {
                if (oldStartingConnector.Equals(connector)) return;
                oldStartingConnector.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasStartingConnectorPoint, oldStartingConnector.getUriModelComponentID()));
            }

            if (!(connector is null))
            {
                publishElementAdded(connector);
                connector.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasStartingConnectorPoint, connector.getUriModelComponentID()));
            }
        }

        public IAbsolute2DVisualizationPoint getStartingConnectorPoint()
        {
            return startingConnector;
        }

        public void setEndingConnectorPoint(IAbsolute2DConnectorPoint connector, int removeCascadeDepth = 0)
        {
            IAbsolute2DVisualizationPoint oldEndingConnector = this.endingConnector;
            // Might set it to null
            this.endingConnector = connector;

            if (oldEndingConnector != null)
            {
                if (oldEndingConnector.Equals(connector)) return;
                oldEndingConnector.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasEndingConnectorPoint, oldEndingConnector.getUriModelComponentID()));
            }

            if (!(connector is null))
            {
                publishElementAdded(connector);
                connector.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasEndingConnectorPoint, connector.getUriModelComponentID()));
            }
        }

        public IAbsolute2DVisualizationPoint getEndingConnectorPoint()
        {
            return endingConnector;
        }

        public void addPathDescribingPoint(IAbsolute2DVisualizationPoint point)
        {
            if (point is null) { return; }
            if (this.pathDescribingPoints.TryAdd(point.getModelComponentID(), point))
            {
                publishElementAdded(point);
                point.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasPathDescribingPoints, point.getUriModelComponentID()));
            }
        }


        public void setPathDescribingPoints(ISet<IAbsolute2DVisualizationPoint> points, int removeCascadeDepth = 0)
        {
            foreach (IAbsolute2DVisualizationPoint point in getPathDescribingPoints().Values)
            {
                removePathDescribingPoint(point.getModelComponentID(), removeCascadeDepth);
            }
            if (points is null) return;
            foreach (IAbsolute2DVisualizationPoint point in points)
            {
                addPathDescribingPoint(point);
            }
        }

        public void removePathDescribingPoint(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (pathDescribingPoints.TryGetValue(id, out IAbsolute2DVisualizationPoint connector))
            {
                pathDescribingPoints.Remove(id);
                connector.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasConnectorPoints, connector.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IAbsolute2DVisualizationPoint> getPathDescribingPoints()
        {
            return new Dictionary<string, IAbsolute2DVisualizationPoint>(pathDescribingPoints);
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new VisualConnectionDescription();
        }

        protected VisualConnectionDescription() { }
    }

}
