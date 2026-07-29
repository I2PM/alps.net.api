using alps.net.api.ALPS;
using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject
{
    /// <summary>
    /// Regression tests for parsing models that contain a guard (interruption) behavior,
    /// using the Order-Supplier model:
    /// - SID_1 is the standard layer (Orderer, Suppliers and their SBDs).
    /// - SID_7 is a guard layer (rdf:type GuardLayer) containing the "Supplier Cancellation Guard"
    ///   subject extension, its GuardBehavior (GBD_8...) and the Customer Satisfaction Department.
    /// - The GuardBehavior guards the Suppliers' base SBD (guardsBehavior triple).
    ///
    /// The states of the guarded (normal) SBD must stay contained in that SBD - the guard
    /// behavior must not claim them - while the guard's own states must stay contained in the
    /// guard behavior. Consumers rely on state.getContainedBy() to classify states as
    /// normal-layer vs. guard-layer states.
    /// </summary>
    [TestClass]
    public class GuardBehaviorParsingTest
    {
        private const string ORDERER_SBD = "SBD_4_SID_1_FullySpecifiedSubject_2";
        private const string SUPPLIER_SBD = "SBD_5_SID_1_FullySpecifiedSubject_49";
        private const string CUST_SAT_SBD = "SBD_12_SID_1_FullySpecifiedSubject_55";
        private const string GUARD_BEHAVIOR = "GBD_8_SID_1_GuardExtension_3";
        private const string GUARD_ENTRY_STATE = "SBD_8_GuardReceiveState_1";
        private const string SUPPLIERS_SUBJECT = "SID_1_FullySpecifiedSubject_49";
        private const string GUARD_EXTENSION = "SID_7_GuardExtension_3";
        private const string CUST_SAT_SUBJECT = "SID_7_FullySpecifiedSubject_55";
        private const string STANDARD_LAYER = "SID_1";
        private const string GUARD_LAYER = "SID_7";

        private static IPASSProcessModel model;

        private static IPASSProcessModel loadModel()
        {
            if (model is { }) return model;
            // Deliberately not using Env.getIoHandler(): it registers the test assembly for type
            // scanning, which changes the parsing tree. This mirrors how external consumers load models.
            var io = alps.net.api.parsing.PASSReaderWriter.getInstance();
            io.loadOWLParsingStructure(new List<string>
            {
                "../../../../../src/standard_PASS_ont_v_1.1.0.owl",
                "../../../../../src/ALPS_ont_v_0.8.0.owl",
            });
            var models = io.loadModels(new List<string> { "../../../src/Order-Supplier.owl" });
            model = models[0];
            return model;
        }

        private static IDictionary<string, IPASSProcessModelElement> allElements()
        {
            return loadModel().getAllElements();
        }

        private static IEnumerable<IState> statesOf(string behaviorId)
        {
            var behavior = (ISubjectBehavior)allElements()[behaviorId];
            return behavior.getBehaviorDescribingComponents().Values.OfType<IState>();
        }

        [TestMethod]
        public void guardBehaviorIsParsedAsGuardBehavior()
        {
            Assert.IsInstanceOfType(allElements()[GUARD_BEHAVIOR], typeof(IGuardBehavior));
            Assert.IsNotInstanceOfType(allElements()[SUPPLIER_SBD], typeof(IGuardBehavior));
            Assert.IsNotInstanceOfType(allElements()[ORDERER_SBD], typeof(IGuardBehavior));
            Assert.IsNotInstanceOfType(allElements()[CUST_SAT_SBD], typeof(IGuardBehavior));
        }

        [TestMethod]
        public void statesOfGuardedBehaviorStayInGuardedBehavior()
        {
            // The guarded (normal) SBD of the Suppliers subject: none of its states may be
            // claimed by the guard behavior that guards it.
            var states = statesOf(SUPPLIER_SBD).ToList();
            Assert.IsTrue(states.Count > 0, "Supplier SBD contains no states at all");
            foreach (IState state in states)
            {
                Assert.IsTrue(state.getContainedBy(out ISubjectBehavior behavior),
                    state.getModelComponentID() + " has no containing behavior");
                Assert.AreEqual(SUPPLIER_SBD, behavior.getModelComponentID(),
                    state.getModelComponentID() + " was claimed by " + behavior.getModelComponentID());
                Assert.IsNotInstanceOfType(behavior, typeof(IGuardBehavior),
                    state.getModelComponentID() + " is owned by a guard behavior");
            }
        }

        [TestMethod]
        public void guardBehaviorContainsOnlyItsOwnComponents()
        {
            var guard = (ISubjectBehavior)allElements()[GUARD_BEHAVIOR];
            var guardedSbdComponents = ((ISubjectBehavior)allElements()[SUPPLIER_SBD])
                .getBehaviorDescribingComponents().Keys;
            var stolen = guard.getBehaviorDescribingComponents().Keys.Intersect(guardedSbdComponents).ToList();
            Assert.AreEqual(0, stolen.Count,
                "Guard behavior claimed components of the guarded SBD: " + string.Join(", ", stolen));
        }

        [TestMethod]
        public void guardStatesAreContainedInGuardBehavior()
        {
            var guardStates = statesOf(GUARD_BEHAVIOR).ToList();
            Assert.AreEqual(3, guardStates.Count, "Guard behavior should hold exactly its 3 own states");
            foreach (IState state in guardStates)
            {
                Assert.IsTrue(state.getContainedBy(out ISubjectBehavior behavior));
                Assert.AreEqual(GUARD_BEHAVIOR, behavior.getModelComponentID());
                Assert.IsInstanceOfType(behavior, typeof(IGuardBehavior));
            }
        }

        [TestMethod]
        public void noStateOutsideTheGuardIsOwnedByAGuardBehavior()
        {
            // Model-wide sweep: exactly the guard's own states may resolve to a guard behavior.
            var guardOwned = allElements().Values.OfType<IState>()
                .Where(s => s.getContainedBy(out ISubjectBehavior b) && b is IGuardBehavior)
                .Select(s => s.getModelComponentID())
                .OrderBy(id => id)
                .ToList();
            CollectionAssert.AreEqual(
                new List<string> { "SBD_8_DoState_33", GUARD_ENTRY_STATE, "SBD_8_SendState_55" },
                guardOwned,
                "States owned by guard behaviors: " + string.Join(", ", guardOwned));
        }

        [TestMethod]
        public void guardEntryStateIsResolvable()
        {
            var guard = (ISubjectBehavior)allElements()[GUARD_BEHAVIOR];
            IState entry = guard.getInitialStateOfBehavior();
            Assert.IsNotNull(entry, "guard behavior has no resolvable initial state");
            Assert.AreEqual(GUARD_ENTRY_STATE, entry.getModelComponentID());
            Assert.IsInstanceOfType(entry, typeof(IReceiveState));
            Assert.IsTrue(entry.isStateType(IState.StateType.InitialStateOfBehavior));
        }

        [TestMethod]
        public void suppliersSubjectExposesBaseAndGuardBehavior()
        {
            var suppliers = (IFullySpecifiedSubject)allElements()[SUPPLIERS_SUBJECT];
            var behaviors = suppliers.getBehaviors();
            Assert.IsTrue(behaviors.ContainsKey(SUPPLIER_SBD), "Suppliers subject lost its base SBD");
            Assert.IsTrue(behaviors.ContainsKey(GUARD_BEHAVIOR), "Suppliers subject lost its guard behavior");
        }

        [TestMethod]
        public void guardLayerKeepsExplicitTypeAndItsElements()
        {
            var guardLayer = (IModelLayer)allElements()[GUARD_LAYER];
            var standardLayer = (IModelLayer)allElements()[STANDARD_LAYER];

            // SID_7 is explicitly rdf-typed as GuardLayer in the OWL and must report it,
            // so that consumers can put its subjects (guard extension, Customer Satisfaction
            // Department) on the guard layer.
            Assert.AreEqual(IModelLayer.LayerType.GUARD, guardLayer.getLayerType());
            Assert.AreEqual(IModelLayer.LayerType.STANDARD, standardLayer.getLayerType());

            // The guard's subject extension must not be ejected from its layer
            Assert.IsTrue(guardLayer.getElements().ContainsKey(GUARD_EXTENSION),
                "guard extension was removed from its layer");
            Assert.IsTrue(guardLayer.getElements().ContainsKey(CUST_SAT_SUBJECT),
                "Customer Satisfaction Department is missing from the guard layer");

            var extension = (ISubjectExtension)guardLayer.getElements()[GUARD_EXTENSION];
            Assert.IsNotNull(extension.getExtendedSubject(), "guard extension lost its extended subject");
            Assert.AreEqual(SUPPLIERS_SUBJECT, extension.getExtendedSubject().getModelComponentID());
        }

        [TestMethod]
        public void subjectsResolveToTheirLayers()
        {
            // Subject-level layer classification: normal subjects resolve to the standard
            // layer, guard-layer-only subjects (Customer Satisfaction Department) to the guard layer.
            var custSat = (ISubject)allElements()[CUST_SAT_SUBJECT];
            Assert.IsTrue(custSat.getContainedBy(out IModelLayer custSatLayer));
            Assert.AreEqual(GUARD_LAYER, custSatLayer.getModelComponentID());

            var suppliers = (ISubject)allElements()[SUPPLIERS_SUBJECT];
            Assert.IsTrue(suppliers.getContainedBy(out IModelLayer suppliersLayer));
            Assert.AreEqual(STANDARD_LAYER, suppliersLayer.getModelComponentID());
        }
    }
}
