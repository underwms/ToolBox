using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Sharp.Primitives;

namespace SecurityFilter
{
   [DataContract]
   public class ModuleName : StringType
   {
      public ModuleName(string value = "") : base(value) { }
   }

   [DataContract]
   public class TypeName : StringType
   {
      public TypeName(string value = "") : base(value) { }
   }

   [DataContract]
   public class IsReadOnlyProperty : BoolType
   {
      public IsReadOnlyProperty(bool value = true) : base(value) { }
   }

   [DataContract]
   public class RoleName : StringType
   {
      public RoleName(string value = "") : base(value) { }
   }

   [DataContract]
   public class RoleDescription : StringType
   {
      public RoleDescription(string value = "") : base(value) { }
   }

   [DataContract]
   public class RoleActive : BoolType
   {
      public RoleActive(bool value = true) : base(value) { }
   }

   [DataContract]
   public class FactoryRoleID : GuidType
   {
      public FactoryRoleID(Guid value) : base(value) { }
   }
}
