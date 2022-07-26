using alps.net.api.parsing;

namespace alps.net.api.util
{
    public class StaticFunctions
    {
        public const char BASE_URI_SEPARATOR = '#';

        /// <summary>
        /// Replaces a base uri contained in a string with a generic placeholder.
        /// Example: http://www.imi.kit.edu/exampleBaseURI#MyModel1 will return baseuri:MyModel1.
        /// The baseuri name mapping is defined in the triple store graph.
        /// This way, the base uri can be changed without changing all triples.
        /// </summary>
        /// <param name="input">The string that MAY contain a base uri</param>
        /// <returns>the string without specific base uri</returns>
        public static string replaceBaseUriWithGeneric(string input, string baseuri)
        {
            string output = input;

            // If a base uri is contained, replace it with generic one
            if (output.Contains(baseuri + BASE_URI_SEPARATOR.ToString()))
            {
                output = output.Replace(baseuri + BASE_URI_SEPARATOR.ToString(), PASSGraph.EXAMPLE_BASE_URI_PLACEHOLDER);
            }

            // If a base uri is contained, replace it with generic one
            if (output.Equals(baseuri))
            {
                output = baseuri;
            }
            return output;
        }


        /// <summary>
        /// Replaces a base uri contained in a string with a generic placeholder.
        /// Example: http://www.imi.kit.edu/exampleBaseURI#MyModel1 will return baseuri:MyModel1.
        /// The baseuri name mapping is defined in the triple store graph.
        /// This way, the base uri can be changed without changing all triples.
        /// </summary>
        /// <param name="input">The string that MAY contain a base uri</param>
        /// <returns>the string without specific base uri</returns>
        public static string removeBaseUri(string input, string baseuri)
        {
            string output = input;

            if (baseuri != null)
            {
                // If a base uri is contained, replace it with generic one
                if (output.Contains(baseuri + BASE_URI_SEPARATOR.ToString()))
                {
                    output = output.Replace(baseuri + BASE_URI_SEPARATOR.ToString(), "");
                }

                // If a base uri is contained, replace it with generic one
                if (output.Contains(baseuri))
                {
                    output = output.Replace(baseuri, "");
                }
            }

            // If a base uri is contained, replace it with generic one
            if (output.Contains(PASSGraph.EXAMPLE_BASE_URI_PLACEHOLDER))
            {
                output = output.Replace(PASSGraph.EXAMPLE_BASE_URI_PLACEHOLDER, "");
            }

            return output;
        }

        public static string addGenericBaseURI(string input)
        {
            string output = input;

            if (!output.StartsWith(PASSGraph.EXAMPLE_BASE_URI_PLACEHOLDER))
                output = PASSGraph.EXAMPLE_BASE_URI_PLACEHOLDER + output;

            return output;
        }
    }
}
