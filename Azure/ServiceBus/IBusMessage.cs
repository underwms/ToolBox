using System;

namespace Azure.ServiceBus
{
    public interface IBusMessage
    {
        int MessageCategory { get; }
        
        int VersionMajor { get; }
        
        int VersionMinor { get; }

        DateTime EventTimeUtc { get; set; }
        
        string Tag { get; set; }
    }
}
