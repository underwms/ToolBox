using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using Newtonsoft.Json;

namespace SecurityFilter
{
   [DataContract]
   public class OrganizationIdentity
   {
      public static readonly IEqualityComparer<OrganizationIdentity> Comparer = new OrganizationIdentityComparer();

      public OrganizationIdentity(long organizationId)
      {
         OrganizationId = organizationId;
      }

      [DataMember]
      public readonly long OrganizationId;

      public string ConvertToString()
      {
         return OrganizationId.ToString();
      }

      public OrganizationIdentity ConvertFromString(string value)
      {
         return new OrganizationIdentity(long.Parse(value));
      }

      public static implicit operator OrganizationIdentity(long value)
      {
         return new OrganizationIdentity(value);
      }

      private class OrganizationIdentityComparer
         : IEqualityComparer<OrganizationIdentity>
      {
         public bool Equals(OrganizationIdentity x, OrganizationIdentity y)
         {
            return x.OrganizationId == y.OrganizationId;
         }

         public int GetHashCode(OrganizationIdentity x)
         {
            return x.OrganizationId.GetHashCode();
         }
      }

      public static OrganizationIdentity Create(long organizationId)
      {
         return new OrganizationIdentity(organizationId);
      }
      

      public static OrganizationIdentity Create(string organizationId)
      {
         return new OrganizationIdentity(long.Parse(organizationId));
      }
   }

   [DataContract]
   public class RoleIdentity
   {
      [JsonConstructor]
      public RoleIdentity(OrganizationIdentity organizationIdentity, Guid roleId)
      {
         OrganizationIdentity = organizationIdentity;
         RoleId = roleId == Guid.Empty ? Guid.NewGuid() : roleId;
      }

      [DataMember] public readonly OrganizationIdentity OrganizationIdentity;
      [DataMember] public readonly Guid RoleId;

      public static readonly IEqualityComparer<RoleIdentity> Comparer = new RoleIdentityEqualityComparer();
      
      public static RoleIdentity Create(OrganizationIdentity organization, Guid roleId)
      {
         return new RoleIdentity(
            organization,
            roleId);
      }

      public static RoleIdentity Create(long organizationId, Guid roleId)
      {
         return new RoleIdentity(
            OrganizationIdentity.Create(organizationId),
            roleId);
      }

      public static RoleIdentity Create(EntityIdentity identity)
      {
         return new RoleIdentity(new OrganizationIdentity(-1), Guid.Empty);
      }

      public override string ToString()
      {
         return JsonConvert.SerializeObject(this);
      }
   }
   public class RoleIdentityEqualityComparer : IEqualityComparer<RoleIdentity>
   {
      public bool Equals(RoleIdentity x, RoleIdentity y)
      {
         return OrganizationIdentity.Comparer.Equals(x.OrganizationIdentity, y.OrganizationIdentity) && x.RoleId.Equals(y.RoleId);
      }

      public int GetHashCode(RoleIdentity obj)
      {
         return OrganizationIdentity.Comparer.GetHashCode(obj.OrganizationIdentity) + obj.RoleId.GetHashCode();
      }
   }

   [DataContract]
   public class EntityIdentity
   {
      public static readonly EntityIdentityComparer Comparer = new EntityIdentityComparer();

      [JsonConstructor]
      internal EntityIdentity(string partitionKey, string id)
      {
         PartitionKey = partitionKey;
         Id = id;
      }

      [DataMember] public readonly string PartitionKey;
      [DataMember] public readonly string Id;

      public static EntityIdentity Create(string partitionKey, string id)
      {
         return new EntityIdentity(partitionKey, id);
      }
      
   }
   public class EntityIdentityComparer
      : IEqualityComparer<EntityIdentity>
   {
      public static readonly EntityIdentityComparer Instance = new EntityIdentityComparer();

      public bool Equals(EntityIdentity x, EntityIdentity y)
      {
         return x.PartitionKey == y.PartitionKey
                && x.Id == y.Id;
      }

      public int GetHashCode(EntityIdentity identity)
      {
         return Hash.Hash32(identity.PartitionKey, identity.Id);
      }
   }
   

}
