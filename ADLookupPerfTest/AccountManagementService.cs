using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.Configuration;

namespace ADLookupPerfTest
{
    public class AccountManagementService
    {
        string _domain = ConfigurationManager.AppSettings["AmsDomain"];
        public List<string> Lookup(string userId)
        {
            var output = new List<string>();

            using (var ctx = new PrincipalContext(ContextType.Domain, _domain))
            {
                using (var user = UserPrincipal.FindByIdentity(ctx, userId))
                {
                    if (user != null)
                    {
                        output = user.GetAuthorizationGroups() //this returns a collection of principal objects
                            .Select(x => x.SamAccountName)
                            .ToList();
                    }
                }
            }
            return output;
        }
    }
}
