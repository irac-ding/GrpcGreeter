using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcGreeter
{
    public class CustomerManagerService : Manager.ManagerBase
    {
        private readonly ILogger<GreeterService> _logger;

        public CustomerManagerService(ILogger<GreeterService> logger)
        {
            this._logger = logger;
        }

        public async override Task<CustomerEntry> Add(NewCustomer request, ServerCallContext context)
        {
            return await Task.FromResult(new CustomerEntry()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DateAdded = Timestamp.FromDateTime(DateTime.UtcNow),
                Id = Guid.NewGuid().ToString()
            });
        }

        public async override Task<CustomerDeleted> Delete(DeleteCustomer request, ServerCallContext context)
        {
            return await Task.FromResult(new CustomerDeleted() { Id = request.Id, DateDeleted = Timestamp.FromDateTime(DateTime.UtcNow) });
        }
    }
}
