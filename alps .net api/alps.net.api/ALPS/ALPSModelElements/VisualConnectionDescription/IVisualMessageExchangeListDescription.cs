using alps.net.api.StandardPASS;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// An interface to define paths (consisting of points) for a simple visual representation of model elements
    /// A path is a double linked list of path points
    /// </summary>
    public interface IVisualMessageExchangeListDescription : IVisualConnectionDescription
    {
        public void setDescribedExchangeList(IMessageExchangeList exchange, int removeCascadeDepth = 0);

        public IMessageExchangeList getDescribedExchangeList();
    }
}