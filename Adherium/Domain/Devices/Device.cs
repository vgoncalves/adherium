namespace Adherium.Domain.Devices;

public class Device
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string SerialNumber { get; set; }
}
