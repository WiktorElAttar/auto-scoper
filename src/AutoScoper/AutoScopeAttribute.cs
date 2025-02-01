namespace AutoScoper;

public static class AutoScopeAttribute
{
    public const string FileName = "AutoScopeAttribute.g.cs";

    public const string FullName = "AutoScoper.AutoScopeAttribute";

    public const string Attribute =
        """
        namespace AutoScoper
        {
            [System.AttributeUsage(System.AttributeTargets.Class)]
            public class AutoScopeAttribute : System.Attribute
            {
            }
        }
        """;
}

// namespace AutoScoper
// {
//     [System.AttributeUsage(System.AttributeTargets.Class)]
//     public class AutoScopeAttribute : System.Attribute
//     {
//     }
// }
