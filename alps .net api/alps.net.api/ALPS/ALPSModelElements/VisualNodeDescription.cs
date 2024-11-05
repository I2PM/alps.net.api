
using alps.net.api.parsing;
using alps.net.api.parsing.graph;
using alps.net.api.src;
using alps.net.api.util;
using System;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class VisualNodeDescription : ALPSModelElement, IVisualNodeDescription
    {
        protected int absoluteWidth;

        protected int absoluteHeight;

        protected int absoluteBorderThickness;

        protected BorderSyle borderStyle;

        protected TextAlignmentHorizontal textAlignHorizontal;

        protected TextAlignmentVertical textAlignVertical;

        protected string hexBorderColor;
        protected string hexFillColor;
        protected string hexTextColor;

        protected string textFont;
        protected IAbsolute2DVisualizationPoint anchorPoint;
        protected ICompDict<string, IAbsolute2DConnectorPoint> connectorPoints = new CompDict<string, IAbsolute2DConnectorPoint>();



        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "VisualNodeDescription";

        public VisualNodeDescription(string labelForID = null, string comment = null, string additionalLabel = null, IList<IPASSTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.hasAnchorPoint) && element is IAbsolute2DVisualizationPoint anchorPoint)
                {
                    setAnchorPoint(anchorPoint);
                    return true;
                }
                else if (predicate.Contains(OWLTags.hasConnectorPoints) && element is IAbsolute2DConnectorPoint connectorPoint)
                {
                    addConnectorPoint(connectorPoint);
                    return true;
                }

            }
            else
            {

                if (predicate.Contains(OWLTags.hasAbsolute2D_Height))
                {
                    string x = objectContent;
                    x = x.Split('^')[0];
                    setAbsolute2D_Height(int.Parse(x));
                    return true;
                }
                else if (predicate.Contains(OWLTags.hasAbsolute2D_Width))
                {
                    string y = objectContent;
                    y = y.Split('^')[0];
                    setAbsolute2D_Width(int.Parse(y));
                    return true;
                }
                else if (predicate.Contains(OWLTags.hasAbsoluteBorderThickness))
                {
                    string y = objectContent;
                    y = y.Split('^')[0];
                    setAbsoluteBorderThickness(int.Parse(y));
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasTextFont))
                {
                    setTextFont(objectContent);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasFillColor))
                {
                    setFillColor(objectContent);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasTextColor))
                {
                    setTextColor(objectContent);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasBorderColor))
                {
                    setBorderColor(objectContent);
                    return true;
                }

                else if (predicate.Contains(OWLTags.hasBorderStyle))
                {
                    string y = objectContent;
                    y = y.Split('^')[0];
                    int parsed = int.Parse(y);
                    if (parsed > -1 && parsed < 3)
                    {
                        setBorderStyle((BorderSyle)parsed);
                        return true;
                    }
                }
                else if (predicate.Contains(OWLTags.hasTextAlignHorizontal))
                {
                    string y = objectContent;
                    y = y.Split('^')[0];
                    int parsed = int.Parse(y);
                    if (parsed > -1 && parsed < 3)
                    {
                        setTextAlignmentHorizontal((TextAlignmentHorizontal)parsed);
                        return true;
                    }
                }
                else if (predicate.Contains(OWLTags.hasTextAlignVertical))
                {
                    string y = objectContent;
                    y = y.Split('^')[0];
                    int parsed = int.Parse(y);
                    if (parsed > -1 && parsed < 3)
                    {
                        setTextAlignmentVertical((TextAlignmentVertical)parsed);
                        return true;
                    }
                }

            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void setAbsolute2D_Height(int h)
        {
            if (h == absoluteHeight || h < 0) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsolute2D_Height, this.absoluteHeight.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            absoluteHeight = h;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsolute2D_Height, absoluteHeight.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }

        public int getAbsolute2D_Height()
        {
            return absoluteHeight;
        }


        public void setAbsolute2D_Width(int w)
        {
            if (w == absoluteWidth || w < 0) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsolute2D_Width, this.absoluteWidth.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            absoluteWidth = w;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsolute2D_Width, absoluteWidth.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }

        public int getAbsolute2D_Width()
        {
            return absoluteWidth;
        }

        public void setAbsoluteBorderThickness(int t)
        {
            if (t == absoluteBorderThickness || t < 0) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsoluteBorderThickness, this.absoluteBorderThickness.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            absoluteBorderThickness = t;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAbsoluteBorderThickness, absoluteBorderThickness.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }

        public int getAbsoluteBorderThickness()
        {
            return absoluteBorderThickness;
        }

        public void setBorderStyle(BorderSyle style)
        {
            if (style == borderStyle) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBorderStyle, this.borderStyle.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            borderStyle = style;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBorderStyle, borderStyle.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }

        public BorderSyle getBorderStyle()
        {
            return borderStyle;
        }

        public void setTextAlignmentHorizontal(TextAlignmentHorizontal align)
        {
            if (align == textAlignHorizontal) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextAlignHorizontal, this.textAlignHorizontal.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            textAlignHorizontal = align;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextAlignHorizontal, textAlignHorizontal.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }

        public TextAlignmentHorizontal getTextAlignmentHorizontal()
        {
            return textAlignHorizontal;
        }


        public void setTextAlignmentVertical(TextAlignmentVertical align)
        {
            if (align == textAlignVertical) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextAlignVertical, this.textAlignVertical.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
            textAlignVertical = align;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextAlignVertical, textAlignVertical.ToString(), new PASSTriple.LiteralDataType(OWLTags.xsdDataTypePositiveInteger)));
        }

        public TextAlignmentVertical getTextAlignmentVertical()
        {
            return textAlignVertical;
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
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextColor, this.hexTextColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
            hexTextColor = hexColor;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasTextColor, hexTextColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
        }

        public string getTextColor()
        {
            return hexTextColor;
        }

        public void setFillColor(string hexColor)
        {
            if (hexColor == hexFillColor) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasFillColor, this.hexFillColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
            hexFillColor = hexColor;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasFillColor, hexFillColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
        }

        public string getFillColor()
        {
            return hexFillColor;
        }

        public void setBorderColor(string hexColor)
        {
            if (hexColor == hexBorderColor) return;
            removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBorderColor, this.hexBorderColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
            hexBorderColor = hexColor;
            addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasBorderColor, hexBorderColor, new PASSTriple.LiteralDataType(OWLTags.xsdHexBinary)));
        }

        public string getBorderColor()
        {
            return hexBorderColor;
        }


        public void setAnchorPoint(IAbsolute2DVisualizationPoint anchor, int removeCascadeDepth = 0)
        {
            IAbsolute2DVisualizationPoint oldAnchor = this.anchorPoint;
            // Might set it to null
            this.anchorPoint = anchor;

            if (oldAnchor != null)
            {
                if (oldAnchor.Equals(anchor)) return;
                oldAnchor.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAnchorPoint, oldAnchor.getUriModelComponentID()));
            }

            if (!(anchor is null))
            {
                publishElementAdded(anchor);
                anchor.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasAnchorPoint, anchor.getUriModelComponentID()));
            }
        }

        public IAbsolute2DVisualizationPoint getAnchorPoint()
        {
            return anchorPoint;
        }

        public void addConnectorPoint(IAbsolute2DConnectorPoint connectorPoint)
        {
            if (connectorPoint is null) { return; }
            if (this.connectorPoints.TryAdd(connectorPoint.getModelComponentID(), connectorPoint))
            {
                publishElementAdded(connectorPoint);
                connectorPoint.register(this);
                addTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasConnectorPoints, connectorPoint.getUriModelComponentID()));
            }
        }


        public void setConnectorPoints(ISet<IAbsolute2DConnectorPoint> connectorPoints, int removeCascadeDepth = 0)
        {
            foreach (IAbsolute2DConnectorPoint connectorPoint in getConnectorPoints().Values)
            {
                removeConnectorPoint(connectorPoint.getModelComponentID(), removeCascadeDepth);
            }
            if (connectorPoints is null) return;
            foreach (IAbsolute2DConnectorPoint connectorPoint in connectorPoints)
            {
                addConnectorPoint(connectorPoint);
            }
        }

        public void removeConnectorPoint(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (connectorPoints.TryGetValue(id, out IAbsolute2DConnectorPoint connector))
            {
                connectorPoints.Remove(id);
                connector.unregister(this, removeCascadeDepth);
                removeTriple(new PASSTriple(getExportXmlName(), OWLTags.abstr + OWLTags.hasConnectorPoints, connector.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IAbsolute2DConnectorPoint> getConnectorPoints()
        {
            return new Dictionary<string, IAbsolute2DConnectorPoint>(connectorPoints);
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new VisualNodeDescription();
        }

        protected VisualNodeDescription() { }
    }

}
