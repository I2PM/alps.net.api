namespace alps.net.api.ALPS
{
    /// <summary>
    /// An interface to define paths (consisting of points) for a simple visual representation of model elements
    /// A path is a double linked list of path points
    /// </summary>
    public interface IAbsolute2DVisualizationPoint : IALPSModelElement
    {
        public void setAbsolute2D_PosX(int x);

        public void setAbsolute2D_PosY(int y);

        public int getAbsolute2D_PosX();

        public int getAbsolute2D_PosY();
    }
}