using alps.net.api.StandardPASS;
using alps.net.api.StandardPASS.BehaviorDescribingComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Ontology;
using VDS.RDF.Writing;

namespace alps.net.api.parsing
{
    /// <summary>
    /// This class is an adapter class for the <see cref="IGraph"/> interface.
    /// It uses an <see cref="OntologyGraph"/> as internal graph.
    /// </summary>
    public class PASSGraph : IPASSGraph
    {
        private readonly IDictionary<string, string> namespaceMappings = new Dictionary<string, string>{
            { "rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"},
            { "rdfs", "http://www.w3.org/2000/01/rdf-schema#"},
            { "xml", "http://www.w3.org/XML/1998/namespace"},
            { "xsd", "http://www.w3.org/2001/XMLSchema#"},
            { "swrla", "http://swrl.stanford.edu/ontologies/3.3/swrla.owl#"},
            { "abstract-pass-ont", "http://www.imi.kit.edu/abstract-pass-ont#"},
            { "standard-pass-ont", "http://www.i2pm.net/standard-pass-ont#"},
            { "owl", "http://www.w3.org/2002/07/owl#"},
             { "", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"}
        };
        private string baseURI;

        protected const string EXAMPLE_BASE_URI = "http://www.imi.kit.edu/exampleBaseURI";

        protected IGraph baseGraph;

        public PASSGraph(string baseURI)
        {
            if (baseURI is null)
                this.baseURI = EXAMPLE_BASE_URI;
            else
                this.baseURI = baseURI;
            namespaceMappings.Add(IPASSGraph.EXAMPLE_BASE_URI_PLACEHOLDER.Replace(":", ""), baseURI + "#");

            OntologyGraph exportGraph = new OntologyGraph();

            // Adding all namespaceMappings (exchange short acronyms like owl: with the complete uri)
            foreach (KeyValuePair<string, string> nameMapping in namespaceMappings)
            {
                exportGraph.NamespaceMap.AddNamespace(nameMapping.Key, new Uri(nameMapping.Value));
            }
            exportGraph.NamespaceMap.AddNamespace("", new Uri(baseURI + "#"));
            exportGraph.BaseUri = new Uri(baseURI);

            INode subjectNode;
            INode predicateNode;
            INode objectNode;
            Triple triple;


            subjectNode = exportGraph.CreateUriNode(exportGraph.BaseUri);
            predicateNode = exportGraph.CreateUriNode("rdf:type");
            objectNode = exportGraph.CreateUriNode("owl:Ontology");
            triple = new Triple(subjectNode, predicateNode, objectNode);
            exportGraph.Assert(triple);


            // Adding import triples for standard pass and abstract pass
            subjectNode = exportGraph.CreateUriNode(exportGraph.BaseUri);
            predicateNode = exportGraph.CreateUriNode("owl:imports");

            objectNode = exportGraph.CreateUriNode(new Uri("http://www.i2pm.net/standard-pass-ont"));
            triple = new Triple(subjectNode, predicateNode, objectNode);
            exportGraph.Assert(triple);
            objectNode = exportGraph.CreateUriNode(new Uri("http://www.imi.kit.edu/abstract-pass-ont"));
            triple = new Triple(subjectNode, predicateNode, objectNode);
            exportGraph.Assert(triple);

            baseGraph = exportGraph;
        }

        public IGraph getGraph()
        {
            return baseGraph;
        }

        public void addTriple(Triple t)
        {
            if (baseGraph.Triples.Contains(t)) return;
            baseGraph.Assert(t);
            string subjWithoutUri = t.Subject.ToString().Replace(baseURI + "#", "");
            if (elements.ContainsKey(subjWithoutUri))
            {
                elements[subjWithoutUri].addTriple(t);
            }
        }

        public IUriNode createUriNode()
        {
            return baseGraph.CreateUriNode();
        }
        public IUriNode createUriNode(Uri uri)
        {
            return baseGraph.CreateUriNode(uri);
        }
        public IUriNode createUriNode(string qname)
        {
            return baseGraph.CreateUriNode(qname);
        }

        public ILiteralNode createLiteralNode(string literal)
        {
            return baseGraph.CreateLiteralNode(literal);
        }
        public ILiteralNode createLiteralNode(string literal, Uri datadef)
        {
            return baseGraph.CreateLiteralNode(literal, datadef);
        }
        public ILiteralNode createLiteralNode(string literal, string langspec)
        {
            return baseGraph.CreateLiteralNode(literal, langspec);
        }

        public void removeTriple(Triple t) { baseGraph.Retract(t); }


        public void register(IParseablePASSProcessModelElement element)
        {
            elements.TryAdd(element.getModelComponentID(), element);
        }

        public void unregister(IParseablePASSProcessModelElement element)
        {
            elements.Remove(element.getModelComponentID());
        }

        public void modelComponentIDChanged(string oldID, string newID)
        {
            IList<IParseablePASSProcessModelElement> elementsToNotify = new List<IParseablePASSProcessModelElement>();
            foreach (Triple t in baseGraph.Triples)
            {
                if (t.ToString().Contains(oldID))
                {
                    string subjWithoutUri = t.Subject.ToString().Replace(baseURI + "#", "");
                    if (elements.ContainsKey(subjWithoutUri))
                    {
                        elementsToNotify.Add(elements[subjWithoutUri]);
                    }
                }
            }
            foreach (IParseablePASSProcessModelElement parseable in elementsToNotify)
            {
                parseable.notifyModelComponentIDChanged(oldID, newID);
            }
        }

        public void exportTo(string filepath)
        {
            IRdfWriter writer = new RdfXmlWriter();
            string fullPath = (filepath.EndsWith(".owl")) ? filepath : filepath + ".owl";
            writer.Save(baseGraph, fullPath);
        }

        public string getBaseURI()
        {
            return baseURI;
        }

        private IDictionary<string, IParseablePASSProcessModelElement> elements = new Dictionary<string, IParseablePASSProcessModelElement>();

        

    }
}
