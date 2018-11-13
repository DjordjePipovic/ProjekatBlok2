using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class CustomPrincipal : IPrincipal
    {
        WindowsIdentity wId;
        List<EnumRoles> eRoles = new List<EnumRoles>();
        List<EnumPrms> ePrms = new List<EnumPrms>();

        public CustomPrincipal(WindowsIdentity windowsIdentity)
        {
            this.wId = windowsIdentity;

            foreach (var groupId in this.wId.Groups)
            {
                string group = groupId.Translate(typeof(NTAccount)).ToString();
                string[] split = group.Split('\\');

                if (split[split.Length - 1] == "Readers")
                {
                    this.eRoles.Add(EnumRoles.Readers);
                    this.ePrms.Add(EnumPrms.Read);
                }
                else if (split[split.Length - 1] == "Operators")
                {
                    this.eRoles.Add(EnumRoles.Operators);
                    this.ePrms.Add(EnumPrms.Update);
                }
                else if (split[split.Length - 1] == "Admins")
                {
                    this.eRoles.Add(EnumRoles.Admins);
                    this.ePrms.Add(EnumPrms.Delete);
                }
            }
        }

        public IIdentity Identity => throw new NotImplementedException();

        public bool IsInRole(string role)
        {
            Enum.TryParse(role, out EnumPrms myStatus);

            if (this.ePrms.Contains(myStatus))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public WindowsIdentity wID
        {
            get { return this.wId; }
            set { this.wId = value; }
        }

        public List<EnumRoles> ERoles
        {
            get { return this.eRoles; }
            set { this.eRoles = value; }
        }

        public List<EnumPrms> EPrms
        {
            get { return this.ePrms; }
            set { this.ePrms = value; }
        }
    }
}
