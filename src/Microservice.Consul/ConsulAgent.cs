using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microservice.Consul
{
    public class ConsulAgent : IHostedService
    {
        private readonly IConsulClient _client;
        private readonly ILogger<ConsulAgent> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ConsulConfig _config;

        public ConsulAgent(IConsulClient client, IOptions<ConsulConfig> config, ILogger<ConsulAgent> logger,
            IHostingEnvironment hostingEnvironment)
        {
            _client = client;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _config = config.Value;
            if (string.IsNullOrEmpty(_config.ID))
            {
                _config.ID = string.Join("-", _config.Name, _config.Address, _config.Port);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var id = _config.ID;

            var tags = _config.Tags ?? new List<string>();
            if (_hostingEnvironment.IsDevelopment())
            {
                tags.Add(Environment.MachineName);
            }

            await _client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = id,
                Name = _config.Name,
                Address = _config.Address,
                Port = _config.Port,
                Tags = tags.ToArray(),
            }, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.Agent.ServiceDeregister(_config.ID, cancellationToken);
        }
    }
}