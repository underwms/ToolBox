using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityFilter
{
   public class SecurityUserFliter
   {
      public async Task<IEnumerable<SecuredModule>> GetUserSpecificSecurityInformationAsync(long userId, long orgId, long[] groups, IEnumerable<Role> organizationsRoles) =>
         await Task.Run(() => GetUserSpecificSecurityInformation(Tuple.Create(userId, orgId, groups), organizationsRoles));

      private static IEnumerable<SecuredModule> GetUserSpecificSecurityInformation(Tuple<long, long, long[]> userInformation, IEnumerable<Role> organizationsRoles)
      {
         var usersRoles = GetAllRolesUserBleongsTo(userInformation, organizationsRoles).ToList();

         // if no roles found, kick out
         if (!usersRoles.Any()) { return new List<SecuredModule>(); }

         var filteredModules = FilterSecurables(userInformation, usersRoles);

         var modulesSecurityInformation = AggregateModuleInformation(filteredModules);

         return FilterModuleInformation(modulesSecurityInformation);
      }

      public static IEnumerable<Role> GetAllRolesUserBleongsTo(Tuple<long, long, long[]> userInformation, IEnumerable<Role> organizationsRoles)
      {
         var enumerableList = organizationsRoles.ToList();
         var rolesUserBelongsIn = enumerableList.Where(role => role.Users.Any(user => user.UserId == userInformation.Item1)).ToList();
         var rolesGroupsBelongsIn = enumerableList.Where(role => role.Groups.Any(group => userInformation.Item3.Any(id => id == group.GroupId))).ToList();

         //union results with no duplicates
         return rolesUserBelongsIn.Concat(
            rolesGroupsBelongsIn.Where(groupRole =>
               !rolesUserBelongsIn.Any(userRole =>
                  RoleIdentity.Comparer.Equals(userRole.Identity, groupRole.Identity))));
      }

      private static IEnumerable<SecuredModule> FilterSecurables(Tuple<long, long, long[]> userInformation, IEnumerable<Role> usersRoles)
      {
         var allModules = new List<SecuredModule>();
         foreach (var role in usersRoles)
         {
            //get the highest RoleLevel
            var groupRoleLevel = role.Groups
               .Where(group => userInformation.Item3.Any(userGroup => userGroup == group.GroupId))
               .DefaultIfEmpty()
               .Max(group => ReferenceEquals(null, group) ? -1 : (int)group.RoleLevel);
            var usersRoleLevel = role.Users
               .Where(user => user.UserId == userInformation.Item1)
               .DefaultIfEmpty()
               .Max(user => ReferenceEquals(null, user) ? -1 : (int)user.RoleLevel);
            var highestRoleLevel = usersRoleLevel >= groupRoleLevel ? usersRoleLevel : groupRoleLevel;

            role.SecuredModules.ForEach(module =>
            {
               //since this a white list anything at or below a user's level, they can do/see
               var filteredCommandList = module.SecuredCommands.Where(m => (int)m.RoleLevel <= highestRoleLevel);
               var filteredPropertyList = module.SecuredProperties.Where(m => (int)m.RoleLevel <= highestRoleLevel);

               allModules.Add(SecuredModule.Create(
                  module.ModuleName,
                  module.UserAccess,
                  module.AdminAccess,
                  module.BreakpointAccess,
                  filteredCommandList.ToArray(),
                  filteredPropertyList.ToArray()
                  ));
            });
         }

         return allModules;
      }

      private static Dictionary<string, SecuredModule> AggregateModuleInformation(IEnumerable<SecuredModule> modules)
      {
         var whiteLists = new Dictionary<string, SecuredModule>();
         foreach (var module in modules)
         {
            var key = module.ModuleName.Value;
            if (whiteLists.ContainsKey(key))
            {
               //combine white lists
               var aggregatedCommandWhitelist = whiteLists[key].SecuredCommands.Concat(module.SecuredCommands);
               var aggregatedPropertyWhitelist = whiteLists[key].SecuredProperties.Concat(module.SecuredProperties);

               //highest access level wins *note the inequality sign is tied directly to the enumeration numbering of AccessState
               var newUserAccessLevel = (int)module.UserAccess < (int)whiteLists[key].UserAccess ? module.UserAccess : whiteLists[key].UserAccess;
               var newAdminAccessLevel = (int)module.AdminAccess < (int)whiteLists[key].AdminAccess ? module.AdminAccess : whiteLists[key].AdminAccess;
               var newBreakpointAccessLevel = (int)module.BreakpointAccess < (int)whiteLists[key].BreakpointAccess ? module.BreakpointAccess : whiteLists[key].BreakpointAccess;

               whiteLists[key] = SecuredModule.Create(
                  new ModuleName(key),
                  newUserAccessLevel,
                  newAdminAccessLevel,
                  newBreakpointAccessLevel,
                  aggregatedCommandWhitelist.ToArray(),
                  aggregatedPropertyWhitelist.ToArray());
            }
            else
            { whiteLists.Add(key, module); }
         }

         return whiteLists;
      }

      private static IEnumerable<SecuredModule> FilterModuleInformation(Dictionary<string, SecuredModule> modulesSecurityInformation)
      {
         var filteredSecurityInformation = new List<SecuredModule>();
         foreach (var module in modulesSecurityInformation)
         {
            //sort and group
            var filteredCommands = ExtractCommandsHighestPermissions(
               module.Value.SecuredCommands
                  .OrderByDescending(command => (int)command.RoleLevel)
                  .ThenBy(command => command.CommandType.Value)
                  .GroupBy(command => new { command.CommandType.Value, level = (int)command.RoleLevel })
                  .Select(newGrouping => newGrouping.First()));

            var filteredProperties = ExtractPropertiesHighestVisibility(
               module.Value.SecuredProperties
                  .OrderByDescending(property => (int)property.RoleLevel)
                  .ThenBy(property => property.PropertyType.Value)
                  .GroupBy(property => new { property.PropertyType.Value, level = (int)property.RoleLevel })
                  .Select(newGrouping => newGrouping.First()));

            filteredSecurityInformation.Add(
               SecuredModule.Create(
                  module.Value.ModuleName,
                  module.Value.UserAccess,
                  module.Value.AdminAccess,
                  module.Value.BreakpointAccess,
                  filteredCommands,
                  filteredProperties));
         }

         return filteredSecurityInformation;
      }

      private static SecuredCommand[] ExtractCommandsHighestPermissions(IEnumerable<SecuredCommand> filteredCommands)
      {
         var enumerableList = filteredCommands.ToList();

         // if nothing made it past the previous filters, kick out
         if (!enumerableList.Any()) { return new SecuredCommand[] { }; }

         var commandName = enumerableList.First().CommandType.Value;
         var highestPermission = (int)enumerableList.First().RoleLevel;
         var newCommands = new List<SecuredCommand>();

         enumerableList.ForEach(command =>
         {
            //a change in command type indicates new securable
            if (!commandName.Equals(command.CommandType.Value))
            {
               newCommands.Add(SecuredCommand.Create(new TypeName(commandName), (RoleLevel)highestPermission));
               commandName = command.CommandType.Value;
               highestPermission = (int)command.RoleLevel;
            }
            else
            { highestPermission = (int)command.RoleLevel < highestPermission ? highestPermission : (int)command.RoleLevel; }
         });

         //check the last one
         var valueToCompareAgainst = newCommands.FirstOrDefault(command => command.CommandType.Value.Equals(commandName));
         if (ReferenceEquals(null, valueToCompareAgainst))
         { newCommands.Add(SecuredCommand.Create(new TypeName(commandName), (RoleLevel)highestPermission)); }
         else
         {
            if (highestPermission > (int)valueToCompareAgainst.RoleLevel)
            { newCommands.Add(SecuredCommand.Create(new TypeName(commandName), (RoleLevel)highestPermission)); }
         }

         return newCommands.ToArray();
      }

      private static SecuredProperty[] ExtractPropertiesHighestVisibility(IEnumerable<SecuredProperty> filteredProperties)
      {
         var enumerableList = filteredProperties.ToList();

         // if nothing made it past the previous filters, kick out
         if (!enumerableList.Any()) { return new SecuredProperty[] { }; }

         var aggregateName = enumerableList.First().AggregateType.Value;
         var propertyName = enumerableList.First().PropertyType.Value;
         var highestVisibility = (int)enumerableList.First().RoleLevel;
         var isReadOnlyProperty = enumerableList.First().IsReadOnlyProperty.Value;
         var newProperties = new List<SecuredProperty>();

         enumerableList.ForEach(property =>
         {
            //a change in aggregate type indicates new securable
            if (!aggregateName.Equals(property.AggregateType.Value))
            {
               newProperties.Add(SecuredProperty.Create(new TypeName(aggregateName), new TypeName(propertyName), (RoleLevel)highestVisibility, new IsReadOnlyProperty(isReadOnlyProperty)));
               aggregateName = property.AggregateType.Value;
               propertyName = property.PropertyType.Value;
               highestVisibility = (int)property.RoleLevel;
               isReadOnlyProperty = property.IsReadOnlyProperty.Value;
            }
            else
            {
               //a change in property type indicates new securable
               if (!propertyName.Equals(property.PropertyType.Value))
               {
                  newProperties.Add(SecuredProperty.Create(new TypeName(aggregateName), new TypeName(propertyName), (RoleLevel)highestVisibility, new IsReadOnlyProperty(isReadOnlyProperty)));
                  aggregateName = property.AggregateType.Value;
                  propertyName = property.PropertyType.Value;
                  highestVisibility = (int)property.RoleLevel;
                  isReadOnlyProperty = property.IsReadOnlyProperty.Value;
               }
               else
               { highestVisibility = (int)property.RoleLevel < highestVisibility ? highestVisibility : (int)property.RoleLevel; }
            }
         });

         //check the last one
         var valueToCompareAgainst = newProperties.FirstOrDefault(property => property.AggregateType.Value.Equals(aggregateName) &&
                                                                              property.PropertyType.Value.Equals(propertyName));
         if (ReferenceEquals(null, valueToCompareAgainst))
         { newProperties.Add(SecuredProperty.Create(new TypeName(aggregateName), new TypeName(propertyName), (RoleLevel)highestVisibility, new IsReadOnlyProperty(isReadOnlyProperty))); }
         else
         {
            if (highestVisibility > (int)valueToCompareAgainst.RoleLevel)
            { newProperties.Add(SecuredProperty.Create(new TypeName(aggregateName), new TypeName(propertyName), (RoleLevel)highestVisibility, new IsReadOnlyProperty(isReadOnlyProperty))); }
         }

         return newProperties.ToArray();
      }
   }
}
