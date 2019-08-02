using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADLookupPerfTest
{
    class Program
    {
        static AccountManagementService ams = new AccountManagementService();
        static DirectorySearcherService dss = new DirectorySearcherService();
        static WindowsIdentityService wis = new WindowsIdentityService();

        static void Main(string[] args)
        {
            var userId = ConfigurationManager.AppSettings["UsernameToFind"];
            var iterations = 50;
            var amsTimeTaken = new List<long>();
            var dssTimeTaken = new List<long>();
            var wisTimeTaken = new List<long>();

            for (var i = 0; i < iterations; i++)
            {
                amsTimeTaken.Add(CallAms(userId));
                dssTimeTaken.Add(CallDss(userId));
                wisTimeTaken.Add(CallWis(userId));
                Console.WriteLine($"iteration {i} done");
            }

            Console.WriteLine($"AMS average is {(amsTimeTaken.Sum() / iterations)}");
            Console.WriteLine($"DSS average is {(dssTimeTaken.Sum() / iterations)}");
            Console.WriteLine($"WIS average is {(wisTimeTaken.Sum() / iterations)}");

            Console.ReadKey();
        }

        static long CallAms(string userId)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ams.Lookup(userId);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
        static long CallDss(string userId)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            dss.Lookup(userId);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
        static long CallWis(string userId)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            wis.Lookup(userId);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
