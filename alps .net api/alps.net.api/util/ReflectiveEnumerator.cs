using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace alps.net.api
{
    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<T> getEnumerableOfType<T>(T element) where T : class
        {
            Type typeOfBase = element.GetType();
            if (!(element is T)) return null;
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.BaseType == typeOfBase))
            {
                T createdObject = createInstance<T>(type);
                // created Object is null
                if (createdObject != null) objects.Add(createdObject);

            }
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsInterface && checkInterfacesSame(myType, typeOfBase)))
            {
                foreach (Type innerType in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && checkDirectlyImplementsInterface(type, myType)))
                {
                    T createdObject = createInstance<T>(type);
                    // created Object is null
                    if (createdObject != null) objects.Add(createdObject);

                }

            }
            return objects;
        }

        /*public static ITreeNode<T> getTreeForType<T>(Type baseType)
        {
            Type typeOfBase = element.GetType();
            if (!(element is T)) return null;
            List<T> objects = new List<T>();

            // Case baseType is a class type or abstract class type -> the type might be a BaseType to other (abstract) classes 
            if (!baseType.IsInterface)
                foreach (Type subType in
                    Assembly.GetAssembly(typeof(T)).GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.BaseType == typeOfBase))
                {
                    T createdObject = createInstance<T>(subType, constructorArgs);
                    // created Object is null
                    if (createdObject != null) objects.Add(createdObject);

                }
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsInterface && checkInterfacesSame(myType, typeOfBase)))
            {
                foreach (Type innerType in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && checkDirectlyImplementsInterface(type, myType)))
                {
                    T createdObject = createInstance<T>(type, constructorArgs);
                    // created Object is null
                    if (createdObject != null) objects.Add(createdObject);

                }

            }
            return objects;
        }*/
        // Baum hier erstellen, Baum aus Interfaces und Klassen erzeugen
        // Weiterhin kinder der klassen checken, aber auch direkte kinder von interfaces

        public static T createInstance<T>(Type type)
        {
            ;
            T finalInstance = default(T);
            object[] args;
            try { args = new object[type.GetConstructors()[0].GetParameters().Length];
                for (int i = 0; i < type.GetConstructors()[0].GetParameters().Length; i++)
                {
                    args[i] = null;
                }
                finalInstance = (T)Activator.CreateInstance(type, args);
            }
            catch
            {
                bool worked = false;
                int num = 0;
                do
                {
                    try
                    {
                        finalInstance = (T)Activator.CreateInstance(type);
                        worked = true;
                    }
                    catch (Exception)
                    {
                        num++;
                        args = new object[num];
                        for (int i = 0; i < num; i++)
                        {
                            args[i] = null;
                        }
                    }
                } while (!worked && num < 15);
            }

            return finalInstance;
        }

        private static bool checkInterfacesSame(Type interfaceType, Type baseType)
        {
            if (interfaceType.GetInterfaces().Length == baseType.GetInterfaces().Length)
            {
                foreach (Type intType in interfaceType.GetInterfaces())
                {
                    if (!baseType.GetInterfaces().Contains(intType))
                        return false;
                }
                return true;
            }
            return false;
        }

        private static bool checkDirectlyImplementsInterface(Type interfaceType, Type baseType)
        {
            if (interfaceType.GetInterfaces().Length + 1 == baseType.GetInterfaces().Length)
            {
                foreach (Type intType in baseType.GetInterfaces())
                {
                    if (!interfaceType.GetInterfaces().Contains(intType))
                        if (!intType.Equals(interfaceType))
                            return false;
                }
                return true;
            }
            return false;
        }
    }


}
