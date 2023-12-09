When writing unit tests for API controllers, test isolation is important.
Avoid model binding (w/ validation), filters, routing, etc.
Using these things just means that the tests become integration tests between the framework components and the controllers