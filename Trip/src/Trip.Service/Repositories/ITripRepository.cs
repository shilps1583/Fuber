namespace Trip.Service.Repositories
{
    public interface ITripRepository
    {
        Domain.Trip Save(Domain.Trip trip);
        Domain.Trip Get(string tripId);
    }
}
