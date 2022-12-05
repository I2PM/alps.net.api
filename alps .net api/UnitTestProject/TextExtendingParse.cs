using alps.net.api;
using alps.net.api.parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class TextExtendingParse
    {


        [TestMethod]
        public void test()
        {
            
            IPASSReaderWriter io =  Env.getIoHandler();
            if (io is PASSReaderWriter readerWriter)
            {
                Assert.IsTrue(readerWriter.getParsingDict().TryGetValue("FullySpecifiedSubject", out IList<(ITreeNode<IParseablePASSProcessModelElement>, int)> listOfNodes));
                Assert.IsTrue(listOfNodes.Count > 0);
                bool contains = false;
                foreach((ITreeNode<IParseablePASSProcessModelElement>, int) tuple in listOfNodes)
                {
                    if (tuple.Item1.getContent() is TestExtendedFullySpecifiedSubject)
                    {
                        contains = true;
                    }
                }
                Assert.IsTrue(contains);

                Assert.IsTrue(readerWriter.getParsingDict().TryGetValue("PASSProcessModel", out IList<(ITreeNode<IParseablePASSProcessModelElement>, int)> listOfNodes2));
                Assert.IsTrue(listOfNodes2.Count > 0);
                bool contains2 = false;
                foreach ((ITreeNode<IParseablePASSProcessModelElement>, int) tuple in listOfNodes2)
                {
                    if (tuple.Item1.getContent() is TestPASSProcessModel)
                    {
                        contains2 = true;
                    }
                }
                Assert.IsTrue(contains2);
            }
        }
    }
}
