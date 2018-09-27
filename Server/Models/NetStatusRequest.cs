namespace SwgAnh.Docker.Models
{
    public class NetStatusRequest : SoeBaseObject
    {
        public short ClientTickCount { get; set; }
        public int LastUpdate { get; set; }
        public int AvarageUpdate { get; set; }
        public int ShortestUpdate { get; set; }
        public int LongestUpdate { get; set; }
        public int LastServerUpdate { get; set; }
        public long PacketsSent { get; set; }
        public long PacketsRecived { get; set; }
    }
}
