using alps.net.api.ALPS.ALPSModelElements;
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS.InteractionDescribingComponents
{
    /// <summary>
    /// Class that represents a subject
    /// </summary>
    public class Subject : InteractionDescribingComponent, ISubject
    {

        // Needs StartSubject as type


        protected int instanceRestriction;
        protected readonly IDictionary<string, IMessageExchange> incomingExchange = new Dictionary<string, IMessageExchange>();
        protected readonly IDictionary<string, IMessageExchange> outgoingExchange = new Dictionary<string, IMessageExchange>();
        protected readonly IDictionary<string, ISubject> implementedInterfaces = new Dictionary<string, ISubject>();
        protected readonly IList<ISubject.Role> roles = new List<ISubject.Role>();
        protected bool isAbstractType = false;

        private const string ABSTRACT_NAME = "AbstractSubject";

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "Subject";


        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new Subject();
        }

       protected Subject() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="comment"></param>
        /// <param name="incomingMessageExchange"></param>
        /// <param name="outgoingMessageExchange"></param>
        /// <param name="maxSubjectInstanceRestriction"></param>
        /// <param name="additionalAttribute"></param>
        public Subject(IModelLayer layer, string labelForID = null,  ISet<IMessageExchange> incomingMessageExchange = null,
            ISet<IMessageExchange> outgoingMessageExchange = null, int maxSubjectInstanceRestriction = 1, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setIncomingMessageExchanges(incomingMessageExchange);
            setInstanceRestriction(maxSubjectInstanceRestriction);
            setOutgoingMessageExchanges(outgoingMessageExchange);
        }

        public int getInstanceRestriction() { return instanceRestriction; }

        public void setInstanceRestriction(int restriction)
        {
            if (restriction == this.instanceRestriction) return;
            removeTriple(new IncompleteTriple(OWLTags.stdhasInstanceRestriction, instanceRestriction.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeNonNegativeInt));
            this.instanceRestriction = (restriction >= 0) ? restriction : 0;
            addTriple(new IncompleteTriple(OWLTags.stdhasInstanceRestriction, instanceRestriction.ToString(), IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeNonNegativeInt));
        }



        public void addIncomingMessageExchange(IMessageExchange exchange)
        {
            if (exchange is null) { return; }
            if (incomingExchange.TryAdd(exchange.getModelComponentID(), exchange))
            {
                publishElementAdded(exchange);
                exchange.register(this);
                exchange.setReceiver(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasIncomingMessageExchange, exchange.getUriModelComponentID()));
            }
        }

        public void addOutgoingMessageExchange(IMessageExchange exchange)
        {
            if (exchange is null) { return; }
            if (outgoingExchange.TryAdd(exchange.getModelComponentID(), exchange))
            {
                publishElementAdded(exchange);
                exchange.register(this);
                exchange.setSender(this);
                addTriple(new IncompleteTriple(OWLTags.stdHasOutgoingMessageExchange, exchange.getUriModelComponentID()));
            }
        }

        public IDictionary<string, IMessageExchange> getIncomingMessageExchanges()
        {
            return new Dictionary<string, IMessageExchange>(incomingExchange);
        }

        public IDictionary<string, IMessageExchange> getOutgoingMessageExchanges()
        {
            return new Dictionary<string, IMessageExchange>(outgoingExchange);
        }


        public void setIncomingMessageExchanges(ISet<IMessageExchange> exchanges, int removeCascadeDepth = 0)
        {
            foreach (IMessageExchange exchange in getIncomingMessageExchanges().Values)
            {
                removeIncomingMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
            }
            if (exchanges is null) return;
            foreach (IMessageExchange exchange in exchanges)
            {
                addIncomingMessageExchange(exchange);
            }
        }

        public void setOutgoingMessageExchanges(ISet<IMessageExchange> exchanges, int removeCascadeDepth = 0)
        {
            foreach (IMessageExchange exchange in getOutgoingMessageExchanges().Values)
            {
                removeOutgoingMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
            }
            if (exchanges is null) return;
            foreach (IMessageExchange exchange in exchanges)
            {
                addOutgoingMessageExchange(exchange);
            }
        }

        public void removeIncomingMessageExchange(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (incomingExchange.TryGetValue(id, out IMessageExchange exchange))
            {
                incomingExchange.Remove(id);
                exchange.unregister(this, removeCascadeDepth);
                exchange.setReceiver(null);
                removeTriple(new IncompleteTriple(OWLTags.stdHasIncomingMessageExchange, exchange.getUriModelComponentID()));
            }
        }

        public void removeOutgoingMessageExchange(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (outgoingExchange.TryGetValue(id, out IMessageExchange exchange))
            {
                outgoingExchange.Remove(id);
                exchange.unregister(this, removeCascadeDepth);
                exchange.setSender(null);
                removeTriple(new IncompleteTriple(OWLTags.stdHasOutgoingMessageExchange, exchange.getUriModelComponentID()));
            }
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasInstanceRestriction))
            {
                string restr = objectContent;
                setInstanceRestriction(int.Parse(restr));
                return true;
            }
            else if (predicate.Contains(OWLTags.type) && objectContent.Contains("StartSubject"))
            {
                assignRole(ISubject.Role.StartSubject);
                return true;
            }
            else if (element != null)
            {
                if (element is IMessageExchange messageExchange)
                {
                    if (predicate.Contains(OWLTags.hasIncomingMessageExchange))
                    {
                        addIncomingMessageExchange(messageExchange);
                        return true;
                    }
                    else if (predicate.Contains(OWLTags.hasOutgoingMessageExchange))
                    {
                        addOutgoingMessageExchange(messageExchange);
                        return true;
                    }
                }
            }
            else
            {
                if (predicate.Contains(OWLTags.type))
                {
                    if (objectContent.Contains(ABSTRACT_NAME))
                    {
                        setIsAbstract(true);
                        return true;
                    }
                }
            }
            return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }

        public void assignRole(ISubject.Role role)
        {
            if (!roles.Contains(role))
            {
                roles.Add(role);
                if (role == ISubject.Role.StartSubject)
                    addTriple(new IncompleteTriple(OWLTags.rdfType, "standard-pass-ont:StartSubject"));
            }
        }

        public bool isRole(ISubject.Role role)
        {
            return roles.Contains(role);
        }

        public void removeRole(ISubject.Role role)
        {
            if (roles.Contains(role))
            {
                roles.Remove(role);
                if (role == ISubject.Role.StartSubject)
                    removeTriple(new IncompleteTriple(OWLTags.rdfType, "standard-pass-ont:StartSubject"));
            }
        }

        public void setImplementedInterfaces(ISet<ISubject> implementedInterface, int removeCascadeDepth = 0)
        {
            foreach (ISubject implInterface in getImplementedInterfaces().Values)
            {
                removeImplementedInterfaces(implInterface.getModelComponentID(), removeCascadeDepth);
            }
            if (implementedInterface is null) return;
            foreach (ISubject implInterface in implementedInterface)
            {
                addImplementedInterface(implInterface);
            }
        }

        public void addImplementedInterface(ISubject implementedInterface)
        {
            if (implementedInterface is null) { return; }
            if (implementedInterfaces.TryAdd(implementedInterface.getModelComponentID(), implementedInterface))
            {
                publishElementAdded(implementedInterface);
                implementedInterface.register(this);
                addTriple(new IncompleteTriple(OWLTags.abstrImplements, implementedInterface.getUriModelComponentID()));
            }
        }

        public void removeImplementedInterfaces(string id, int removeCascadeDepth = 0)
        {
            if (id is null) return;
            if (implementedInterfaces.TryGetValue(id, out ISubject implInterface))
            {
                implementedInterfaces.Remove(id);
                implInterface.unregister(this, removeCascadeDepth);
                removeTriple(new IncompleteTriple(OWLTags.abstrImplements, implInterface.getUriModelComponentID()));
            }
        }

        public IDictionary<string, ISubject> getImplementedInterfaces()
        {
            return new Dictionary<string, ISubject>(implementedInterfaces);
        }

        public override void notifyModelComponentIDChanged(string oldID, string newID)
        {
            if (incomingExchange.ContainsKey(oldID))
            {
                IMessageExchange element = incomingExchange[oldID];
                incomingExchange.Remove(oldID);
                incomingExchange.Add(element.getModelComponentID(), element);
            }
            if (outgoingExchange.ContainsKey(oldID))
            {
                IMessageExchange element = outgoingExchange[oldID];
                outgoingExchange.Remove(oldID);
                outgoingExchange.Add(element.getModelComponentID(), element);
            }
            base.notifyModelComponentIDChanged(oldID, newID);
        }

        public override ISet<IPASSProcessModelElement> getAllConnectedElements(ConnectedElementsSetSpecification specification)
        {
            ISet<IPASSProcessModelElement> baseElements = base.getAllConnectedElements(specification);
            foreach (IMessageExchange exchange in getIncomingMessageExchanges().Values) baseElements.Add(exchange);
            foreach (IMessageExchange exchange in getOutgoingMessageExchanges().Values) baseElements.Add(exchange);
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
                    removeIncomingMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
                    // Try to remove the outgoing exchange
                    removeOutgoingMessageExchange(exchange.getModelComponentID(), removeCascadeDepth);
                }
            }
        }

        public void setIsAbstract(bool isAbstract)
        {
            this.isAbstractType = isAbstract;
            if (isAbstract)
            {
                addTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + ABSTRACT_NAME));
            }
            else
            {
                removeTriple(new IncompleteTriple(OWLTags.rdfType, getExportTag() + ABSTRACT_NAME));
            }
        }

        public bool isAbstract()
        {
            return isAbstractType;
        }
    }
}