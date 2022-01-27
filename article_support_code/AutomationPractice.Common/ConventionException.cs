namespace AutomationPractice.Common;

[Serializable]
public class ConventionException : Exception
{
    public ConventionException() { }
    public ConventionException(string message) : base(message) { }
    public ConventionException(string message, Exception inner) : base(message, inner) { }
    protected ConventionException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}