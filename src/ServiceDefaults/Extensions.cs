// using Microsoft.AspNetCore.Builder;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
//
// namespace ServiceDefaults;
//
// public static class Extensions
// {
//     public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
//     {
//         builder.Services.AddServiceDiscovery();
//         
//         builder.Services.ConfigureHttpClientDefaults(http =>
//         {
//             // Turn on resilience by default
//             http.AddStandardResilienceHandler();
//         
//             // Turn on service discovery by default
//             http.AddServiceDiscovery();
//         });
//
//         return builder;
//     }
// }