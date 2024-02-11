using GymManagement.Infrastructure.Common.Middleware;
using Microsoft.AspNetCore.Builder;

namespace GymManagement.Infrastructure;

/// <summary>
/// Request pipeline for the infrastructure layer
/// </summary>
public static class RequestPipeline
{
  public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder builder)
  {
      builder.UseMiddleware<EventualConsistencyMiddleware>();

      return builder;
  }
}
