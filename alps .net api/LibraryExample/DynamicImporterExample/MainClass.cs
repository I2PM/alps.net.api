using alps.net.api;
using alps.net.api.parsing;
using System.Collections.Generic;
using alps.net.api.StandardPASS;
using System.Linq;
using System;
using alps.net.api.ALPS;
using System.Diagnostics;
using AngleSharp.Common;

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
               //"C:\\Data\\Dropbox\\SO Material\\Ontology Daten Format\\standard_PASS_ont_v_1.1.0.owl",
               //"C:\\Data\\Dropbox\\SO Material\\Ontology Daten Format\\abstract-layered-pass-ont.owl",
            };

            // Load these files once (no future calls needed)
            // This call creates both parsing trees and the parsing dictionary
            io.loadOWLParsingStructure(paths);

            // This loads models from the specified owl.
            // Every owl instance of a FullySpecifiedSubject is parsed to an AdditionalFunctionalityFullySpecifiedSubject
            IList<IPASSProcessModel> models = io.loadModels(new List<string> { "C:\\Data\\ExportImportTest1.owl" });

            // IDictionary of all elements
            IDictionary<string, IPASSProcessModelElement> allElements = models[0].getAllElements();
            // Drop the keys, keep values
            ICollection<IPASSProcessModelElement> onlyElements = models[0].getAllElements().Values;
            // Filter for a specific interface (Enumerable, not so easy to use -> convert to list)
            IList<IAdditionalFunctionalityElement> onlyAdditionalFunctionalityElements = models[0].getAllElements().Values.OfType<IAdditionalFunctionalityElement>().ToList();


            //some output examples for a parsed model
            Console.WriteLine("Number ob Models loaded: " + models.Count);
            Console.WriteLine("Found " + onlyAdditionalFunctionalityElements.Count +
                              " AdditionalFunctionalityElements in First model!");

            IDictionary<string, IModelLayer> layers = models[0].getModelLayers();
            Console.WriteLine("Layers in first model: " + layers.Count);

            IModelLayer firstLayer = layers.ElementAt(0).Value;
   
            IFullySpecifiedSubject mySubject = firstLayer.getFullySpecifiedSubject(0);
            IDictionary<string, ISubjectBehavior> mySubjectBehaviors =  mySubject.getBehaviors();
            Console.WriteLine("Numbers of behaviors: " + mySubjectBehaviors.Count);

            ISubjectBehavior firstBehavior = mySubjectBehaviors.ElementAt(0).Value;
            Console.WriteLine("Numbers of Elements in Behavior: " + firstBehavior.getBehaviorDescribingComponents().Count);
            Console.WriteLine("First Element: " + firstBehavior.getBehaviorDescribingComponents().ElementAt(0).Value.getModelComponentID());
            IState firstState = firstBehavior.getInitialStateOfBehavior();

            iterateStates(firstBehavior);
        }

        private static void iterateStates(ISubjectBehavior someBehavior)
        {
            Console.WriteLine("State Stats");
            
            foreach (KeyValuePair<string, IBehaviorDescribingComponent> kvp in someBehavior.getBehaviorDescribingComponents())
            {
                IPASSProcessModelElement myComponent = kvp.Value;
                if (myComponent is IState)
                {
                    Console.Write("state: " + myComponent.getModelComponentID());

                    IState myIstate = (IState)myComponent;

                    Console.Write(" - start: " + myIstate.isStateType(IState.StateType.InitialStateOfBehavior));
                    Console.WriteLine(" - end: " + myIstate.isStateType(IState.StateType.EndState));
                }
            }
        }
    }

    
}
