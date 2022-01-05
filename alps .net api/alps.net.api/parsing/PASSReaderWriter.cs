using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Ontology;
using VDS.RDF.Writing;
using alps.net.api.StandardPASS;
using alps.net.api.util;

namespace alps.net.api.parsing
{
    /// <summary>
    /// The main Class of the alps_.net_api. It is the controll class that imports and exports all the given Data. 
    /// If any model data is imported, this class returns a class which contains the given information in form of 
    /// objects. 
    /// If models should be exported, the parser creates a new empty file with the given filename and creates an 
    /// owl file at the same place where the imported data comes from.
    /// </summary>
    public class PASSReaderWriter : IPASSReaderWriter
    {
        /// <summary>
        /// The element factory gets an uri that should be parsed and a list of possible instances of classes this uri can be instanciated with.
        /// The list of possible instances is stored inside the parsingDict.
        /// the element factory than decides which instance to use.
        /// </summary>
        private IPASSProcessModelElementFactory<IParseablePASSProcessModelElement> elementFactory = new BasicPASSProcessModelElementFactory();



        private readonly List<IPASSProcessModel> passProcessModells = new List<IPASSProcessModel>();

        private ITreeNode<IParseablePASSProcessModelElement> treeRootNode;
        private IList<OntologyClass> baseClasses = new List<OntologyClass>();

        private readonly OntologyGraph parsingStructureOntologyGraph = new OntologyGraph();
        private readonly List<OntologyGraph> loadedModelGraphs = new List<OntologyGraph>();
        private readonly Dictionary<string, List<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict = new Dictionary<string, List<(ITreeNode<IParseablePASSProcessModelElement>, int)>>();

        public IList<OntologyGraph> getLoadedModelGraphs()
        {
            return loadedModelGraphs;
        }

        /// <summary>
        /// Creates an empty Instance of the OwlGraph Class
        /// </summary>
        private PASSReaderWriter()
        {
            string path = Directory.GetCurrentDirectory();

            if (File.Exists(path + "\\logs\\" + "logfile.txt"))
            {
                File.Delete(path + "\\logs\\" + "logfile.txt");
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                //.WriteTo.Console()
                .WriteTo.File("logs\\logfile.txt")
                .CreateLogger();
        }

        private static PASSReaderWriter graph;

        public static PASSReaderWriter getInstance()
        {
            if (graph is null)
            {
                graph = new PASSReaderWriter();
            }
            return graph;
        }

        public void loadOWLParsingStructure(IList<string> filepathsToOWLFiles)
        {
            IList<OntologyGraph> owlStructureGraphs = new List<OntologyGraph>();
            foreach (string filepath in filepathsToOWLFiles)
            {
                try
                {
                    // Create a new OntologyGraph
                    OntologyGraph owlGraph = new OntologyGraph();
                    // Load files into it
                    owlGraph.LoadFromFile(filepath);
                    owlStructureGraphs.Add(owlGraph);
                }
                catch (RdfParseException parseException)
                {
                    Log.Error("Parser Error when reading the new File " + parseException);
                }
            }
            loadOWLParsingStructure(owlStructureGraphs);

        }

        private void loadOWLParsingStructure(IList<OntologyGraph> owlStructureGraphs, ProgressBar bar = null)
        {
            treeRootNode = null;
            baseClasses.Clear();
            parsingStructureOntologyGraph.Clear();

            foreach (OntologyGraph owlGraph in owlStructureGraphs)
            {
                parsingStructureOntologyGraph.Merge(owlGraph);

                /*
                 * This was only a workaround for a bug where an ontology references classes of another ontology,
                 * but does not use the correct base uri (the classes are not merged correctly)
                 */
                /*// Merge it into one big graph
                if (parsingStructureOntologyGraph.IsEmpty)
                    parsingStructureOntologyGraph.Merge(owlGraph);
                else
                    mergeIntoGraph(parsingStructureOntologyGraph, owlGraph);*/
            }
            if (bar != null) bar.increaseProgress();
            // Create the inheritance tree for the loaded owl classes
            baseClasses = createOWLInheritanceTree();
            // Create the inheritance tree for the loaded owl classes
            if (bar != null) bar.increaseProgress();
            treeRootNode = createClassInheritanceTree();
            if (bar != null) bar.increaseProgress();

            foreach (OntologyClass baseClass in baseClasses)
            {
                createParsingStructureFromTrees(baseClass, treeRootNode);
            }
            if (bar != null) bar.increaseProgress();
        }


        public IList<IPASSProcessModel> loadModels(IList<string> filepaths, bool overrideOWLParsingStructure = false)
        {
            ProgressBar bar = new ProgressBar(5);
            passProcessModells.Clear();
            loadedModelGraphs.Clear();

            IList<OntologyGraph> owlStructureGraphs = new List<OntologyGraph>();
            foreach (string filepath in filepaths)
            {
                try
                {
                    // Create a new OntologyGraph
                    OntologyGraph owlGraph = new OntologyGraph();
                    // Load files into it
                    owlGraph.LoadFromFile(filepath);
                    owlStructureGraphs.Add(owlGraph);

                    if (!isStandardPass(owlGraph.BaseUri.ToString())) { loadedModelGraphs.Add(owlGraph); }
                    Log.Information("Done reading the new File: " + filepath);
                }
                catch (RdfParseException parseException)
                {
                    Log.Error("Parser Error when reading the new File " + parseException);
                }
            }
            if (overrideOWLParsingStructure) { loadOWLParsingStructure(owlStructureGraphs, bar); }

            createAllElements();

            bar.increaseProgress();
            Console.WriteLine("Finished loading the new in memory models");

            return passProcessModells;
        }

        /*
         * This was only a workaround for a bug where an ontology references classes of another ontology,
         * but does not use the correct base uri (the classes are not merged correctly)
         */
        /*private void mergeIntoGraph(OntologyGraph baseGraph, OntologyGraph mergeGraph)
        {
            foreach (OntologyClass mergeOntClass in mergeGraph.OwlClasses)
            {
                if (mergeOntClass.ToString().Contains("auto")) continue;
                IList<OntologyClass> mergeWithClasses = new List<OntologyClass>();
                foreach (OntologyClass baseOntClass in baseGraph.OwlClasses)
                {
                    if (removeUri(baseOntClass.ToString()).Equals(removeUri(mergeOntClass.ToString())))
                    {
                        mergeWithClasses.Add(baseOntClass);

                    }
                }
                foreach (OntologyClass baseOntClass in mergeWithClasses)
                {
                    mergeClasses(baseGraph, baseOntClass, mergeOntClass);
                }
            }
        }

        private void mergeClasses(OntologyGraph baseGraph, OntologyClass mergeInto, OntologyClass mergeFrom)
        {
            IList<OntologyClass> childsToBeAdded = new List<OntologyClass>(mergeFrom.DirectSubClasses);
            foreach (OntologyClass subclass in mergeFrom.DirectSubClasses)
            {
                foreach (OntologyClass subClassMerge in mergeInto.DirectSubClasses)
                {
                    if (removeUri(subClassMerge.ToString()).Equals(removeUri(subclass.ToString())))
                    {
                        childsToBeAdded.Remove(subclass);
                        mergeClasses(baseGraph, subClassMerge, subclass);
                        break;
                    }
                }
            }
            foreach (OntologyClass subclass in childsToBeAdded)
            {
                INode subclassNode = baseGraph.CreateUriNode(new Uri(subclass.ToString()));


                //mergeInto.AddSubClass(newSubclass);
                INode subclassPropertyNode = baseGraph.GetUriNode(new Uri("http://www.w3.org/2000/01/rdf-schema#subClassOf"));
                INode classPropertyNode = baseGraph.GetUriNode(new Uri("http://www.w3.org/2002/07/owl#Class"));
                INode typePropertyNode = baseGraph.GetUriNode(new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"));
                INode mergeClassNode = baseGraph.GetUriNode(new Uri(mergeInto.ToString()));

                baseGraph.Assert(new Triple(subclassNode, typePropertyNode, classPropertyNode));
                baseGraph.Assert(new Triple(subclassNode, subclassPropertyNode, mergeClassNode));
                //http://www.w3.org/2000/01/rdf-schema#subClassOf
                OntologyClass newSubclass = baseGraph.OwlClasses.Where(ontClass => ontClass.ToString().Equals(subclass.ToString())).First();
                mergeClasses(baseGraph, newSubclass, subclass);

                *//*{
                http://www.w3.org/2002/07/owl#Class}
                    {
                    http://www.w3.org/1999/02/22-rdf-syntax-ns#type}*//*
            }
        }*/

        private string removeUri(string stringWithUri)
        {
            string[] splitStr = stringWithUri.Split("#");
            return splitStr[splitStr.Length - 1];
        }

        /// <summary>
        /// Creates a parsing dictionary containing Ontology urls as keys and instances of c# classes that can be used to parse the ontology classes.
        /// If an ontlogy class cannot be parsed using any path through both trees (i.e. the trees differ in some places), this class and all its childs will be parsed
        /// using the last instance class where the trees did not differ.
        /// </summary>
        /// <param name="ontClass"></param>
        /// <param name="rootNode"></param>
        private void createParsingStructureFromTrees(OntologyClass ontClass, ITreeNode<IParseablePASSProcessModelElement> rootNode)
        {
            // Start with mapping the roots, they are both PASSProcessModelElement
            if (parsingDict.Count == 0) parsingDict.Add(removeUri(ontClass.Resource.ToString()), new List<(ITreeNode<IParseablePASSProcessModelElement>, int)> { (rootNode, 0) });
            // Create a new dictionary for those urls that could not be mapped properly (need that later)
            Dictionary<OntologyClass, ITreeNode<IParseablePASSProcessModelElement>> unmappableDict = new Dictionary<OntologyClass, ITreeNode<IParseablePASSProcessModelElement>>();

            // Start to parse childs, passing the parent ontology class and the parent C#-class, as well as the dict off classes already parsed (only the root)
            parseChilds(parsingDict, ontClass, rootNode, unmappableDict);

            Log.Information("Created parsing structure for owl classes.");
            Log.Information(parsingDict.Count + " classes could be mapped correctly");
            Log.Information(unmappableDict.Count + " were not mapped correctly");
            foreach (KeyValuePair<OntologyClass, ITreeNode<IParseablePASSProcessModelElement>> pair in unmappableDict)
            {
                mapRestWithParentNode(parsingDict, pair.Key, pair.Value, 1);
            }
        }

        /// <summary>
        /// Tries to find C#-classes that can parse given urls of the ontology
        /// both structures (the ontology inheritance classes as well as the c# inheritance classes) are structured as a tree
        /// This method is called recursive. It tries to map each child of the given ontology parent class with one or more childs of the given c# parent class.
        /// It is asserted that the c# parent class was previously selected to be able to parse the ontology parent class (else this would make no sense)
        /// this algorithm is trying to map both tree structures together, containing the mapping inside a parsing dictionary.
        /// If a part of the Ontology tree cannot be mapped, the algorithm marks this url in another dict and does not try to map the childs of this element.
        /// </summary>
        /// <param name="parsingDict">Keeps all the valid mappings found down to the current class</param>
        /// <param name="parentOntClass">the parent Ontology class that can be parsed with the instance of IPASSProcessModelElement in the current parentNode</param>
        /// <param name="parentNode">A node containing an instance representing a valid parsing class for the Ontology class given by parentOntClass</param>
        /// <param name="unmappableDict">A dict of elements that could not be mapped</param>
        private void parseChilds(Dictionary<string, List<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict, OntologyClass parentOntClass, ITreeNode<IParseablePASSProcessModelElement> parentNode,
            Dictionary<OntologyClass, ITreeNode<IParseablePASSProcessModelElement>> unmappableDict)
        {
            // Go through all child classes of the ontology class
            foreach (OntologyClass directSubclass in parentOntClass.DirectSubClasses)
            {
                List<ITreeNode<IParseablePASSProcessModelElement>> parseableClasses = new List<ITreeNode<IParseablePASSProcessModelElement>>();
                string url = removeUri(directSubclass.Resource.ToString());
                // Go through all child classes of the parent class instance
                foreach (ITreeNode<IParseablePASSProcessModelElement> childNode in parentNode.getChildNodes())
                {
                    // Check if the class is a correct instanciation for the current url
                    if (childNode.getContent().canParse(url) != IParseablePASSProcessModelElement.CANNOT_PARSE)
                    {
                        // add it to parseable classes
                        parseableClasses.Add(childNode);
                    }
                }

                // Add the url to the list of classes that could not be parsed correctly.
                // The parent could be parsed correctly (else this method would not have been called for the child),
                // So the unparsed child will be parsed by using the class that can parse the parent
                if (parseableClasses.Count == 0 && !parsingDict.ContainsKey(url))
                {
                    unmappableDict.TryAdd(directSubclass, parentNode);
                }
                else
                {
                    foreach (OntologyClass ontClass in unmappableDict.Keys)
                    {
                        if (removeUri(ontClass.Resource.ToString()).Equals(url)) { unmappableDict.Remove(ontClass); break; }
                    }

                    // We only want to store the most specific instantiation of a class.
                    // All classes that are base classes to the currently found class - with our class still being able to parse the url - will be removed from the list
                    List<(ITreeNode<IParseablePASSProcessModelElement>, int)> toBeRemoved = new List<(ITreeNode<IParseablePASSProcessModelElement>, int)>();
                    if (parsingDict.ContainsKey(url))
                    {
                        foreach (ITreeNode<IParseablePASSProcessModelElement> element in parseableClasses)
                        {
                            foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) tuple in parsingDict[url])
                            {
                                ITreeNode<IParseablePASSProcessModelElement> node = tuple.Item1;
                                if (element.isSubClassOf(node))
                                {
                                    toBeRemoved.Add(tuple);
                                }
                            }
                        }
                    }
                    // Remove all unnecessary classes (because currently found classes are more specific)
                    foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) tuple in toBeRemoved)
                    {
                        parsingDict[url].Remove(tuple);
                    }

                    // Add all classes that can parse the url
                    foreach (ITreeNode<IParseablePASSProcessModelElement> element in parseableClasses)
                    {
                        if (parsingDict.ContainsKey(url))
                        {
                            parsingDict[url].Clear();
                        }
                        addToParsingDict(parsingDict, directSubclass, element, 0);
                    }

                    // Then parse the childs of the matched pair respectively:
                    // Now passing the current url and the matched instance as parents
                    foreach (ITreeNode<IParseablePASSProcessModelElement> element in parseableClasses)
                    {
                        parseChilds(parsingDict, directSubclass, element, unmappableDict);
                    }
                }
            }
        }

        /// <summary>
        /// Adds mapped elements to the parsing dictionary.
        /// If there exists a list for the given key, the value is being added to the existing list.
        /// </summary>
        /// <param name="parsingDict">the dictionary used for parsing</param>
        /// <param name="ontClass">the Ontology class used (url as key)</param>
        /// <param name="element">the instance that can parse the ontology class (used as value)</param>
        private void addToParsingDict(Dictionary<string, List<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict, OntologyClass ontClass, ITreeNode<IParseablePASSProcessModelElement> element, int depth)
        {
            string ontRessource = removeUri(ontClass.Resource.ToString());
            if (parsingDict.ContainsKey(ontRessource))
            {
                parsingDict[ontRessource].Add((element, depth));
            }
            else parsingDict.Add(ontRessource, new List<(ITreeNode<IParseablePASSProcessModelElement>, int)> { (element, depth) });
        }

        private void mapRestWithParentNode(Dictionary<string, List<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict, OntologyClass ontClass,
            ITreeNode<IParseablePASSProcessModelElement> parentNode, int depth)
        {
            string ontRessource = removeUri(ontClass.Resource.ToString());
            if (!(parsingDict.ContainsKey(ontRessource)))
            {
                Log.Error("Could not map " + ontRessource + " correctly, mapped with " + parentNode.getContent().getClassName() + " instead");
                addToParsingDict(parsingDict, ontClass, parentNode, depth);
            }
            foreach (OntologyClass childOntClass in ontClass.DirectSubClasses.ToList())
            {
                if (!(parsingDict.ContainsKey(removeUri(childOntClass.Resource.ToString()))))
                {
                    mapRestWithParentNode(parsingDict, childOntClass, parentNode, depth + 1);
                }
            }
        }

        /// <summary>
        /// Checks whether an Ontology class has a parent class or not (in the given graph)
        /// </summary>
        /// <param name="ontClass">the Ontology class</param>
        /// <returns>true if a parent class exists, false if not</returns>
        private bool hasParentClass(OntologyClass ontClass)
        {
            if (ontClass.SuperClasses.ToList().Count > 0)
            {
                foreach (OntologyClass superClass in ontClass.SuperClasses.ToList())
                {
                    if (!(superClass.ToString().ToLower().Contains("auto"))) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This class creates the inheritance tree out of the given files
        /// </summary>
        private IList<OntologyClass> createOWLInheritanceTree()
        {
            List<OntologyClass> nodesWithoutParents = new List<OntologyClass>();

            // Find a root for the tree by finding alls classes that have no parent
            foreach (OntologyClass ontClass in parsingStructureOntologyGraph.OwlClasses)
            {
                if (!ontClass.ToString().Contains("auto"))
                {
                    // Get all nodes that have no parent and are classes
                    if (hasParentClass(ontClass) == false)
                    {
                        nodesWithoutParents.Add(ontClass);
                    }
                }
            }

            IList<OntologyClass> baseClasses = new List<OntologyClass>();

            // Set the PassProcessModelElement as root
            foreach (OntologyClass ontNode in nodesWithoutParents)
            {
                if (ontNode.ToString().ToLower().Contains("passprocessmodelelement"))
                {
                    baseClasses.Add(ontNode);
                }
            }
            if (baseClasses.Count > 0) return baseClasses;
            Log.Error("No PassProcessModelElement found in the loaded graphs");
            return null;
        }

        /// <summary>
        /// Creates the inheritance Graph out of hardcoded classes
        /// WIP: Hier müssen noch die korrekten Namen der Elemente (URIs) eingetragen werden, das hab ich immernoch nicht gemacht, aber irgendwie ändert das gerade auch nichts mehr
        /// </summary>
        private ITreeNode<IParseablePASSProcessModelElement> createClassInheritanceTree()
        {
            ITreeNode<IParseablePASSProcessModelElement> treeRootNode = new TreeNode<IParseablePASSProcessModelElement>(new PASSProcessModelElement());
            findChildsAndAdd(treeRootNode);

            //addTreeToPassObject(treeRootNode);

            Log.Information("Finished Creating the Standard Pass Tree");
            return treeRootNode;
        }

        private void findChildsAndAdd(ITreeNode<IParseablePASSProcessModelElement> node)
        {
            //IEnumerable<IPASSProcessModelElement> enumerable = ReflectiveEnumerator.getEnumerableOfType<IPASSProcessModelElement>(node.getContent(), new object[] { null });
            IEnumerable<IParseablePASSProcessModelElement> enumerable = ReflectiveEnumerator.getEnumerableOfType(node.getContent());
            foreach (IParseablePASSProcessModelElement element in enumerable.ToList())
            {
                node.addChild(new TreeNode<IParseablePASSProcessModelElement>(element));
            }
            foreach (TreeNode<IParseablePASSProcessModelElement> childNode in node.getChildNodes())
            {
                findChildsAndAdd(childNode);
            }
        }

        /// <summary>
        /// Verifies whether a triple (as string) is part of a standard pass definition owl or a model
        /// </summary>
        /// <param name="someString">a triple converted to string</param>
        /// <returns>true if it is part of a standard pass document, false if it is only part of a normal model</returns>
        private bool isStandardPass(Triple triple)
        {
            return isStandardPass(triple.ToString());
        }

        /// <summary>
        /// Verifies whether a triple (as string) is part of a standard pass definition owl or a model
        /// </summary>
        /// <param name="someString">a triple converted to string</param>
        /// <returns>true if it is part of a standard pass document, false if it is only part of a normal model</returns>
        private bool isStandardPass(String trpl)
        {
            return (trpl.Contains("standard") && trpl.Contains("pass")) || (trpl.Contains("abstract") && trpl.Contains("pass"));
        }

        /// <summary>
        /// Finds and creates all the named individuals in the given files and creates a new list with all the individuals
        /// </summary>
        private void createAllElements()
        {
            foreach (IGraph graph in loadedModelGraphs)
            {
                IEnumerable<Triple> triplesWithNamedIndividualSubject;
                List<Triple> namedIndividualsList = new List<Triple>();
                Dictionary<string, List<string>> namedIndividualsDict = new Dictionary<string, List<string>>();

                // Iterate over triples in the graph
                foreach (Triple triple in graph.Triples)
                {
                    // Add named individuals
                    if (triple.Object.ToString().Contains("NamedIndividual") && triple.Subject.ToString().Contains("#") && !isStandardPass(triple))
                    {
                        namedIndividualsList.Add(triple);
                    }
                }


                foreach (Triple t in namedIndividualsList)
                {
                    triplesWithNamedIndividualSubject = graph.Triples.WithSubject(t.Subject);

                    foreach (Triple l in triplesWithNamedIndividualSubject)
                    {
                        // Get the one triple that specifies type and does not contain NamedIndividual as object
                        if (l.Predicate.ToString().Contains("type") && !l.Object.ToString().Contains("NamedIndividual"))
                        {
                            //this.namedIndiviualsType.Add(l);
                            if (!namedIndividualsDict.ContainsKey(t.Subject.ToString()))
                            {
                                namedIndividualsDict.Add(l.Subject.ToString(), new List<string> { l.Object.ToString() });
                            }
                            else
                            {
                                namedIndividualsDict[l.Subject.ToString()].Add(l.Object.ToString());
                            }
                        }
                    }
                }


                createInstances(graph, namedIndividualsDict);
            }
        }

        /// <summary>
        /// Method that creates the empty Instances of the Objects which later get completed by the completetObjects() Methode
        /// </summary>
        /// <returns></returns>
        private List<IPASSProcessModel> createInstances(IGraph graph, Dictionary<string, List<string>> namedIndividualsDict)
        {
            IDictionary<string, IParseablePASSProcessModelElement> createdElements = new Dictionary<string, IParseablePASSProcessModelElement>();

            foreach (KeyValuePair<string, List<string>> pair in namedIndividualsDict)
            {


                string uri = elementFactory.createInstance(parsingDict, pair.Value, out IParseablePASSProcessModelElement modelElement);
                string[] splittedURI;
                if (uri is null)
                {
                    splittedURI = pair.Value[0].Split('#');
                }
                else
                {
                    splittedURI = uri.Split('#');
                }
                string name = splittedURI[splittedURI.Length - 1];

                // If the factory found a fitting element
                // Hand the element a callback (this), and the pair that describes the current element
                if (!(modelElement is null))
                {
                    modelElement.addTriples(new List<Triple>(graph.GetTriplesWithSubject(graph.GetUriNode(new Uri(pair.Key)))));

                    if (modelElement is IPASSProcessModel passProcessModell)
                    {

                        //passProcessModell.addAllAttributes(this, pair.Key);

                        passProcessModells.Add(passProcessModell);
                        passProcessModell.setBaseURI(pair.Key.Split('#')[0]);
                        IPASSGraph modelBaseGraph = passProcessModell.getBaseGraph();
                        foreach (Triple triple in graph.Triples.WithSubject(graph.GetUriNode(new Uri(pair.Key.Split('#')[0]))))
                        {
                            modelBaseGraph.addTriple(triple);
                        }

                    }
                    else
                    {
                        // if the factory could not find a fitting element
                        // TODO set Model before adding Attributes
                        //modelElement.addAllAttributes(this, pair.Key); 
                        //modelElement.setModelComponentID(pair.Key);
                    }
                    createdElements.Add(modelElement.getModelComponentID(), modelElement);
                }
            }
            if (passProcessModells.Count == 0)
            {
                IPASSProcessModel passProcessModell = new PASSProcessModel(graph.BaseUri.ToString());
                passProcessModell.createUniqueModelComponentID("defaultModelID");
                passProcessModells.Add(passProcessModell);
            }

            foreach (IPASSProcessModel model in passProcessModells)
            {
                if (model is IParseablePASSProcessModelElement parseable)
                    parseable.completeObject(ref createdElements);
            }

            // Was tun mit elementen die in keinem Model sind? Wie prüfen?

            return passProcessModells;
        }


        public string exportModel(IPASSProcessModel model, string filepath, out IGraph exportGraph)
        {
            // Get the graph hold by the model and use the export function given by the library
            string fullPath = (filepath.EndsWith(".owl")) ? filepath : filepath + ".owl";
            model.getBaseGraph().exportTo(fullPath);
            if (model.getBaseGraph() is PASSGraph graph)
                exportGraph = graph.getGraph();
            else exportGraph = null;
            return fullPath;
        }

        public string exportModel(IPASSProcessModel model, string filepath)
        {
            return exportModel(model, filepath, out IGraph graph);
        }
    }
}
