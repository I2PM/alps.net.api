using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// An interface to define paths (consisting of points) for a simple visual representation of model elements
    /// A path is a double linked list of path points
    /// </summary>
    public interface IVisualNodeDescription : IALPSModelElement
    {
        public void setAbsolute2D_Height(int h);

        public int getAbsolute2D_Height();


        public void setAbsolute2D_Width(int w);

        public int getAbsolute2D_Width();

        public void setAbsoluteBorderThickness(int t);

        public int getAbsoluteBorderThickness();

        public void setBorderStyle(BorderSyle style);

        public BorderSyle getBorderStyle();

        public void setTextAlignmentHorizontal(TextAlignmentHorizontal align);

        public TextAlignmentHorizontal getTextAlignmentHorizontal();


        public void setTextAlignmentVertical(TextAlignmentVertical align);

        public TextAlignmentVertical getTextAlignmentVertical();

        public void setTextFont(string font);

        public string getTextFont();

        public void setTextColor(string hexColor);

        public string getTextColor();

        public void setFillColor(string hexColor);

        public string getFillColor();

        public void setBorderColor(string hexColor);

        public string getBorderColor();


        public void setAnchorPoint(IAbsolute2DVisualizationPoint anchor, int removeCascadeDepth = 0);

        public IAbsolute2DVisualizationPoint getAnchorPoint();

        public void addConnectorPoint(IAbsolute2DConnectorPoint connectorPoint);


        public void setConnectorPoints(ISet<IAbsolute2DConnectorPoint> connectorPoints, int removeCascadeDepth = 0);

        public void removeConnectorPoint(string id, int removeCascadeDepth = 0);

        public IDictionary<string, IAbsolute2DConnectorPoint> getConnectorPoints();
    }

    public enum BorderSyle
    {
        SOLID = 0,
        DASHED = 1,
        DOTTED = 2,
    }

    public enum TextAlignmentHorizontal
    {
        LEFT = 0,
        CENTER = 1,
        RIGHT = 2,
    }
    public enum TextAlignmentVertical
    {
        TOP = 0,
        CENTER = 1,
        BOTTOM = 2,
    }


}