namespace AutomationPractice.Common;

public static class ReflectionExtensions
{
    public static string GetFriendlyTypeName(this object target)
    {
        var type = target as Type ?? target.GetType();
        var name = type.Name;
        return type.IsGenericType ? $"{name.ExtractPattern("\\w+")}<{string.Join(",", type.GetGenericArguments().Select(t => t.GetFriendlyTypeName()))}>" : name;
    } 
    
}