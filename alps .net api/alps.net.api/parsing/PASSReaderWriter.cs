using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Ontology;
using alps.net.api.StandardPASS;
using alps.net.api.util;

namespace alps.net.api.parsing
{
    /// <summary>
    /// The main parser class.
    /// 
    /// To load a model contained in an owl/rdf formatted file, either use
    /// <br></br>
    /// - loadOWLParsingStructure() and pass path references to used ontology classes (i.e. the standard-pass-ont.owl) and afterwards
    /// loadModels(paths) with path references to the owl files containing the models or
    /// <br></br>
    /// - loadModels(paths, true) with path references to the owl files containing the models.
    /// <br></br>
    /// Loading the parsing structure is expensive, so it is advised to do it seperately and not reload it if not neccessary.
    /// </summary>
    public class PASSReaderWriter : IPASSReaderWriter
    {
        /// <summary>
        /// The element factory gets an uri that should be parsed and a list of possible instances of classes this uri can be instanciated with.
        /// The list of possible instances is stored inside the parsingDict.
        /// the element factory than decides which instance to use.
        /// </summary>
        private IPASSProcessModelElementFactory<IParseablePASSProcessModelElement> elementFactory = new BasicPASSProcessModelElementFactory();


        private ParsingTreeMatcher matcher = new();
        private readonly IList<IPASSProcessModel> passProcessModells = new List<IPASSProcessModel>();
        private readonly OntologyGraph parsingStructureOntologyGraph = new();
        private readonly IList<OntologyGraph> loadedModelGraphs = new List<OntologyGraph>();
        private IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> parsingDict
            = new Dictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>>();

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

        private static PASSReaderWriter readerWriter;

        public static PASSReaderWriter getInstance()
        {
            if (readerWriter is null)
            {
                readerWriter = new PASSReaderWriter();
            }
            return readerWriter;
        }

        public IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> getParsingDict()
        {
            IDictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> newParsingDict =
                new Dictionary<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>>();
            foreach(KeyValuePair<string, IList<(ITreeNode<IParseablePASSProcessModelElement>, int)>> pair in parsingDict)
            {
                newParsingDict.Add(pair.Key, new List<(ITreeNode<IParseablePASSProcessModelElement>, int)>(pair.Value));
            }
            return newParsingDict;
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
                    Console.WriteLine();
                    // TODO error loggen
                }
            }
            parsingDict = matcher.loadOWLParsingStructure(owlStructureGraphs, parsingStructureOntologyGraph);

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
            if (overrideOWLParsingStructure)
                parsingDict = matcher.loadOWLParsingStructure(owlStructureGraphs, parsingStructureOntologyGraph, bar);

            createAllElements();

            bar.increaseProgress();
            Console.WriteLine("Finished loading the new in memory models");

            return passProcessModells;
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
                IList<Triple> namedIndividualsList = new List<Triple>();
                IDictionary<string, IList<string>> namedIndividualsDict = new Dictionary<string, IList<string>>();

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
        /// Method that creates the empty Instances of the Objects which later get completed by the completetObjects() method
        /// </summary>
        /// <param name="namedIndividualsDict">A dictionary containing the uri for each NamedIndividual as key and the type(s) as value (in the form of a list)</param>
        /// <param name="graph">The graph used for parsing</param>
        /// <returns></returns>
        private IList<IPASSProcessModel> createInstances(IGraph graph, IDictionary<string, IList<string>> namedIndividualsDict)
        {
            IDictionary<string, IParseablePASSProcessModelElement> createdElements = new Dictionary<string, IParseablePASSProcessModelElement>();

            //Object:     owl:Ontology
            //Predicate:  rdf:type
            // The base uri for the current parsing graph
            string baseUri = graph.Triples.WithObject(graph.GetUriNode("owl:Ontology")).First().Subject.ToString();

            foreach (KeyValuePair<string, IList<string>> pair in namedIndividualsDict)
            {

                // Generates a new modelElement and returns the type this element is instantiated with
                // (i.e. an abstract DoState has the types "AbstractState" and "DoState", but is instantiated with a DoState, so this method returns "DoState"
                string elementType = elementFactory.createInstance(parsingDict, pair.Value, out IParseablePASSProcessModelElement modelElement);

                // If the factory found a fitting element
                if (!(modelElement is null))
                {
                    // The model element receives its triples which define all its characteristics in the form of incomplete triples
                    // Incomplete triples carry no information about the subject, as the subjects uri can change during parsing.
                    IList<Triple> elementTriples = new List<Triple>(graph.GetTriplesWithSubject(graph.GetUriNode(new Uri(pair.Key))));
                    IList<IIncompleteTriple> elementIncompleteTriples = new List<IIncompleteTriple>();

                    foreach (Triple triple in elementTriples)
                        elementIncompleteTriples.Add(new IncompleteTriple(triple, baseUri));
                    modelElement.addTriples(elementIncompleteTriples);

                    // Important! The ModelComponentID is overwritten by the suffix of the elements uri (= "baseuri#suffix").
                    if (elementTriples.Count > 0)
                        modelElement.setModelComponentID(StaticFunctions.removeBaseUri(elementTriples[0].Subject.ToString(), baseUri));

                    if (modelElement is IPASSProcessModel passProcessModell)
                    {
                        passProcessModells.Add(passProcessModell);
                        passProcessModell.setBaseURI(baseUri);
                        IPASSGraph modelBaseGraph = passProcessModell.getBaseGraph();

                        // Add all the triples to the graph that describe the owl file directly (version iri, imports...)
                        foreach (Triple triple in graph.Triples.WithSubject(graph.GetUriNode(new Uri(baseUri))))
                            modelBaseGraph.addTriple(triple);

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

            // Now all elements are instanciated and received their describing triples.
            // Triples that do not point to other elements have already been parsed (i.e. hasModelComponentID, hasComment ...)

            if (passProcessModells.Count == 0)
            {
                IPASSProcessModel passProcessModell = new PASSProcessModel(graph.BaseUri.ToString());
                passProcessModell.createUniqueModelComponentID("defaultModelID");
                passProcessModells.Add(passProcessModell);
            }

            // To parse all triples that link to other elements, the model is involved
            // The model is parsed top down; Every element that needs the reference to another can ask the model
            // by passing the suffix of the uri of the required element (which is also the ModelComponentID).
            foreach (IPASSProcessModel model in passProcessModells)
            {
                if (model is IParseablePASSProcessModelElement parseable)
                    parseable.completeObject(ref createdElements);
            }

            // ? Was tun mit elementen die in keinem Model sind? Wie prüfen ?

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

        public void setModelElementFactory(IPASSProcessModelElementFactory<IParseablePASSProcessModelElement> elementFactory)
        {
            if (elementFactory is null) return;
            this.elementFactory = elementFactory;
        }
    }
}
