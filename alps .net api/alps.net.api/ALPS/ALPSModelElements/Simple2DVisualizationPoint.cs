
using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.ALPS
{
    public class Simple2DVisualizationPoint : ALPSModelElement, ISimple2DVisualizationPoint
    {

        protected double posx, posy;

        /// <summary>
        /// Name of the class
        /// </summary>
        private const string className = "Simple2DVisualizationPoint";

        public Simple2DVisualizationPoint(string labelForID = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(labelForID, comment, additionalLabel, additionalAttribute) { }

        public override string getClassName()
        {
            return className;
        }

        protected override string getExportTag()
        {
            return OWLTags.abstr;
        }

        public double getRelative2D_PosX()
        {
            return posx;
        }

        public double getRelative2D_PosY()
        {
            return posy;
        }

        public void setRelative2D_PosX(double posx)
        {
            this.posx = posx;
        }

        public void setRelative2D_PosY(double posy)
        {
            this.posy = posy;
        }

        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new Simple2DVisualizationPoint();
        }

       protected Simple2DVisualizationPoint() { }
    }
}
