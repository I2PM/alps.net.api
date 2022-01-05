using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// An interface for classes that can (in a PASS context) implement other PASS objects which act as interfaces.
    /// </summary>
    /// <typeparam name="T">The type of the implemented classes, usually the type of the implementing class itself</typeparam>
    public interface IImplementingElement<T>
    {
        /// <summary>
        /// Sets the set of implemented interfaces for the instance
        /// </summary>
        /// <param name="implementedInterface">The set of implemented interfaces</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setImplementedInterfaces(ISet<T> implementedInterface, int removeCascadeDepth = 0);

        /// <summary>
        /// Adds an implemented interface
        /// </summary>
        /// <param name="implementedInterface">the new interface</param>
        void addImplementedInterface(T implementedInterface);

        /// <summary>
        /// Removes a specified interface from the set of implemented interfaces.
        /// </summary>
        /// <param name="id">the id of the interface that should be removed</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeImplementedInterfaces(string id, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the interfaces implemented by this instance
        /// </summary>
        /// <returns>the implemented interfaces</returns>
        IDictionary<string, T> getImplementedInterfaces();
    }
}
