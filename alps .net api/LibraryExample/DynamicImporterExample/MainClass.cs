﻿using alps.net.api;
using alps.net.api.parsing;
using System.Collections.Generic;
using alps.net.api.StandardPASS;
using System.Linq;
using System;
using alps.net.api.ALPS;
using alps.net.api.StandardPASS.PassProcessModelElements.DataDescribingComponents;
using alps.net.api.ALPS.ALPSModelElements.ALPSSIDComponents;

namespace LibraryExample.DynamicImporterExample
{
    public class MainClass
    {
        public static void Main(string[] args)
        {

            //Neo4JGraph graph = new Neo4JGraph("neo4j://imi-intwertl-pass.imi.kit.edu:7688", "neo4j", "neo4pass");
            //Console.WriteLine("Trying to add triple");
            //return;

            IPASSProcessModel model = new PASSProcessModel("http://example-baseuri.de");
            IFullySpecifiedSubject subj = new FullySpecifiedSubject(model.getBaseLayer());
            IState state = new State(subj.getBehaviors().Values.First());
            IAction action = state.getAction();
            action.setModelComponentID("NeueID");

            IPASSReaderWriter io = PASSReaderWriter.getInstance();

            // Needs to be called once
            // Now the reflective enumerator searches for classes in the library assembly as well as in the current.
            io.addAssemblyToCheckForTypes(System.Reflection.Assembly.GetExecutingAssembly());

            // Set own factory as parsing factory to parse ontology classes to the right instances
            io.setModelElementFactory(new AdditionalFunctionalityClassFactory());

            IList<string> paths = new List<string>
            {
               "../../../../../src/standard_PASS_ont_v_1.1.0.owl",
               //"../../../../../src/abstract-layered-pass-ont.owl",
               "../../../../../src/ALPS_ont_v_0.8.0.owl",
               
               //"C:\\Data\\Dropbox\\SO Material\\Ontology Daten Format\\standard_PASS_ont_v_1.1.0.owl",
               //"C:\\Data\\Dropbox\\SO Material\\Ontology Daten Format\\abstract-layered-pass-ont.owl",
            };

            // Load these files once (no future calls needed)
            // This call creates both parsing trees and the parsing dictionary
            io.loadOWLParsingStructure(paths);

            var graphFactory = new Neo4JGraphFactory("neo4j://imi-intwertl-pass.imi.kit.edu:7688", "neo4j", "neo4pass");

            // This loads models from the specified owl.
            // Every owl instance of a FullySpecifiedSubject is parsed to an AdditionalFunctionalityFullySpecifiedSubject
            //IList<IPASSProcessModel> models = io.loadModels(new List<string> { "C:\\Data\\ExportImportTest1.owl" });
            //IList<IPASSProcessModel> models = io.loadModels(new List<string> { "C:\\Data\\ExportImportTest1WOLayers.owl" });
            IList<IPASSProcessModel> models = io.loadModels(new List<string> { "C:\\Users\\lukas\\Documents\\IMI\\ServerInfrastructure.owl" }, modelGraphFactory: null);


            // IDictionary of all elements
            IDictionary<string, IPASSProcessModelElement> allElements = models[0].getAllElements();
            Console.WriteLine(models[0]);
            // Drop the keys, keep values
            ICollection<IPASSProcessModelElement> onlyElements = models[0].getAllElements().Values;
            // Filter for a specific interface (Enumerable, not so easy to use -> convert to list)
            IList<IAdditionalFunctionalityElement> onlyAdditionalFunctionalityElements = models[0].getAllElements().Values.OfType<IAdditionalFunctionalityElement>().ToList();


            //some output examples for a parsed model
            Console.WriteLine("Number ob Models loaded: " + models.Count);
            Console.WriteLine("Found " + onlyAdditionalFunctionalityElements.Count +
                              " AdditionalFunctionalityElements in First model!");

            Console.WriteLine();
            IFullySpecifiedSubject sub2 = models[0].getAllElements().Values.OfType<IFullySpecifiedSubject>().Where(x => x.getModelComponentID().Contains("SID_1_FullySpecifiedSubject_2")).First();
            sub2.removeModelComponentLabel(new LanguageSpecificString("Subject 2@en"));
            sub2.addModelComponentLabel("Peter@de");

            IDictionary<string, IModelLayer> layers = models[0].getModelLayers();
            Console.WriteLine("Layers in first model: " + layers.Count);

            foreach (KeyValuePair<string, IModelLayer> kvp in layers)
            {
                Console.WriteLine(" - Layer ID: " + kvp.Value.getModelComponentID() + " type: " + kvp.Value.getLayerType());
            }

            IModelLayer firstLayer = layers.ElementAt(0).Value;

            IStandaloneMacroSubject sams = iterateTrhoughSIDandGetStandaloneMacroSubjectFromA(firstLayer);

            if (sams != null)
            {
                ISubjectBehavior samsB = sams.getBehavior();
                if (samsB != null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("##### macro behavior! components: ######");
                    lookForChoiceSegementStuffIn(samsB);
                }
            }


            IFullySpecifiedSubject mySubject = firstLayer.getFullySpecifiedSubject(0);
            if (mySubject != null)
            {

                IDictionary<string, ISubjectBehavior> mySubjectBehaviors = mySubject.getBehaviors();
                Console.WriteLine();
                Console.WriteLine("Found a subject: " + mySubject.getModelComponentID());
                Console.WriteLine("Numbers of behaviors in subject: " + mySubjectBehaviors.Count);

                ISubjectBehavior firstBehavior = mySubjectBehaviors.ElementAt(0).Value;
                Console.WriteLine("Numbers of Elements in Behavior: " + firstBehavior.getBehaviorDescribingComponents().Count);
                Console.WriteLine("First Element: " + firstBehavior.getBehaviorDescribingComponents().ElementAt(0).Value.getModelComponentID());
                IState firstState = firstBehavior.getInitialStateOfBehavior();
                if (firstState != null)
                {
                    Console.WriteLine("Initial State of Behavior: " + firstState.getModelComponentID());
                }



                iterateStates(firstBehavior);
                Console.WriteLine();
                iterateTransitions(firstBehavior);
                Console.WriteLine();

            }
        }


        private static IStandaloneMacroSubject iterateTrhoughSIDandGetStandaloneMacroSubjectFromA(IModelLayer layer)
        {
            IStandaloneMacroSubject result = null;
            Console.WriteLine("SID Elements - nr. of elements: " + layer.getElements().Count);
            foreach (KeyValuePair<string, IPASSProcessModelElement> kvp in layer.getElements())
            {
                IPASSProcessModelElement myComponent = kvp.Value;

                if (myComponent is ISubject mySub)
                {
                    Console.WriteLine(" Subject: " + myComponent.getModelComponentID());

                    if (myComponent is IStandaloneMacroSubject isms)
                    {
                        result = isms;
                    }
                    else if (myComponent is ISystemInterfaceSubject)
                    {
                        Console.WriteLine(" SystemInterfacesubject: " + myComponent.getModelComponentID());
                        ISystemInterfaceSubject mySIS = (ISystemInterfaceSubject)myComponent;
                        Console.WriteLine(" - contained interfaces: " + mySIS.getContainedInterfaceSubjects().Count());
                    }
                    else if (myComponent is ISubjectGroup)
                    {
                        Console.WriteLine(" Subject Group: " + myComponent.getModelComponentID());
                        ISubjectGroup mySIS = (ISubjectGroup)myComponent;
                        Console.WriteLine(" - contained Subjects: " + mySIS.getContainedSubjects().Count());
                    }
                    else if (myComponent is IInterfaceSubject iis)
                    {
                        Console.WriteLine("  Interface Subject: ");
                        Console.WriteLine("  - interface subject sisimapping exists: " + (iis.getSimpleSimInterfaceSubjectResponseDefinition() != null));
                        if (iis.getSimpleSimInterfaceSubjectResponseDefinition() != null)
                        {
                            Console.WriteLine(iis.getSimpleSimInterfaceSubjectResponseDefinition().OuterXml);
                        }
                    }

                    Console.WriteLine(" - 2d page ratio: " + mySub.get2DPageRatio());
                    Console.WriteLine(" - 2d width: " + mySub.getRelative2DWidth());
                    Console.WriteLine(" - 2d height: " + mySub.getRelative2DHeight());
                    Console.WriteLine(" - 2d posX: " + mySub.getRelative2DPosX());
                    Console.WriteLine(" - 2d posY: " + mySub.getRelative2DPosY());

                }
                else if (myComponent is IMessageExchange ime)
                {
                    Console.WriteLine(" MessageExchange: " + ime.getModelComponentID());
                    Console.WriteLine("  - Exchange Type: " + ime.getMessageExchangeType());

                }
                else if (myComponent is IMessageExchangeList imel)
                {
                    Console.WriteLine(" MessageExchangeList: " + imel.getModelComponentID());
                    Console.WriteLine(" - Number of Pathpoints: " + imel.getSimple2DPathPoints().Count());
                    Console.WriteLine(" - Number of Messages on here: " + imel.getMessageExchanges().Count);
                }
                else if (myComponent is ICommunicationChannel ame)
                {
                    Console.WriteLine(" Channel: " + ame.getModelComponentID());
                    Console.WriteLine(" - Number of Pathpoints: " + ame.getSimple2DPathPoints().Count());
                }

                else
                {
                    Console.WriteLine(" #other component: " + myComponent.getModelComponentID());
                }

            }
            return result;
        }

        private static void lookForChoiceSegementStuffIn(ISubjectBehavior someBehavior)
        {
            Console.WriteLine("States of Behavior: " + someBehavior.getModelComponentID());

            foreach (KeyValuePair<string, IBehaviorDescribingComponent> kvp in someBehavior.getBehaviorDescribingComponents())
            {
                IPASSProcessModelElement myComponent = kvp.Value;
                if (myComponent is IState myState)
                {
                    Console.WriteLine(" state: " + myState.getModelComponentID());
                    if (myState is IChoiceSegment mcs)
                    {
                        Console.WriteLine(" Number of CS-Paths: " + mcs.getChoiceSegmentPaths().Count);
                        if (mcs.getChoiceSegmentPaths().Count >= 1)
                        {
                            IChoiceSegmentPath mcsp = mcs.getChoiceSegmentPaths().ElementAt(0).Value;
                            Console.WriteLine(" - first path ID: " + mcsp.getModelComponentID());
                            Console.WriteLine(" - fist element of first path: " + mcsp.getInitialState().getModelComponentID());
                            Console.WriteLine(" - mandatory - start: " + mcsp.getIsOptionalToStartChoiceSegmentPath() + " - end: " + mcsp.getIsOptionalToEndChoiceSegmentPath());

                        }
                    }
                }
            }
        }
        private static void iterateStates(ISubjectBehavior someBehavior)
        {
            Console.WriteLine("States of Behavior: " + someBehavior.getModelComponentID());

            foreach (KeyValuePair<string, IBehaviorDescribingComponent> kvp in someBehavior.getBehaviorDescribingComponents())
            {
                IPASSProcessModelElement myComponent = kvp.Value;
                if (myComponent is IState)
                {
                    Console.Write("state: " + myComponent.getModelComponentID());

                    IState myIstate = (IState)myComponent;

                    Console.Write(" - start: " + myIstate.isStateType(IState.StateType.InitialStateOfBehavior));
                    Console.WriteLine(" - end: " + myIstate.isStateType(IState.StateType.EndState));
                    if (myIstate is IDoState myDo)
                    {
                        iterateThroughStateAttributes(myDo);

                    }
                    else if (myIstate is IReceiveState myR)
                    {
                        Console.WriteLine(" - receive billed waiting time: " + myR.getSisiBilledWaitingTime());
                    }


                }
            }
        }

        private static void iterateTransitions(ISubjectBehavior someBehavior)
        {

            Console.WriteLine("Transitions: ###########################");
            foreach (KeyValuePair<string, IBehaviorDescribingComponent> kvp in someBehavior.getBehaviorDescribingComponents())
            {
                IPASSProcessModelElement myComponent = kvp.Value;
                if (myComponent is ITransition myTrans)
                {
                    Console.WriteLine("transition: " + myTrans.getModelComponentID());
                    Console.Write(" - start: " + myTrans.getSourceState().getModelComponentID());
                    Console.WriteLine(" - end: " + myTrans.getTargetState().getModelComponentID());
                    Console.WriteLine(" - type: " + myTrans.getTransitionType());


                    Console.WriteLine(" - Number of Pathpoints: " + myTrans.getSimple2DPathPoints().Count);

                    if (myTrans is ISendTransition myST)
                    {
                        Console.WriteLine(" - Send Transition: ");
                        ISendTransitionCondition mySTC = myST.getTransitionCondition();
                        Console.Write("  - tranition condition - message " + mySTC.getRequiresSendingOfMessage().getModelComponentID());
                        Console.WriteLine("  - receiver: " + mySTC.getRequiresMessageSentTo().getModelComponentID());
                    }
                    else if (myTrans is IReceiveTransition myRT)
                    {
                        Console.WriteLine(" - Receive Transition: ");
                        Console.WriteLine(" - priority number of ReceiveTransition " + myRT.getPriorityNumber());

                    }
                    else if (myTrans is IDoTransition myDT)
                    {
                        Console.WriteLine(" - Do Transition: ");
                        Console.WriteLine(" - priority number of Do Transition " + myDT.getPriorityNumber());

                    }
                    else if (myTrans is IUserCancelTransition)
                    {
                        Console.WriteLine(" - IUserCancelTransition Transition: ");
                    }
                    else
                    {
                        Console.WriteLine(" - some other type");
                    }



                    /*
                    Transition mytt = (Transition)mytrans;
                    foreach (Triple myTrip in mytt.getTriples())
                    {
                        Console.WriteLine("     - Triple: " + myTrip);
                        //Console.WriteLine("     - tool specific def: " + myFunc.Value.getToolSpecificDefinition());

                    }*/



                    /*
                    mytrans.get2DPageRatio();
                    Console.Write(" - Visualization - page ratio: " + mytrans.get2DPageRatio());
                    Console.Write(" - hight: " + mytrans.get);
                    Console.Write(" - width: " + mytrans.getRelative2DWidth());
                    Console.WriteLine(" - Pos: (" + mytrans.getRelative2DPosX() + "," + mytrans.getRelative2DPosY() + ")");
                    */
                }
            }
        }

        private static void findMappingFunctionIn(IDictionary<string, IPASSProcessModelElement> allElements)
        {


            Console.WriteLine("Data Mapping Functions: ");
            foreach (KeyValuePair<string, IPASSProcessModelElement> kvp in allElements)
            {
                IPASSProcessModelElement myComponent = kvp.Value;
                if (myComponent is IDataMappingFunction)
                {
                    IDataMappingFunction myDataMapping = (IDataMappingFunction)myComponent;
                    Console.WriteLine(" Found a Data Mapping Function: " + myDataMapping.getModelComponentID());
                    Console.WriteLine(" - typename: " + myDataMapping.GetType().Name);
                    Console.WriteLine(" - string: " + myDataMapping.getDataMappingString());
                }
            }
            Console.WriteLine("");
        }

        private static void iterateThroughStateAttributes(IState someState)
        {

            Console.WriteLine("  Attribute Details for State: " + someState.getModelComponentLabels()[0]);

            if (someState is IDoState)
            {
                DoState myDo = (DoState)someState;
                IDictionary<string, IDataMappingFunction> myMapDic = myDo.getDataMappingFunctions();
                Console.WriteLine("   - number of comments: " + myDo.getComments().Count());
                Console.WriteLine("   - number of incomplete Triples: " + myDo.getIncompleteTriples().Count());
                Console.WriteLine("   - number of Triples: " + myDo.getIncompleteTriples().Count());
                Console.WriteLine("   - SiSiAttributes: ");
                if (!(myDo.getSisiExecutionDuration() == null))
                {
                    Console.WriteLine("     - Times: " + myDo.getSisiExecutionDuration().ToString()); //getTriples().Count());
                }
                Console.WriteLine("     - cost: " + myDo.getSisiCostPerExecution()); //getTriples().Count());
                //Console.WriteLine("     - end stay chance: " + myDo.); //getTriples().Count());
                Console.WriteLine("     - VSM time category chance: " + myDo.getSisiVSMTimeCategory()); //getTriples().Count());

                Console.Write("     - Visualization - page ratio: " + myDo.get2DPageRatio());
                Console.Write(" - hight: " + myDo.getRelative2DHeight());
                Console.Write(" - width: " + myDo.getRelative2DWidth());
                Console.WriteLine(" - Pos: (" + myDo.getRelative2DPosX() + "," + myDo.getRelative2DPosY() + ")");


                Console.WriteLine("   - number of data mappings: " + myMapDic.Count);
                Console.WriteLine("   - number of unspecific Relations : " + myDo.getElementsWithUnspecifiedRelation().Count);
                foreach (KeyValuePair<string, IPASSProcessModelElement> myFunc in myDo.getElementsWithUnspecifiedRelation())
                {
                    Console.WriteLine("     - unspecific element: " + myFunc.Value.getModelComponentID());
                    //Console.WriteLine("     - tool specific def: " + myFunc.Value.getToolSpecificDefinition());

                }
            }


            if (someState is IMacroState)
            {
                IMacroState myMacroState = ((IMacroState)someState);
                if (myMacroState.getReferencedMacroBehavior() != null)
                {
                    Console.WriteLine("   - Macro State, called macroBehavior: " + myMacroState.getReferencedMacroBehavior().getModelComponentID());
                    IMacroBehavior myMB = myMacroState.getReferencedMacroBehavior();
                }
            }

            IDictionary<string, IPASSProcessModelElement> myDic = someState.getElementsWithUnspecifiedRelation();

            foreach (KeyValuePair<string, IPASSProcessModelElement> att in myDic)
            {
                Console.WriteLine("   - unspecific special: " + att.Key + " value: " + att.Value);

            }
        }
    }
}
