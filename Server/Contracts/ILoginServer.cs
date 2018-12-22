namespace SwgAnh.Docker.Contracts
{
    public interface ILoginServer
    {
        /// <summary>
        ///     Start Login Server for inncomming UDP packets
        /// </summary>
        void StartServer();

        void CloseServer();
    }
}