using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Shared.Utilities
{
    public record DelegateDTO
    {
        public string Name;
        public Delegate Delegate;
    }

    public static class InitializationChecker
    {
        public static void CheckComponents(string className, params Component[] components)
        {
            string errors = string.Empty;
            foreach (Component component in components)
            {
                if (component == null)
                {
                    errors += $"[{className}] missing [{component.GetType().Name}]\n";
                }
            }

            if (errors != string.Empty)
            {
                throw new MissingComponentException(errors);
            }
        }

        public static void CheckDelegates(string className, params DelegateDTO[] dtos)
        {
            string errors = string.Empty;
            foreach (DelegateDTO dto in dtos)
            {
                if (dto.Delegate == null)
                {
                    errors += $"[{className}] missing [{dto.Name}]\n";
                }
            }

            if (errors != string.Empty)
            {
                throw new MissingDelegateException(errors);
            }
        }

        public static void CheckDependencies(string className, params MonoBehaviour[] dependencies)
        {
            string errors = string.Empty;
            foreach (MonoBehaviour dependency in dependencies)
            {
                if (dependency == null)
                {
                    errors += $"[{className}] missing [{dependency.GetType().Name}]\n";
                }
            }

            if (errors != string.Empty)
            {
                throw new MissingDependencyException(errors);
            }
        }
    }
}

