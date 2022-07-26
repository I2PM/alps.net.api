using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface to the macro state class
    /// <br></br>
    /// <br></br>
    /// From PASS Doc: <i>A state that references a macro behavior that is executed upon entering this state. Only after executing the macro behavior this state is finished also.</i>
    /// </summary>

    public interface IMacroState : IState
    {
        /// <summary>
        /// Sets the macro behavior that is referenced by the macro state
        /// </summary>
        /// <param name="macroBehavior">the macro behavior</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReferencedMacroBehavior(IMacroBehavior macroBehavior, int removeCascadeDepth = 0);

        /// <summary>
        /// Gets the macro behavior that is referenced by the macro state
        /// </summary>
        /// <returns>the macro behavior</returns>
        IMacroBehavior getReferencedMacroBehavior();

        /// <summary>
        /// Adds a StateReference to the set of contained StateReferences.
        /// </summary>
        /// <param name="stateReference">the new state reference</param>
        void addStateReference(IStateReference stateReference);

        /// <summary>
        /// Remove a StateReference from the list of current StateReferences
        /// </summary>
        /// <param name="stateRefID">the id of the reference</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void removeStateReference(string stateRefID, int removeCascadeDepth = 0);

        /// <summary>
        /// Overrides all current StateReferences
        /// </summary>
        /// <param name="references">the new references</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setStateReferences(ISet<IStateReference> references, int removeCascadeDepth = 0);

        /// <summary>
        /// Return all the StateReferences
        /// </summary>
        /// <returns>all references</returns>
        IDictionary<string, IStateReference> getStateReferences();

    }
}
