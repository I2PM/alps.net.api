namespace LibraryExample.DynamicImporterExample
{
    /// <summary>
    /// This is an interface for all classes that do not belong to the standard library
    /// It helps the factory to differentiate between library and non-library classes
    /// </summary>
    public interface IAdditionalFunctionalityElement
    {
        string getAdditionalFunctionality();

        void setAdditionalFunctionality(string additionalFunctionality);
    }
}
