using alps.net.api.parsing;
using System;
using System.Collections.Generic;
using System.Text;
using VDS.RDF;

namespace alps.net.api.util
{
    public class IncompleteTriple : IIncompleteTriple
    {
        private string predicateContent, objectContent;
        private IStringWithExtra extraString;

        public enum LiteralType
        {
            DATATYPE,
            LANGUAGE
        }

        public IncompleteTriple(string predicate, string objectContent)
        {
            predicateContent = predicate;
            this.objectContent = objectContent;
        }

        public IncompleteTriple(Triple realTriple)
        {
            predicateContent = realTriple.Predicate.ToString();
            if (realTriple.Object is ILiteralNode literal)
            {
                if (literal.Language != null && !literal.Language.Equals(""))
                    extraString = new LanguageSpecificString(literal.Value, literal.Language);
                else if (literal.DataType != null && !literal.DataType.ToString().Equals(""))
                    extraString = new LanguageSpecificString(literal.Value, literal.DataType.ToString());
                else
                    this.objectContent = realTriple.Object.ToString();
            }
            else
                this.objectContent = realTriple.Object.ToString();
        }

        public IncompleteTriple(string predicate, string objectContent, LiteralType literalType, string objectAddition)
        {
            predicateContent = predicate;
            if (literalType == LiteralType.DATATYPE)
            {
                extraString = new DataTypeString(objectContent, objectAddition);
            }
            else if (literalType == LiteralType.LANGUAGE) { extraString = new LanguageSpecificString(objectContent, objectAddition); }
        }
        public IncompleteTriple(string predicate, IStringWithExtra objectWithExtra)
        {
            predicateContent = predicate;
            this.extraString = objectWithExtra;
        }

        public Triple getRealTriple(IPASSGraph graph, INode subjectNode)
        {
            INode predicateNode;
            try
            {
                predicateNode = graph.createUriNode(predicateContent);
            }
            catch (RdfException)
            {
                predicateNode = graph.createUriNode(new Uri(predicateContent));
            }
            INode objectNode;
            if (extraString is null && objectContent != null)
            {
                if (!objectContent.Contains("http://") && !objectContent.Contains("https://"))
                try
                {
                    objectNode = graph.createUriNode(objectContent);
                }
                catch (RdfException)
                {
                    objectNode = graph.createUriNode(new Uri(objectContent));
                }
                else objectNode = graph.createUriNode(new Uri(objectContent));
            }

            else if (extraString != null)
            {
                objectNode = extraString.getNodeFromString(graph);
            }
            else return null;
            return new Triple(subjectNode, predicateNode, objectNode);
        }

        public string getPredicate()
        { return predicateContent; }
        public string getObject()
        {
            if (objectContent != null)
            {
                return objectContent;
            }
            else { return extraString.getContent(); }
        }

        public override bool Equals(object otherObject)
        {
            int matches = 0;
            if (otherObject is IncompleteTriple triple)
            {
                if ((triple.getPredicate() != null && getPredicate() != null && triple.getPredicate().Equals(getPredicate())) || (triple.getPredicate() is null && getPredicate() is null))
                    matches++;
                if ((triple.getObject() != null && getObject() != null && triple.getObject().Equals(getObject())) || (triple.getObject() is null && getObject() is null))
                    matches++;
                if ((triple.getObjectWithExtra() != null && getObjectWithExtra() != null && triple.getObjectWithExtra().Equals(getObjectWithExtra())) || (triple.getObjectWithExtra() is null && getObjectWithExtra() is null))
                    matches++;
            }
            if (matches == 3) return true;
            return false;
        }

        public IStringWithExtra getObjectWithExtra()
        {
            return (extraString is null) ? null : extraString.clone();
        }

        public override int GetHashCode()
        {
            string baseString = "";
            if (getPredicate() != null)
                baseString += getPredicate();
            if (getObject() != null)
                baseString += getObject();
            if (getObjectWithExtra() != null)
                baseString += getObjectWithExtra();
            return baseString.GetHashCode();
        }
    }
}