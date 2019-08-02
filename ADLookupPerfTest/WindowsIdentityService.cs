using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ADLookupPerfTest
{
    public class WindowsIdentityService
    {
        public List<string> Lookup(string userName)
        {
            List<string> result = new List<string>();
            WindowsIdentity wi = new WindowsIdentity(userName);

            foreach (IdentityReference group in wi.Groups)
            {
                try
                {
                    result.Add(group.Translate(typeof(NTAccount)).ToString().Replace("HIQ\\",""));
                }
                catch (Exception ex) { }
            }
            return result;
        }
    }
}
