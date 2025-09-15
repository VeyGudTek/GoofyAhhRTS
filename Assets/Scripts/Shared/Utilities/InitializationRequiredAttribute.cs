using System;

namespace Source.Shared.Utilities
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InitializationRequiredAttribute : Attribute
    {

    }
}
