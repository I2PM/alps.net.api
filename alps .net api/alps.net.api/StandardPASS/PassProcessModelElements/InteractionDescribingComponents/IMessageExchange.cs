namespace alps.net.api.StandardPASS.InteractionDescribingComponents
{
    /// <summary>
    /// Interface to the message exchange class
    /// </summary>

    public interface IMessageExchange : IInteractionDescribingComponent
    {
        /// <summary>
        /// Method that sets the message specification attribute of the instance
        /// </summary>
        /// <param name="messageSpecification">the type of message</param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setMessageType(IMessageSpecification messageSpecification, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the message specification attribute of the instance
        /// </summary>
        /// <returns>The message specification attribute of the instance</returns>
        IMessageSpecification getMessageType();

        /// <summary>
        /// Method that sets the receiver attribute of the instance
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setReceiver(ISubject receiver, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the receiver attribute of the instance
        /// </summary>
        /// <returns>The receiver attribute of the instance</returns>
        ISubject getReceiver();

        /// <summary>
        /// Method that sets the sender attribute of the instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="removeCascadeDepth">Parses the depth of a cascading delete for elements that are connected to the currently deleted one</param>
        void setSender(ISubject sender, int removeCascadeDepth = 0);

        /// <summary>
        /// Method that returns the sender attribute of the instance
        /// </summary>
        /// <returns>The sender attribute of the instance</returns>
        ISubject getSender();

    }

}
