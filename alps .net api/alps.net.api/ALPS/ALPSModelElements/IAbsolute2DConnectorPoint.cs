namespace alps.net.api.ALPS
{
    /// <summary>
    /// An interface to define paths (consisting of points) for a simple visual representation of model elements
    /// A path is a double linked list of path points
    /// </summary>
    public interface IAbsolute2DConnectorPoint : IAbsolute2DVisualizationPoint
    {
        public void setIsConnectorPointOccupied(bool isOccupied);

        public bool isConnectorPointOccupied();
    }
}