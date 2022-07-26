using alps.net.api.parsing;
using System;
using VDS.RDF;

namespace alps.net.api.util
{
    public class IncompleteTriple : IIncompleteTriple
    {
        private string predicateContent;
        private IStringWithExtra extraString;

        public enum LiteralType
        {
            DATATYPE,
            LANGUAGE
        }

        public IncompleteTriple(string predicate, string objectContent)
        {
            predicateContent = predicate;
            this.extraString = new StringWithoutExtra(objectContent);
        }

        public IncompleteTriple(Triple realTriple, string baseUriToReplace = null)
        {
            predicateContent = (baseUriToReplace == null) ? realTriple.Predicate.ToString() : StaticFunctions.replaceBaseUriWithGeneric(realTriple.Predicate.ToString(), baseUriToReplace);
            if (realTriple.Object is ILiteralNode literal)
            {
                if (literal.Language != null && !literal.Language.Equals(""))
                    extraString = new LanguageSpecificString(literal.Value, literal.Language);
                else if (literal.DataType != null && !literal.DataType.ToString().Equals(""))
                    extraString = new DataTypeString(literal.Value, literal.DataType.ToString());
                else
                {
                    extraString = new StringWithoutExtra(StaticFunctions.replaceBaseUriWithGeneric(realTriple.Object.ToString(), baseUriToReplace));
                }
            }
            else
                extraString = new StringWithoutExtra(StaticFunctions.replaceBaseUriWithGeneric(realTriple.Object.ToString(), baseUriToReplace));
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
            INode objectNode = extraString.getNodeFromString(graph);

            return new Triple(subjectNode, predicateNode, objectNode);
        }

        public string getPredicate()
        { return predicateContent; }
        public string getObject()
        {
            return extraString.getContent();
        }

        public override bool Equals(object otherObject)
        {
            int matches = 0;
            if (otherObject is IncompleteTriple triple)
            {
                if ((triple.getPredicate() != null && getPredicate() != null && triple.getPredicate().Equals(getPredicate())) || (triple.getPredicate() is null && getPredicate() is null))
                    matches++;
                if ((triple.getObjectWithExtra() != null && getObjectWithExtra() != null && triple.getObjectWithExtra().Equals(getObjectWithExtra())) || (triple.getObjectWithExtra() is null && getObjectWithExtra() is null))
                    matches++;
            }
            if (matches == 2) return true;
            return false;
        }

        public IStringWithExtra getObjectWithExtra()
        {
            return extraString.clone();
        }

        public override int GetHashCode()
        {
            string baseString = "";
            if (getPredicate() != null)
                baseString += getPredicate();
            if (getObjectWithExtra() != null)
                baseString += getObjectWithExtra();
            return baseString.GetHashCode();
        }
    }
}