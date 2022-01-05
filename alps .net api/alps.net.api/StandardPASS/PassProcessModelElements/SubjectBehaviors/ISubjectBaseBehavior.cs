using alps.net.api.StandardPASS.BehaviorDescribingComponents;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS.SubjectBehaviors
{
    /// <summary>
    /// Interface to the subject base behavior class
    /// </summary>
    public interface ISubjectBaseBehavior : ISubjectBehavior
    {
        /// <summary>
        /// Get all the end states this behavior contains.
        /// All these are as well listed in the overall amount of BehaviorDescribingComponent()s this behavior holds.
        /// </summary>
        /// <returns>A dictionary of states with their ids as keys</returns>
        IDictionary<string, IState> getEndStates();
    }

}
