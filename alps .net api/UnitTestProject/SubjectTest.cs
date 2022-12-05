using alps.net.api.StandardPASS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestProject
{
    [TestClass]
    public class SubjectTest
    {
        [TestMethod]
        public void checkStartSubject()
        {
            IPASSProcessModel model = Env.getGenericModel();
            IList<ISubject> subjList = model.getAllElements().Values.OfType<ISubject>().ToList();
            ISubject subj = subjList.FirstOrDefault();

            // Set role on Subj
            Assert.IsTrue(model.getStartSubjects().Count == 0);
            subj.assignRole(ISubject.Role.StartSubject);
            Assert.IsTrue(model.getStartSubjects().Count == 1);
            subj.removeRole(ISubject.Role.StartSubject);
            Assert.IsTrue(model.getStartSubjects().Count == 0);

            // Add start subj to model with subject contained
            model.addStartSubject(subj);
            Assert.IsTrue(model.getStartSubjects().Count == 1);
            Assert.IsTrue(subj.isRole(ISubject.Role.StartSubject));
            model.removeStartSubject(subj.getModelComponentID());
            Assert.IsTrue(model.getStartSubjects().Count == 0);
            Assert.IsFalse(subj.isRole(ISubject.Role.StartSubject));
            Assert.IsTrue(model.getAllElements().Values.OfType<ISubject>().ToList().Count == 1);

            // Add start subj to model without subject contained
            model.removeElement(subj.getModelComponentID());
            model.addStartSubject(subj);
            Assert.IsTrue(model.getStartSubjects().Count == 1);
            Assert.IsTrue(subj.isRole(ISubject.Role.StartSubject));
            model.removeStartSubject(subj.getModelComponentID());
            Assert.IsTrue(model.getStartSubjects().Count == 0);
            Assert.IsFalse(subj.isRole(ISubject.Role.StartSubject));
            Assert.IsTrue(model.getAllElements().Values.OfType<ISubject>().ToList().Count == 1);

            // Add a StartSubject to the model and remove it again
            model.removeElement(subj.getModelComponentID());
            subj.assignRole(ISubject.Role.StartSubject);
            Assert.IsTrue(model.getStartSubjects().Count == 0);
            model.addElement(subj);
            Assert.IsTrue(model.getStartSubjects().Count == 1);
            Assert.IsTrue(subj.isRole(ISubject.Role.StartSubject));
            model.removeElement(subj.getModelComponentID());
            Assert.IsTrue(model.getStartSubjects().Count == 0);
            Assert.IsFalse(subj.isRole(ISubject.Role.StartSubject));
            Assert.IsTrue(model.getAllElements().Values.OfType<ISubject>().ToList().Count == 0);
        }
    }
}
