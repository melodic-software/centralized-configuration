﻿using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Configuration.API.Tests.IntegrationTests.Loggers.Generic;

internal sealed class XUnitLogger<T> : XUnitLogger, ILogger<T>
{
    public XUnitLogger(ITestOutputHelper testOutputHelper, LoggerExternalScopeProvider scopeProvider)
        : base(testOutputHelper, scopeProvider, typeof(T).FullName)
    {
    }
}