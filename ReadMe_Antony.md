# API Integration Technical Test


### Solution Notes ###

* I've implemented the solution using VS2022 and .Net Core 5 (just in case - VS2019 doesn't support .Net Core 6)
* I've also updated the class libraries to .Net 5.0 as this is effectively the new .Net Standard (https://devblogs.microsoft.com/dotnet/the-future-of-net-standard/)
* This allows me to use the newer C# 9 features and libraries - things have changed even more with .Net 6, C# 10.


### Console App Test Harness ###
* The app could be hosted in an AWS lambda or Azure Function but I've added a console app so you can view the log messages easily.


### HttpClientfactory Library ###

* I've used HttpClientfactory to create a named Http client for the external call. 
* HttpClientFactory resolves all of the httpclient issues found in this post: https://www.stevejgordon.co.uk/introduction-to-httpclientfactory-aspnetcore
* I've also added Polly to implement a retry policy on this HttpClient to improve endpoint resiliency.
