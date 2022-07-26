using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an InputConstraintHandlingStrategy
    /// </summary>

    public class InputPoolConstraintHandlingStrategy : InteractionDescribingComponent, IInputPoolConstraintHandlingStrategy
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "InputPoolContstraintHandlingStrategy";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new InputPoolConstraintHandlingStrategy();
        }

       protected InputPoolConstraintHandlingStrategy() { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="labelForID"></param>
        /// <param name="comment"></param>
        /// <param name="additionalLabel"></param>
        /// <param name="additionalAttribute"></param>
        public InputPoolConstraintHandlingStrategy(IModelLayer layer, string labelForID = null, string comment = null,
            string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        { }


    }
}
