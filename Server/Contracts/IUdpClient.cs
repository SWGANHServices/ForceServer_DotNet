namespace SwgAnh.Docker.Contracts
{
    public interface IUdpClient
    {
        /// <summary>
        ///     Must be called first
        /// </summary>
        byte[] Receive();

        void SendAsync(byte[] datagram, int bytes);
        void Close();
    }
}