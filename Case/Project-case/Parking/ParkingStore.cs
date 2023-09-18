using static Project_case.Parking.ParkingStore;

namespace Project_case.Parking
{
    public interface IParkingStore
    {
        Parking Get(string licensePlate);
        void Save(Parking Parking);
        void DeleteAll(string licensePlate);
    }

    public class ParkingStore : IParkingStore
    {
        private static readonly Dictionary<int, Parking> Database = new Dictionary<int, Parking>();
        private static int ParkingId = 0;

        public Parking Get(string licensePlate)
        {
            return Database.FirstOrDefault(x => x.Value.LicensePlate == licensePlate).Value;
        }

        public void Save(Parking parking)
        {
            Database[ParkingId++] = parking;
        }

        public void DeleteAll(string licensePlate)
        {
            foreach (var parking in Database.Where(p => p.Value.LicensePlate == licensePlate).ToList())
            {
                Database.Remove(parking.Key);
            }
        }
    }
}
