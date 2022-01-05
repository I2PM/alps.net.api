using System;
using System.Collections.Generic;

namespace alps.net.api.parsing
{
    class GenericPASSProcessModelElementFactoryAdapter<T> : IPASSProcessModelElementFactory<T> where T : IParseablePASSProcessModelElement
    {
        private readonly IPASSProcessModelElementFactory<IParseablePASSProcessModelElement> factory;

        public GenericPASSProcessModelElementFactoryAdapter()
        {
            factory = new BasicPASSProcessModelElementFactory();
        }

        public string createInstance(Dictionary<string, List<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict, List<string> names, out T element)
        {
            string parsed = factory.createInstance(parsingDict, names, out IParseablePASSProcessModelElement outElement);
            try
            {
                element = (T)Convert.ChangeType(outElement, typeof(T));
            }
            catch (InvalidCastException)
            {
                element = default;
            }
            return parsed;
        }

    }
}
