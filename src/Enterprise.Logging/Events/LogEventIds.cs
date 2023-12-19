using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Events;

public static class LogEventIds
{
    // EventIds are not required, but use if it helps
    // consider using "ranges" to isolate feature entries
    // this implies some forethought / organization with numbering
    // ex: 1000-1999 = "browsing products" feature
    // ex: 2000-2999 = "checking out" feature
    // this enables another way to query and filter log entries

    public static EventId UnknownError = new(0, "UnknownError");
    public static EventId CustomError = new(10, "CustomError");
}