using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Shared.Utilities
{
    public static class InitializationChecker
    {
        public static void CheckInitializeRequired(this MonoBehaviour instance)
        {
            Type t = instance.GetType();
            IEnumerable<FieldInfo> fieldsWithInitialization = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttributes(typeof(InitializationRequiredAttribute)).Count() != 0);

            string errorMessage = string.Empty;
            foreach (FieldInfo field in fieldsWithInitialization)
            {
                if (field.FieldType.IsSubclassOf(typeof(Delegate)))
                {
                    errorMessage += CheckDependency(field.Name, (Delegate)field.GetValue(instance));
                    continue;
                }
                if(field.FieldType.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    errorMessage += CheckDependency(field.Name, (MonoBehaviour)field.GetValue(instance));
                    continue;
                }
                if(field.FieldType == typeof(InputAction))
                {
                    errorMessage += CheckDependency(field.Name, (InputAction)field.GetValue(instance));
                    continue;
                }
                if (field.FieldType.IsSubclassOf(typeof(Component)))
                {
                    errorMessage += CheckDependency(field.Name, (Component)field.GetValue(instance));
                    continue;
                }
            }

            if (errorMessage != string.Empty)
            {
                throw new InitializationException($"[Class] {t.Name} is missing the following dependencies: \n{errorMessage}");
            }
        }

        public static string CheckDependency(string fieldName, Component component)
        {
            if (component == null)
            {
                return $"\t[Component] {fieldName}\n";
            }
            return string.Empty;
        }

        public static string CheckDependency(string fieldName, Delegate callback)
        {
            if (callback == null)
            {
                return $"\t[Callback] {fieldName}\n";
            }
            return string.Empty;
        }

        public static string CheckDependency(string fieldName, MonoBehaviour monoBehavior)
        {
            if (monoBehavior == null)
            {
                return $"\t[Script] {fieldName}\n";
            }
            return string.Empty;
        }

        public static string CheckDependency(string fieldName, InputAction inputAction)
        {
            if (inputAction == null)
            {
                return $"\t[Input] {fieldName}\n";
            }
            return string.Empty;
        }
    }
}

