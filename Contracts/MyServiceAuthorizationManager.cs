using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class MyServiceAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            try
            {
                ServiceSecurityContext securityContext = operationContext.ServiceSecurityContext;
                WindowsIdentity callingIdentity = securityContext.WindowsIdentity;

                CustomPrincipal principal = new CustomPrincipal(callingIdentity);
                return principal.IsInRole("Readers");
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
