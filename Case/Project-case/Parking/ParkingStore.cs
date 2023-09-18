using static Project_case.Parking.ParkingStore;
using System.Linq;
using CarTypeService.Models;

namespace Project_case.Parking
{
    public interface IParkingStore
    {
        List<Parking> GetAllParkings(string licensePlate);
        bool RegisterParking(Parking parking);
        void DeleteAll(string licensePlate);
        public Parking GetActiveParking(string licensePlate);
        bool EndParking(string licensePlate);

    }

    public class ParkingStore : IParkingStore
    {
        private static readonly Dictionary<int, Parking> Database = new Dictionary<int, Parking>();
        private static int ParkingId = 0;

        public List<Parking> GetAllParkings(string licensePlate)
        {
           return Database.Values.Where(p => p.LicensePlate == licensePlate).ToList();
        }

        public Parking GetActiveParking(string licensePlate)
        {
            return Database.FirstOrDefault(x => (x.Value.LicensePlate == licensePlate) && (x.Value.IsActive)).Value;
        }

        public bool RegisterParking(Parking parking)
        {
            if (GetActiveParking(parking.LicensePlate) == null) 
            { 
                Database[ParkingId++] = parking;
                return true;
            }
            else
                return false;
        }

        public bool EndParking(string licensePlate)
        {
            Parking parking = GetActiveParking(licensePlate);
            if (parking != null)
            {
                parking.EndParking();
                return true;
            }
            else
                return false;
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
