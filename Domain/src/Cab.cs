namespace Domain
{
    public class Cab
    {
        public string Id { get; private set; }
        public string VehicleType { get; private set; }
        public CabType Type { get; private set; }
        
        public Cab(string id, string vehicleType, CabType type)
        {
            Id = id;
            VehicleType = vehicleType;
            Type = type;
        }
    }
}
