using alps.net.api.StandardPASS;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// An interface to define paths (consisting of points) for a simple visual representation of model elements
    /// A path is a double linked list of path points
    /// </summary>
    public interface IVisualStateDescription : IVisualNodeDescription
    {
        public void setDescribedState(IState state, int removeCascadeDepth = 0);

        public IState getDescribedState();
    }
}