using System;
using UnityEngine;

namespace Source.Shared.Utilities
{
    public abstract class InitializationRequiredMonoBehavior<M> : MonoBehaviour where M : class
    {
        protected bool Initialized = false;

        protected void CheckInitialized()
        {
            if (!Initialized)
            {
                throw new InitializationException($"{this.GetType().Name} Not Initialized.");
            }
        }

        public abstract void Initialize(M callbacks);
    }
}
