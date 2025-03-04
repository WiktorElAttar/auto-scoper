﻿namespace AutoScoper;

public static class AutoScopeProviderHelper
{
    public const string GeneratedFileName = "AutoScopeProvider.g.cs";

    public const string Attribute =
        """
        // <auto-generated/>
        namespace AutoScoper;
        
        public static class AutoScopeProvider
        {
            public static global::System.IServiceProvider ServiceProvider { get; set; } = null!;
        }
        """;
}

// public static class AutoScopeProvider
// {
//     public static global::System.IServiceProvider ServiceProvider { get; set; } = null!;
// }

