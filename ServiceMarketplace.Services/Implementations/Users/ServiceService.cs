using Microsoft.Extensions.Logging;
using ServiceMarketplace.Data;
using ServiceMarketplace.Services.Interfaces.Users;

namespace ServiceMarketplace.Services.Implementations.Users;

public class ServiceService : IServiceService
{
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(ApplicationContext applicationContext, ILogger<ServiceService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

}
