namespace threshold.Producers.Connections
{
    public interface IConnection
    {
        int OwnerPid { get; set; }
        int ExternalPort { get; set; }
        int LocalPort { get; set; }
        string ExternalAddress { get; set; }
        string LocalAddress { get; set; }
        string Protocol { get; set; }
        string State { get; set; }
        string OwnerExecutablePath { get; }
    }
}
