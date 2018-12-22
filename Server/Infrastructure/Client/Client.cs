namespace SwgAnh.Docker.Infrastructure.Client
{
    public class Client
    {
        private readonly int _clientId;

        public Client(int clientId) => _clientId = clientId;

        public int ClientTickSent { get; set; }
        public int ClientTickRecived { get; set; }
    }
}