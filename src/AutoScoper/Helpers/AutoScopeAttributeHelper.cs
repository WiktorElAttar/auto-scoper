﻿namespace AutoScoper
{
    public static class AutoScopeAttributeHelper
    {
        public const string GeneratedFileName = "AutoScopeAttribute.g.cs";

        public const string FullName = "AutoScoper.AutoScopeAttribute";

        public const string Attribute =
"""
// <auto-generated/>
namespace AutoScoper
{
    [System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AutoScopeAttribute : global::System.Attribute
    {
        public AutoScopeAttribute(params global::System.Type[] interfacesType)
        {
            InterfacesType = interfacesType;
        }

        public global::System.Type[] InterfacesType { get; }
    }
}
""";
    }
}

// namespace AutoScoper
// {
//     [System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
//     public sealed class AutoScopeAttribute : global::System.Attribute
//     {
//         public AutoScopeAttribute(params global::System.Type[] interfacesType)
//         {
//             InterfacesType = interfacesType;
//         }
//
//         public global::System.Type[] InterfacesType { get; }
//     }
// }
