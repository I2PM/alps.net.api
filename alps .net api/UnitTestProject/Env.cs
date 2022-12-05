using alps.net.api;
using alps.net.api.parsing;
using alps.net.api.StandardPASS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UnitTestProject
{
    public class Env
    {
        private const string generatedFolder = "../../../src/generated/";
        private const string srcFolder = "../../../src/";
        private static bool firstDelete = false;
        private static IPASSReaderWriter ioHandler;

        public static string getTestResourcePath()
        {
            return srcFolder;
        }

        public static string getTestResourceGeneratePath()
        {
            if (!firstDelete)
            {
                if (Directory.Exists(generatedFolder)) { deleteDirectory(generatedFolder); }
                firstDelete = true;
            }
            Directory.CreateDirectory(generatedFolder);
            
            return generatedFolder;
        }
        public static string getTestResourceGeneratePath(Object callingInstance)
        {
            string generalPath = getTestResourceGeneratePath();
            string newPath = generalPath + "/" + callingInstance.GetType().Name;
            if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);

            return newPath + "/";
        }

        public static IPASSReaderWriter getIoHandler()
        {
            if (ioHandler is { }) return ioHandler;
            ReflectiveEnumerator.addAssemblyToCheckForTypes(Assembly.GetExecutingAssembly());
            ioHandler = PASSReaderWriter.getInstance();
            IList<string> path = new List<string>
            {
                // Going for the ontology that lies in the more general src folder to avoid multiple files
                "../../" + srcFolder + "standard_PASS_ont_v_1.1.0.owl",
                "../../" + srcFolder + "abstract-layered-pass-ont.owl",
            };
            ioHandler.loadOWLParsingStructure(path);
            return ioHandler;
        }

        public static void deleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                deleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        public static IPASSProcessModel getGenericModel()
        {
            IPASSProcessModel model = new PASSProcessModel("http://www.exampleTestUri.com");
            new FullySpecifiedSubject(model.getBaseLayer());
            return model;
        }
    }
}
