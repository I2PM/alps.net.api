namespace alps.net.api.StandardPASS
{
    /// <summary>
    /// Interface for the MessageSpecification class
    /// </summary>

    public interface IMessageSpecification : IInteractionDescribingComponent
    {
        /// <summary>
        /// Sets the payload description for the message specification, which describes the payload of the message
        /// </summary>
        /// <param name="payloadDescription">the payload description</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setContainedPayloadDescription(IPayloadDescription payloadDescription, int removeCascadeDepth = 0);

        /// <summary>
        /// Returns the payload description for the message specification, which describes the payload of the message
        /// </summary>
        /// <returns>the payload description</returns>
        IPayloadDescription getContainedPayloadDescription();
    }

}
