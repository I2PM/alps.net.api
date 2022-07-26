using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    /// <summary>
    /// Method that repesents an actor place holder 
    /// </summary>
    class ActorPlaceHolder : ALPSSIDComponent, IActorPlaceHolder
    {
        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "ActorPlaceHolder";

        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new ActorPlaceHolder();
        }

        protected ActorPlaceHolder() { }
        public ActorPlaceHolder(IModelLayer layer, string labelForID = null, string comment = null, string additionalLabel = null,
            IList<IIncompleteTriple> additionalAttribute = null)
            : base(layer, labelForID, comment, additionalLabel, additionalAttribute)
        { }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

    }
}
