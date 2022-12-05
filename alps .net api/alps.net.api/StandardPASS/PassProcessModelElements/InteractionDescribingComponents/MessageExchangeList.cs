using alps.net.api.ALPS;
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Class that represents an message exchange list
    /// </summary>

    public class MessageExchangeList : InteractionDescribingComponent, IMessageExchangeList
    {
        protected ICompatibilityDictionary<string, IMessageExchange> messageExchanges = new CompatibilityDictionary<string, IMessageExchange>();

        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "MessageExchangeList";


        public override string getClassName()
        {
            return className;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new MessageExchangeList();
        }

       protected MessageExchangeList() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="messageExchanges"></param>
        /// <param name="additionalAttribute"></param>
        public MessageExchangeList(IModelLayer layer, string labelForID = null,  ISet<IMessageExchange> messageExchanges = null, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null) : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setContainsMessageExchanges(messageExchanges);

        }


        public void addContainsMessageExchange(IMessageExchange messageExchange)
        {
            if (messageExchange is null) { return; }
            if (messageExchanges.TryAdd(messageExchange.getModelComponentID(), messageExchange))
            {
                publishElementAdded(messageExchange);
                messageExchange.register(this);
                addTriple(new IncompleteTriple(OWLTags.stdContains, messageExchange.getUriModelComponentID()));
            }
        }


        public void setContainsMessageExchanges(ISet<IMessageExchange> messageExchanges, int removeCascadeDepth = 0)
        {
            foreach (IMessageExchange messageExchange in getMessageExchanges().Values)
            {
                removeMessageExchange(messageExchange.getModelComponentID(), removeCascadeDepth);
            }
            if (messageExchanges is null) return;
            foreach (IMessageExchange messageExchange in messageExchanges)
            {
                addContainsMessageExchange(messageExchange);
            }
        }

        public void removeMessageExchange(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (messageExchanges.TryGetValue(id, out IMessageExchange exchange))
            {
                messageExchanges.Remove(id);
                exchange.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.stdContains, exchange.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IMessageExchange> getMessageExchanges()
        {
            return new Dictionary<string, IMessageExchange>(messageExchanges);
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (element != null)
            {
                if (predicate.Contains(OWLTags.contains) && element is IMessageExchange exchange)
                {
                    addContainsMessageExchange(exchange);
                    return true;
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (messageExchanges.ContainsKey(oldID))
            {
                IMessageExchange element = messageExchanges[oldID];
                messageExchanges.Remove(oldID);
                messageExchanges.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach(IMessageExchange exchange in getMessageExchanges().Values) baseElements.Add(exchange);
            return baseElements;
        }

        public override void updateRemoved(IPASSProcessModelElement update, IPASSProcessModelElement caller, int removeCascadeDepth = 0)
        {
            base.updateRemoved(update, caller, removeCascadeDepth);
            if (update != null)
            {
                if (update is IMessageExchange exchange)
                {
                    // Try to remove the incoming exchange
                    removeMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
                }
            }
        }
    }
}