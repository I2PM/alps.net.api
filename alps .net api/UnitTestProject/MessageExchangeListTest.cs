using alps.net.api.ALPS;
using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class MessageExchangeListTest
    {
        [TestMethod]
        public void messageExchangeListExport()
        {
            IPASSProcessModel model =  Env.getGenericModel();
            IModelLayer layer = model.getBaseLayer();
            IMessageExchangeList list = new MessageExchangeList(layer, "list");
            for (int i = 0; i < 5; i++)
            {
                IMessageSpecification specification = new MessageSpecification(layer, "specification" + i);
                specification.setModelComponentID("specification" + i);
                IFullySpecifiedSubject sender = new FullySpecifiedSubject(layer);
                IFullySpecifiedSubject receiver = new FullySpecifiedSubject(layer);
                IMessageExchange exchange = new MessageExchange(layer, "messageExchange" + i, specification, sender, receiver);
                list.addContainsMessageExchange(exchange);
                list.setModelComponentID("messageList" + i);
            }
            Env.getIoHandler().exportModel(model, Env.getTestResourceGeneratePath(this) + "ExchangeList");
        }
    }
}
