using System;
using System.Collections.Generic;
using System.Text;
using VDS.RDF;

namespace alps.net.api.util
{
    public class NodeHelper
    {
        public static string getNodeContent(INode node)
        {
            if (node is ILiteralNode literal) return literal.Value;
            return node.ToString();
        }

        public static string getLangIfContained(INode node)
        {
            if (node is ILiteralNode literal) return literal.Language;
            return "";
        }

        public static string getDataTypeIfContained(INode node)
        {
            if (node is ILiteralNode literal && literal.DataType != null) return literal.DataType.ToString();
            return "";
        }

        public static string cutURI(string uri)
        {
            string firstCut = uri.Split("#")[uri.Split("#").Length - 1];
            return firstCut.Split(":")[firstCut.Split(":").Length - 1];
        }
    }
}
