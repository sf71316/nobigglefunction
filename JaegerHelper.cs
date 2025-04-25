namespace yournamespace
{
   internal static class JaegerHelper
{
    public static Activity StartLogActivity(ActivitySource activitySource, HttpRequestHeaders headers,
     [CallerMemberName] string methodName = "", bool isRoot = false)
{
    if (headers != null && isRoot)
    {
        PropagationContext parentContext = Propagators.DefaultTextMapPropagator.Extract(default(PropagationContext), headers, extract);
        Baggage.Current = parentContext.Baggage;
        var activity = activitySource.StartActivity(methodName, ActivityKind.Internal,
            parentContext.ActivityContext);
        activity.TraceStateString = parentContext.ActivityContext.TraceState;
        return activity;

    }
    else
    {
        return activitySource.StartActivity(methodName);
    }
}
    private static IEnumerable<string> extract(IEnumerable<KeyValuePair<string, IEnumerable<string>>> arg1, string arg2)
    {
        var rs = arg1.FirstOrDefault(p => p.Key == arg2);
        if (!rs.Equals(default(KeyValuePair<string, IEnumerable<string>>)))
        {
            return rs.Value;
        }
        return null;
    }
}
}
