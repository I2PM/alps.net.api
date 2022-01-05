using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.StandardPASS.SubjectBehaviors;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS.BehaviorDescribingComponents
{
    /// <summary>
    /// Class that represents a Choice Segment
    /// </summary>
    public class ChoiceSegment : State, IChoiceSegment
    {
        protected IDictionary<string, IChoiceSegmentPath> choiceSegmentPathDict = new Dictionary<string, IChoiceSegmentPath>();

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "ChoiceSegment";

        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ChoiceSegment();
        }

       protected ChoiceSegment() { }
        public ChoiceSegment(ISubjectBehavior behavior, string labelForID = null,   IGuardBehavior guardBehavior = null,
            IFunctionSpecification functionSpecification = null, ISet<ITransition> incomingTransition = null, ISet<ITransition> outgoingTransition = null,
            ISet<IChoiceSegmentPath> choiceSegmentPathList = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(behavior, labelForID,  guardBehavior, functionSpecification, incomingTransition, outgoingTransition, comment, additionalLabel, additionalAttribute)
        {
            setContainsChoiceSegmentPaths(choiceSegmentPathList);
        }


        public void setContainsChoiceSegmentPaths(ISet<IChoiceSegmentPath> choiceSegmentPaths, int removeCascadeDepth = 0)
        {
            foreach (IChoiceSegmentPath path in getChoiceSegmentPaths().Values)
            {
                removeChoiceSegmentPath(path.getModelComponentID(), removeCascadeDepth);
            }
            if (choiceSegmentPaths is null) return;
            foreach (IChoiceSegmentPath path in choiceSegmentPaths)
            {
                addContainsChoiceSegmentPath(path);
            }
        }

        public void addContainsChoiceSegmentPath(IChoiceSegmentPath choiceSegmentPath)
        {
            if (choiceSegmentPath is null) { return; }
            if (choiceSegmentPathDict.TryAdd(choiceSegmentPath.getModelComponentID(), choiceSegmentPath))
            {
                publishElementAdded(choiceSegmentPath);
                choiceSegmentPath.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdContains, choiceSegmentPath.getUriModelComponentID()));
            }
            
        }


        public IDictionary<string, IChoiceSegmentPath> getChoiceSegmentPaths()
        {
            return new Dictionary<string, IChoiceSegmentPath>(choiceSegmentPathDict);
        }

        public void removeChoiceSegmentPath(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (choiceSegmentPathDict.TryGetValue(id, out IChoiceSegmentPath path))
            {
                choiceSegmentPathDict.Remove(id);
                path.unregister(this, removeCascadeDepth);
                addTriple(new IncompleteTriple(OWLTags.stdContains, path.getUriModelComponentID()));
            }
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.contains) && element is IChoiceSegmentPath path)
                {
                    addContainsChoiceSegmentPath(path);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IChoiceSegmentPath component in getChoiceSegmentPaths().Values)
                baseElements.Add(component);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IChoiceSegmentPath path)
                    removeChoiceSegmentPath(path.getModelComponentID(), removeCascadeDepth);
            }
        }

    }
}
