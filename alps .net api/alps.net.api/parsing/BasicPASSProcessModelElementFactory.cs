using alps.net.api.StandardPASS;
using System.Collections.Generic;
using System.Linq;

namespace alps.net.api.parsing
{
    public class BasicPASSProcessModelElementFactory : IPASSProcessModelElementFactory<IParseablePASSProcessModelElement>
    {
        public string createInstance(IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict,
            IList<string> names, out IParseablePASSProcessModelElement element)
        {
            element = new PASSProcessModelElement();
            HashSet<string> bestParseableNames = new HashSet<string>();
            int lowestParseDiff = int.MaxValue;

            // Check how good the instanciations for the names are.
            // Only use the names where instanciation-pairs have lowest numbers
            foreach (string uriName in names)
            {
                string name = removeUri(uriName);
                if (parsingDict.ContainsKey(name))
                {

                    int count = 0;
                    foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) pair in parsingDict[name])
                    {
                        if (pair.Item2 == lowestParseDiff)
                        {
                            bestParseableNames.Add(name);
                        }
                        if (pair.Item2 < lowestParseDiff)
                        {
                            lowestParseDiff = pair.Item2;
                            bestParseableNames.Clear();
                            bestParseableNames.Add(name);
                        }
                        count++;
                    }
                }
            }

            IDictionary<IParseablePASSProcessModelElement, string> possibleElements = new Dictionary<IParseablePASSProcessModelElement, string>();
            // Gather all possible instanciations
            foreach (string name in bestParseableNames)
            {
                foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) pair in parsingDict[name])
                    possibleElements.Add(pair.Item1.getContent(), name);
            }

            if (bestParseableNames.Count > 1)
            {
                bool foundSubclass;

                // If one of the possible instances is superclass to another possible instance, throw the superclass out (only want most specific instance)
                do
                {
                    foundSubclass = false;
                    IList<IParseablePASSProcessModelElement> remove = new List<IParseablePASSProcessModelElement>();
                    foreach (IParseablePASSProcessModelElement someElement in possibleElements.Keys)
                    {
                        foreach (IParseablePASSProcessModelElement someOtherElement in possibleElements.Keys)
                        {
                            if (someElement != someOtherElement && someOtherElement.GetType().IsSubclassOf(someElement.GetType()))
                            {
                                remove.Add(someElement);
                                foundSubclass = true;
                                break;
                            }
                        }
                    }
                    foreach (IParseablePASSProcessModelElement someElement in remove)
                    {
                        possibleElements.Remove(someElement);
                    }
                } while (foundSubclass);
            }

            // Take the only element and return a new instance
            if (possibleElements.Count == 1)
            {
                //element = ReflectiveEnumerator.createInstance<IParseablePASSProcessModelElement>(possibleElements.Keys.First().GetType());
                element = possibleElements.Keys.First().getParsedInstance();
                return possibleElements.Values.First();
            }
            // Still some elements left that are both
            // - equally good in parsing
            // - no superclass to another class
            // parse the one having the longest matching name (longer name -> more specific instance ?)
            else if (possibleElements.Count > 1)
            {
                KeyValuePair<IParseablePASSProcessModelElement, string> selectedPair = decideForElement(possibleElements);
                element = selectedPair.Key.getParsedInstance();
                return selectedPair.Value;
            }

            return null;
        }
        private string removeUri(string stringWithUri)
        {
            string[] splitStr = stringWithUri.Split('#');
            return splitStr[splitStr.Length - 1];
        }

        protected virtual KeyValuePair<IParseablePASSProcessModelElement, string> decideForElement(IDictionary<IParseablePASSProcessModelElement, string> possibleElements)
        {
            int max = -1;
            KeyValuePair<IParseablePASSProcessModelElement, string> maxPair = new KeyValuePair<IParseablePASSProcessModelElement, string>();
            int counter = 0;
            foreach (KeyValuePair<IParseablePASSProcessModelElement, string> pair in possibleElements)
            {
                int parseability = pair.Key.canParse(pair.Value);
                if (parseability > max)
                {
                    max = parseability;
                    maxPair = pair;
                }
                counter++;
            }

            return maxPair;
            
        }
    }
}
