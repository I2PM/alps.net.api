using alps.net.api.ALPS.ALPSModelElements;
using alps.net.api.parsing;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an InteractionDescriptionComponten 
    /// </summary>

    public class InteractionDescribingComponent : PASSProcessModelElement, IInteractionDescribingComponent
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "InteractionDescribingComponent";

        protected IModelLayer layer;

        public override string getClassName()
        {
            return className;
        }

        public void setContainedBy(IModelLayer layer)
        {
            if (layer == null || layer.Equals(this.layer)) return;
            this.layer = layer;
            layer.addElement(this);
        }

        public bool getContainedBy(out IModelLayer subject)
        {
            subject = layer;
            return layer != null;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new InteractionDescribingComponent();
        }

        protected InteractionDescribingComponent() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="additionalAttribute"></param>
        public InteractionDescribingComponent(IModelLayer layer, string labelForID = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { setContainedBy(layer); }

        public override string getBaseURI()
        {
            if (layer != null && layer is IParseablePASSProcessModelElement parseable)
                return parseable.getBaseURI();
            return base.getBaseURI();
        }

        protected override IDictionary<string, IParseablePASSProcessModelElement> getDictionaryOfAllAvailableElements()
        {
            if (!getContainedBy(out IModelLayer layer) ) return null;
            if (!layer.getContainedBy(out IPASSProcessModel model)) return null;
            IDictionary<string, IPASSProcessModelElement> allElements = model.getAllElements();
            IDictionary<string, IParseablePASSProcessModelElement> allParseableElements = new Dictionary<string, IParseablePASSProcessModelElement>();
            foreach (KeyValuePair<string, IPASSProcessModelElement> pair in allElements)
                if (pair.Value is IParseablePASSProcessModelElement parseable) allParseableElements.Add(pair.Key, parseable);
            return allParseableElements;
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            if (specification == ConnectedElementsSetSpecification.ALL)
                if (getContainedBy(out IModelLayer layer)) baseElements.Add(layer);
            return baseElements;
        }

    }
}
