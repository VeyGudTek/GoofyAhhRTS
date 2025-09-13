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
            IEnumerable<FieldInfo> nullFieldsWithInitialization = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => 
                    f.GetCustomAttributes(typeof(InitializationRequiredAttribute)).Count() != 0 &&
                    f.GetValue(instance) == null
                );

            if (nullFieldsWithInitialization.Count() == 0)
            {
                return;
            }

            string errorMessage = $"[{t.Name}] is missing the following dependencies:\n";
            foreach (FieldInfo field in nullFieldsWithInitialization)
            {
                errorMessage += $"\t[{GetFieldTypeName(field.FieldType)}] {field.Name}\n";
            }

            throw new InitializationException(errorMessage);
        }

        private static string GetFieldTypeName(Type type)
        {
            if (type.IsSubclassOf(typeof(Delegate)))
            {
                return "CallBack";
            }
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                return "Script";
            }
            if (type == typeof(InputAction))
            {
                return "Input";
            }
            if (type.IsSubclassOf(typeof(Component)))
            {
                return "Component";
            }
            return $"Property({type.Name})";
        }
    }
}

