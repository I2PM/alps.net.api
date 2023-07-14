﻿using alps.net.api.parsing;
using alps.net.api.src;
using alps.net.api.util;
using System.Collections.Generic;

namespace alps.net.api.StandardPASS.PassProcessModelElements.DataDescribingComponents
{

    /// <summary>
    /// Class that represents a data mapping function
    /// </summary>
    public class DataMappingFunction : DataDescribingComponent, IDataMappingFunction
    {
        protected string dataMappingString;
        protected string feelExpression;
        protected string toolSpecificDefinition;
        /// <summary>
        /// Name of the class, needed for parsing
        /// </summary>
        private const string className = "DataMappingFunction";



        public override string getClassName()
        {
            return className;
        }
        public override IParseablePASSProcessModelElement getParsedInstance()
        {
            return new DataMappingFunction();
        }

        protected DataMappingFunction() { }

        public DataMappingFunction(IPASSProcessModel model, string labelForID = null, string dataMappingString = null, string feelExpression = null,
            string toolSpecificDefinition = null, string comment = null, string additionalLabel = null, IList<IIncompleteTriple> additionalAttribute = null)
            : base(model, labelForID, comment, additionalLabel, additionalAttribute)
        {
            setDataMappingString(dataMappingString);
            setFeelExpressionAsDataMapping(feelExpression);
            setToolSpecificDefinition(toolSpecificDefinition);
        }


        public void setDataMappingString(string dataMappingString)
        {
            if (dataMappingString != null && dataMappingString.Equals(this.dataMappingString)) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasDataMappingString, this.dataMappingString, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));
            this.dataMappingString = dataMappingString is null || dataMappingString.Equals("") ? null : dataMappingString;
            if (dataMappingString != null)
                addTriple(new IncompleteTriple(OWLTags.stdHasDataMappingString, dataMappingString, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));

        }


        public void setFeelExpressionAsDataMapping(string feelExpression)
        {
            if (feelExpression != null && feelExpression.Equals(this.feelExpression)) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasFeelExpressionAsDataMapping, this.feelExpression, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));
            this.feelExpression = feelExpression is null || feelExpression.Equals("") ? null : feelExpression;
            if (feelExpression != null)
            {
                addTriple(new IncompleteTriple(OWLTags.stdHasFeelExpressionAsDataMapping, feelExpression, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));
                setToolSpecificDefinition(null);
            }
        }


        public void setToolSpecificDefinition(string toolSpecificDefinition)
        {
            if (toolSpecificDefinition != null && toolSpecificDefinition.Equals(this.toolSpecificDefinition)) return;
            removeTriple(new IncompleteTriple(OWLTags.stdHasToolSpecificDefinition, this.toolSpecificDefinition, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));
            this.toolSpecificDefinition = toolSpecificDefinition is null || toolSpecificDefinition.Equals("") ? null : toolSpecificDefinition;
            if (toolSpecificDefinition != null)
            {
                addTriple(new IncompleteTriple(OWLTags.stdHasToolSpecificDefinition, toolSpecificDefinition, IncompleteTriple.LiteralType.DATATYPE, OWLTags.xsdDataTypeString));
                setFeelExpressionAsDataMapping(null);
            }
        }


        public string getDataMappingString()
        {
            return dataMappingString;
        }


        public string getFeelExpressionAsDataMapping()
        {
            return feelExpression;
        }


        public string getToolSpecificDefinition()
        {
            return toolSpecificDefinition;
        }

        public bool hasToolSpecificDefinition()
        {
            return !(feelExpression is null);
        }

        public bool hasFeelExpression()
        {
            return !(feelExpression is null);
        }


        protected override bool parseAttribute(string predicate, string objectContent, string lang, string dataType, IParseablePASSProcessModelElement element)
        {
            if (predicate.Contains(OWLTags.hasDataMappingString))
            {
                setDataMappingString(objectContent);
                return true;
            }
            else if (predicate.Contains(OWLTags.hasFeelExpressionAsDataMapping))
            {
                setFeelExpressionAsDataMapping(objectContent);
                return true;
            }
            else if (predicate.Contains(OWLTags.hasToolSpecificDefinition))
            {
                setToolSpecificDefinition(objectContent);
                return true;
            }
            else return base.parseAttribute(predicate, objectContent, lang, dataType, element);
        }


    }
}
