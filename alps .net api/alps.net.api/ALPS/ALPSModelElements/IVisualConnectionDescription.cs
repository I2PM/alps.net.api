using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// An interface to define paths (consisting of points) for a simple visual representation of model elements
    /// A path is a double linked list of path points
    /// </summary>
    public interface IVisualConnectionDescription : IALPSModelElement
    {

        public void setAbsoluteBoxBorderThickness(int t);

        public int getAbsoluteBoxBorderThickness();


        public void setBoxTextAlignmentHorizontal(TextAlignmentHorizontal align);

        public TextAlignmentHorizontal getBoxTextAlignmentHorizontal();


        public void setBoxTextAlignmentVertical(TextAlignmentVertical align);

        public TextAlignmentVertical getBoxTextAlignmentVertical();

        public void setTextFont(string font);

        public string getTextFont();

        public void setTextColor(string hexColor);

        public string getTextColor();

        public void setConnectionColor(string hexColor);

        public string getConnectionColor();

        public void setBoxBorderColor(string hexColor);

        public string getBoxBorderColor();


        public void setStartingConnectorPoint(IAbsolute2DConnectorPoint connector, int removeCascadeDepth = 0);

        public IAbsolute2DVisualizationPoint getStartingConnectorPoint();

        public void setEndingConnectorPoint(IAbsolute2DConnectorPoint connector, int removeCascadeDepth = 0);

        public IAbsolute2DVisualizationPoint getEndingConnectorPoint();

        public void addPathDescribingPoint(IAbsolute2DVisualizationPoint point);


        public void setPathDescribingPoints(ISet<IAbsolute2DVisualizationPoint> points, int removeCascadeDepth = 0);

        public void removePathDescribingPoint(string id, int removeCascadeDepth = 0);

        public IDictionary<string, IAbsolute2DVisualizationPoint> getPathDescribingPoints();
    }



}