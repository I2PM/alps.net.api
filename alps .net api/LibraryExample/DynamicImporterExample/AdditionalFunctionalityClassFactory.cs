using alps.net.api.parsing;
using System.Collections.Generic;

namespace LibraryExample.DynamicImporterExample
{
    /// <summary>
    /// This factory is passed to the library parser.
    /// It is consulted and has to decide for one class when the parser finds multiple available parsing options.
    /// </summary>
    public class AdditionalFunctionalityClassFactory : BasicPASSProcessModelElementFactory
    {
        protected override KeyValuePair<IParseablePASSProcessModelElement, string> decideForElement
            (IDictionary<IParseablePASSProcessModelElement, string> possibleElements)
        {
            // Always chose IAdditionalFunctionalityElements over standard classes,
            // if no IAdditionalFunctionalityElement is available let the standard implementation decide
            foreach (KeyValuePair<IParseablePASSProcessModelElement, string> pair in possibleElements)
            {
                if (pair.Key is IAdditionalFunctionalityElement) return pair;
            }
            return base.decideForElement(possibleElements);
        }
    }
}
