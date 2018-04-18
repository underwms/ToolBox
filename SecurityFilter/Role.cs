using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using Newtonsoft.Json;

namespace SecurityFilter
{


   [DataContract]
   public class Role
   {
      public Role(
         RoleIdentity identity,
         Guid factoryRoleIdentity,
         bool isFactoryRole,
         RoleName name,
         RoleDescription description,
         bool isActive,
         List<UserAssociation> users,
         List<GroupAssociation> groups,
         List<SecuredModule> securedModules)
      {
         Identity = identity;
         FactoryRoleIdentity = factoryRoleIdentity;
         IsFactoryRole = isFactoryRole;
         Name = name;
         Description = description;
         IsActive = isActive;
         Users = users;
         Groups = groups;
         SecuredModules = securedModules;
      }

      [DataMember] public readonly RoleIdentity Identity;
      [DataMember] public readonly Guid FactoryRoleIdentity;
      [DataMember] public readonly bool IsFactoryRole;
      [DataMember] public readonly RoleName Name;
      [DataMember] public readonly RoleDescription Description;
      [DataMember] public readonly bool IsActive;
      [DataMember] public readonly List<UserAssociation> Users;
      [DataMember] public readonly List<GroupAssociation> Groups;
      [DataMember] public readonly List<SecuredModule> SecuredModules;

      public static Role Default(EntityIdentity identity)
      {
         return new Role(
            RoleIdentity.Create(identity),
            Guid.Empty,
            false,
            new RoleName(""),
            new RoleDescription(""),
            true,
            new List<UserAssociation>(),
            new List<GroupAssociation>(),
            new List<SecuredModule>());
      }
   }

   [DataContract]
   public class UserAssociation
   {
      [JsonConstructor]
      public UserAssociation(long userId, RoleLevel roleLevel)
      {
         UserId = userId;
         RoleLevel = roleLevel;
      }

      [DataMember] public readonly long UserId;
      [DataMember] public readonly RoleLevel RoleLevel;

      public static readonly IEqualityComparer<UserAssociation> Comparer = new UserAssociationEqualityComparer();

      public static UserAssociation Create(long userId, RoleLevel roleLevel) =>
         new UserAssociation(userId, roleLevel);
   }

   public class UserAssociationEqualityComparer : IEqualityComparer<UserAssociation>
   {
      public bool Equals(UserAssociation x, UserAssociation y)
      {
         return x.UserId == y.UserId && x.RoleLevel == y.RoleLevel;
      }

      public int GetHashCode(UserAssociation obj)
      {
         return Hash.Hash32(obj.UserId, obj.RoleLevel);
      }
   }

   [DataContract]
   public class GroupAssociation
   {
      [JsonConstructor]
      public GroupAssociation(long groupId, RoleLevel roleLevel)
      {
         GroupId = groupId;
         RoleLevel = roleLevel;
      }

      [DataMember] public readonly long GroupId;
      [DataMember] public readonly RoleLevel RoleLevel;

      public static readonly IEqualityComparer<GroupAssociation> Comparer = new GroupAssociationEqualityComparer();

      public static GroupAssociation Create(long groupId, RoleLevel roleLevel) =>
         new GroupAssociation(groupId, roleLevel);
   }
   public class GroupAssociationEqualityComparer : IEqualityComparer<GroupAssociation>
   {
      public bool Equals(GroupAssociation x, GroupAssociation y)
      {
         return x.GroupId == y.GroupId && x.RoleLevel == y.RoleLevel;
      }

      public int GetHashCode(GroupAssociation obj)
      {
         return Hash.Hash32(obj.GroupId, obj.RoleLevel);
      }
   }
}
