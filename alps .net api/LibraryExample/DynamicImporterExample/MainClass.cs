using alps.net.api;
using alps.net.api.parsing;
using System.Collections.Generic;
using alps.net.api.StandardPASS;
using System.Linq;
using System;

namespace LibraryExample.DynamicImporterExample
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            // Needs to be called once
            // Now the reflective enumerator searches for classes in the library assembly as well as in the current.
            ReflectiveEnumerator.addAssemblyToCheckForTypes(System.Reflection.Assembly.GetExecutingAssembly());

            IPASSReaderWriter io = PASSReaderWriter.getInstance();

            // Set own factory as parsing factory to parse ontology classes to the right instances
            io.setModelElementFactory(new AdditionalFunctionalityClassFactory());

            IList<string> paths = new List<string>
            {
                "../../../../../src/standard_PASS_ont_v_1.1.0.owl",
                "../../../../../src/abstract-layered-pass-ont.owl",
            };

            // Load these files once (no future calls needed)
            // This call creates both parsing trees and the parsing dictionary
            io.loadOWLParsingStructure(paths);

            // This loads models from the specified owl.
            // Every owl instance of a FullySpecifiedSubject is parsed to an AdditionalFunctionalityFullySpecifiedSubject
            IList<IPASSProcessModel> models = io.loadModels(new List<string> { "../../../../../src/StateExtensionTest.owl" });

            // IDictionary of all elements
            IDictionary<string, IPASSProcessModelElement> allElements = models[0].getAllElements();
            // Drop the keys, keep values
            ICollection<IPASSProcessModelElement> onlyElements = models[0].getAllElements().Values;
            // Filter for a specific interface (Enumerable, not so easy to use -> convert to list)
            IList<IAdditionalFunctionalityElement> onlyAdditionalFunctionalityElements = models[0].getAllElements().Values.OfType<IAdditionalFunctionalityElement>().ToList();

            Console.WriteLine("Found " + onlyAdditionalFunctionalityElements.Count +
                              " AdditionalFunctionalityElements!");
            

        }
    }
}
