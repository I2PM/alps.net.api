using alps.net.api.StandardPASS.BehaviorDescribingComponents;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Interface of the receive state class
    /// </summary>
    public interface IReceiveState : IStandardPASSState
    {
        /// <summary>
        /// Method that sets the receive function attribute of the instance
        /// </summary>
        /// <param name="specification">the function specification</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        new void setFunctionSpecification(IFunctionSpecification specification, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the receive function attribute of the instance
        /// </summary>
        /// <returns>The receive function attribute of the instance</returns>
        new IReceiveFunction getFunctionSpecification();

    }

}
