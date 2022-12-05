using alps.net.api.util;
using System;
using System.Collections.Generic;
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
        public interface IGraphCallback
        {
            void notifyTriple(Triple triple);

            string getSubjectName();

            void notifyModelComponentIDChanged(string oldID, string newID);
        }

        public const string EXAMPLE_BASE_URI_PLACEHOLDER = "baseuri:";
        public const string EXAMPLE_BASE_URI_PLACEHOLDER_MAPPING_KEY = "baseuri";

        private ICompatibilityDictionary<string, IGraphCallback> elements = new CompatibilityDictionary<string, IGraphCallback>();

        private readonly ICompatibilityDictionary<string, string> namespaceMappings = new CompatibilityDictionary<string, string>{
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

        public bool containsNonBaseUri(string input)
        {
            foreach (KeyValuePair<string, string> nameMapping in namespaceMappings)
            {
                if (input.Contains(nameMapping.Value) && !nameMapping.Key.Equals(EXAMPLE_BASE_URI_PLACEHOLDER.Replace(":", "")))
                    return true;
            }
            return false;
        }

        protected const string EXAMPLE_BASE_URI = "http://www.imi.kit.edu/exampleBaseURI";

        protected IGraph baseGraph;

        public PASSGraph(string baseURI)
        {
            if (baseURI is null)
                this.baseURI = EXAMPLE_BASE_URI;
            else
                this.baseURI = baseURI;
            namespaceMappings.Add(EXAMPLE_BASE_URI_PLACEHOLDER_MAPPING_KEY, baseURI + "#");

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

        public void changeBaseURI(string newUri)
        {
            if (newUri is null)
                this.baseURI = EXAMPLE_BASE_URI;
            else
                this.baseURI = newUri;

            namespaceMappings[EXAMPLE_BASE_URI_PLACEHOLDER_MAPPING_KEY] = baseURI + "#";
            // baseGraph.NamespaceMap.RemoveNamespace("");
            // baseGraph.NamespaceMap.RemoveNamespace(EXAMPLE_BASE_URI_PLACEHOLDER_MAPPING_KEY);
            baseGraph.NamespaceMap.AddNamespace("", new Uri(baseURI + "#"));
            baseGraph.NamespaceMap.AddNamespace(EXAMPLE_BASE_URI_PLACEHOLDER_MAPPING_KEY, new Uri(baseURI + "#"));
            //exportGraph.NamespaceMap.AddNamespace("", new Uri(baseURI + "#"));
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
                elements[subjWithoutUri].notifyTriple(t);
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


        public void register(IGraphCallback element)
        {
            elements.TryAdd(element.getSubjectName(), element);
        }

        public void unregister(IGraphCallback element)
        {
            elements.Remove(element.getSubjectName());
        }

        public void modelComponentIDChanged(string oldID, string newID)
        {
            IList<IGraphCallback> elementsToNotify = new List<IGraphCallback>();
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
            foreach (IGraphCallback parseable in elementsToNotify)
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

    }
}
