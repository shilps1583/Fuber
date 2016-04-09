using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Cab.Repositories
{
    public class CabsDatabase
    {
        static List<Domain.Cab> allCabs = new List<Domain.Cab>
            {
                new Domain.Cab("1", "Toyota Etios", CabType.Regular),
                new Domain.Cab("2", "Maruti Swift", CabType.Regular),
                new Domain.Cab("3", "Toyota Etios", CabType.Pink),
                new Domain.Cab("4", "Maruti Swift", CabType.Regular),
                new Domain.Cab("5", "Toyota Etios", CabType.Pink),
            };
        public static List<Domain.Cab> GetAllCabs()
        {
            return allCabs;
        }
    }
}
