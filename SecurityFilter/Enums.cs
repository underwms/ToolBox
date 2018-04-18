using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityFilter
{
   public enum RoleLevel
   {
      Limited = 1,
      Standard = 2,
      Elevated = 3
   }

   public enum AccessState
   {
      Enabled = 1,
      Disabled = 2,
      Locked = 3
   }
}
