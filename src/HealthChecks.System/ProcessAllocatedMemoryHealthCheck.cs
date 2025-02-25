using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.System
{
    public class ProcessAllocatedMemoryHealthCheck : IHealthCheck
    {
        private readonly int _maximumMegabytesAllocated;

        public ProcessAllocatedMemoryHealthCheck(int maximumMegabytesAllocated)
        {
            _maximumMegabytesAllocated = maximumMegabytesAllocated;
        }

        /// <inheritdoc />
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            long totalMemoryBytes = GC.GetTotalMemory(false) / 1024 / 1024;

            if (totalMemoryBytes >= _maximumMegabytesAllocated)
            {
                return Task.FromResult(new HealthCheckResult(
                    context.Registration.FailureStatus,
                    description: $"Allocated megabytes in memory beyond threshold: {totalMemoryBytes} mb / {_maximumMegabytesAllocated} mb"));
            }

            return Task.FromResult(HealthCheckResult.Healthy(description: $"Allocated megabytes in memory: {totalMemoryBytes} mb"));
        }
    }
}
