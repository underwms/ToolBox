using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace SecurityFilter
{
   [DataContract]
   public class SecuredModule
   {
      [JsonConstructor]
      public SecuredModule(
         ModuleName moduleName,
         AccessState userAccess,
         AccessState adminAccess,
         AccessState breakpointAccess,
         SecuredCommand[] securedCommands,
         SecuredProperty[] securedProperties)
      {
         ModuleName = moduleName;
         UserAccess = userAccess;
         AdminAccess = adminAccess;
         BreakpointAccess = breakpointAccess;
         SecuredCommands = securedCommands;
         SecuredProperties = securedProperties;
      }

      [DataMember] public readonly ModuleName ModuleName;
      [DataMember] public readonly AccessState UserAccess;
      [DataMember] public readonly AccessState AdminAccess;
      [DataMember] public readonly AccessState BreakpointAccess;
      [DataMember] public readonly SecuredCommand[] SecuredCommands;
      [DataMember] public readonly SecuredProperty[] SecuredProperties;

      public static SecuredModule Default(ModuleName moduleName)
      {
         return new SecuredModule(
            moduleName
            , AccessState.Disabled
            , AccessState.Disabled
            , AccessState.Disabled
            , new SecuredCommand[] { }
            , new SecuredProperty[] { });
      }

      public static SecuredModule Create(
         ModuleName moduleName
         , AccessState userAccess
         , AccessState adminAccess
         , AccessState breakpointAccess
         , SecuredCommand[] whitelistCommands
         , SecuredProperty[] whitelistProperties) =>
         new SecuredModule(
            moduleName
            , userAccess
            , adminAccess
            , breakpointAccess
            , whitelistCommands
            , whitelistProperties);
   }

   [DataContract]
   public class SecuredCommand
   {
      [JsonConstructor]
      internal SecuredCommand(TypeName commandType, RoleLevel roleLevel)
      {
         CommandType = commandType;
         RoleLevel = roleLevel;
      }

      [DataMember] public readonly TypeName CommandType;
      [DataMember] public readonly RoleLevel RoleLevel;

      public static readonly IEqualityComparer<SecuredCommand> Comparer = new SecuredCommandEqualityComparer();

      public static SecuredCommand Create(TypeName commandType, RoleLevel roleLevel) =>
         new SecuredCommand(commandType, roleLevel);
   }
   public class SecuredCommandEqualityComparer : IEqualityComparer<SecuredCommand>
   {
      public bool Equals(SecuredCommand x, SecuredCommand y) =>
         x.CommandType.Equals(y.CommandType) &&
         x.RoleLevel == y.RoleLevel;

      public int GetHashCode(SecuredCommand obj) =>
         Hash.Hash32(obj.CommandType, obj.RoleLevel);
   }

   [DataContract]
   public class SecuredProperty
   {
      [JsonConstructor]
      internal SecuredProperty(TypeName aggregateType, TypeName propertyType, RoleLevel roleLevel, IsReadOnlyProperty isReadOnlyProperty)
      {
         AggregateType = aggregateType;
         PropertyType = propertyType;
         RoleLevel = roleLevel;
         IsReadOnlyProperty = isReadOnlyProperty;
      }

      [DataMember] public readonly TypeName AggregateType;
      [DataMember] public readonly TypeName PropertyType;
      [DataMember] public readonly RoleLevel RoleLevel;
      [DataMember] public readonly IsReadOnlyProperty IsReadOnlyProperty;

      public static readonly IEqualityComparer<SecuredProperty> Comparer = new SecuredPropertyEqualityComparer();

      public static SecuredProperty Create(TypeName aggregateType, TypeName propertyType, RoleLevel roleLevel, IsReadOnlyProperty isReadOnlyProperty) =>
         new SecuredProperty(aggregateType, propertyType, roleLevel, isReadOnlyProperty);
   }
   public class SecuredPropertyEqualityComparer : IEqualityComparer<SecuredProperty>
   {
      public bool Equals(SecuredProperty x, SecuredProperty y) =>
         x.AggregateType.Equals(y.AggregateType) &&
         x.PropertyType.Equals(y.PropertyType) &&
         x.RoleLevel == y.RoleLevel;

      public int GetHashCode(SecuredProperty obj) =>
         Hash.Hash32(obj.AggregateType, obj.PropertyType, obj.RoleLevel);
   }
}
