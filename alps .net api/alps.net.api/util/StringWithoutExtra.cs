﻿using alps.net.api.parsing;
using System;
using VDS.RDF;

namespace alps.net.api.util
{
    public class StringWithoutExtra : StringWithExtra
    {

        public StringWithoutExtra(string content) : base(content)
        {
            // No need for calls here, base calls our overwriten setContent
        }

        public StringWithoutExtra(string content, string lang) : base(content)
        {
            // No need for calls here, base calls our overwriten setContent
        }

        public override IStringWithExtra clone()
        {
            return new StringWithoutExtra(getContent());
        }

        public override INode getNodeFromString(IPASSGraph graph)

        {
            INode objectNode;
            if (!content.Contains("http://") && !content.Contains("https://"))
                try
                {
                    objectNode = graph.createUriNode(content);
                }
                catch (RdfException)
                {
                    objectNode = graph.createUriNode(new Uri(content));
                }
            else objectNode = graph.createUriNode(new Uri(content));
            return objectNode;
        }


        public override void setExtra(string lang)
        {
            return;
        }

        public override string ToString()
        {
            return content;
        }
    }
}
