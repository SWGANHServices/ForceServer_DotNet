namespace SwgAnh.Docker.Helper
{
    public static class GameTimeHelper
    {
        public static int ServerTick { get; set; }
        public static int ClientTick { get; set; }
        public static long ClientPacketSent { get; set; }
        public static long ClientPacketRecived { get; set; }
        public static int ServerPacketSent { get; set; }
        public static int ServerPacketRecived { get; set; }
    }
}