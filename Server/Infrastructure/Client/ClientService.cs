using System.Collections.Generic;

namespace SwgAnh.Docker.Infrastructure.Client
{
    public class ClientService
    {
        private IDictionary<int, Client> ClientRepository { get; set; }
        public Client GetClient(int clientId) => ClientRepository[clientId];
    }
}