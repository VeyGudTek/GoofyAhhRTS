using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Shared.Utilities
{
    public record InitializeCheckDTO<T>
    {
        public string Name;
        public T Dependency;
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

        public static void CheckDelegates(string className, params InitializeCheckDTO<Delegate>[] dtos)
        {
            string errors = string.Empty;
            foreach (InitializeCheckDTO<Delegate> dto in dtos)
            {
                if (dto.Dependency == null)
                {
                    errors += $"[{className}] missing [{dto.Name}]\n";
                }
            }

            if (errors != string.Empty)
            {
                throw new MissingDelegateException(errors);
            }
        }

        public static void CheckMonoBehaviors(string className, params InitializeCheckDTO<MonoBehaviour>[] monoBehaviors)
        {
            string errors = string.Empty;
            foreach (InitializeCheckDTO<MonoBehaviour> monobehavior in monoBehaviors)
            {
                if (monobehavior.Dependency == null)
                {
                    errors += $"[{className}] missing [{monobehavior.Name}]\n";
                }
            }

            if (errors != string.Empty)
            {
                throw new MissingDependencyException(errors);
            }
        }
    }
}

