namespace AutomationPractice.Common;

public static class Conventions
{
    public static void Enforce<T>(T target, Predicate<T> condition,string message)
    {
        if(target is null)
            throw new ArgumentNullException("Argument cannot be null.");
        if (!condition.Invoke(target))
            throw new ConventionException($"[{target.GetFriendlyTypeName()}] Convention Error :{message}");
    }
}

public static class Gaurd
{
    public static void NotNull(object target)
    {
        if(target is null)
            throw new ArgumentNullException("Argument cannot be null.");
    }
    
    public static void Catch<T>(Action action) where T: Exception
    {
        try
        {
            action.Invoke();
        }
        catch (T)
        {
        }
    }
    public static TOut? Catch<T,TIn,TOut>(Func<TIn, TOut> action,TIn obj) where T: Exception
    {
        try
        {
            return action.Invoke(obj);
        }
        catch (T)
        {
            return default;
        }
    }

    
}