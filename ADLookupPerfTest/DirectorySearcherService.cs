using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Configuration;
using System.Security.Principal;
using System.DirectoryServices.ActiveDirectory;

namespace ADLookupPerfTest
{
    public class DirectorySearcherService
    {

        public List<string> Lookup(string userId)
        {
            List<string> userNestedMembership = new List<string>();
            
            DirectoryEntry domainConnection = new DirectoryEntry();
            domainConnection.Path = ConfigurationManager.AppSettings["DssDomainPath"];

            DirectorySearcher samSearcher = new DirectorySearcher();
            
            samSearcher.SearchRoot = domainConnection;
            samSearcher.Filter = "(samAccountName=" + userId + ")";
            samSearcher.PropertiesToLoad.Add("displayName");

            SearchResult samResult = samSearcher.FindOne();

            if (samResult != null)
            {
                DirectoryEntry theUser = samResult.GetDirectoryEntry();
                theUser.RefreshCache(new string[] { "tokenGroups" });

                foreach (byte[] resultBytes in theUser.Properties["tokenGroups"])
                {
                    System.Security.Principal.SecurityIdentifier mySID = new System.Security.Principal.SecurityIdentifier(resultBytes, 0);

                    DirectorySearcher sidSearcher = new DirectorySearcher();

                    sidSearcher.SearchRoot = domainConnection;
                    sidSearcher.Filter = "(objectSid=" + mySID.Value + ")";
                    sidSearcher.PropertiesToLoad.Add("distinguishedName");
                    sidSearcher.PropertiesToLoad.Add("name");

                    SearchResult sidResult = sidSearcher.FindOne();

                    if (sidResult != null)
                    {
                        userNestedMembership.Add((string)sidResult.Properties["name"][0]);
                    }
                }
                
            }
            else
            {
                Console.WriteLine("The user doesn't exist");
            }

            return userNestedMembership;

        }

    }
}
