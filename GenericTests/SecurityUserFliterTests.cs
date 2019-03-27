using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using GenericTests.Fixtures;
//using SecurityFilter;
using Xunit;

namespace GenericTests
{
   public class SecurityUserFliterTests
   {
      [Fact]
      public void Filter_Uses_Role_If_Only_User_Is_Provided()
      {
         //arrange
         var userInformation = Tuple.Create(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups);
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserElevated },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule>()
         );
         var testRoles = new List<Role> { testRole1 };

         //act
         var actual = SecurityUserFliter.GetAllRolesUserBleongsTo(userInformation, testRoles).ToList();

         //assert
         Assert.True(actual.Count == testRoles.Count);
         Assert.Collection(actual,
            role => Assert.Contains(testRole1.Name.Value, role.Name.Value));
      }

      [Fact]
      public void Filter_Uses_Role_If_Only_Group_User_Is_In_Is_Provided()
      {
         //arrange
         var userInformation = Tuple.Create(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups);
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation>(),
            groups: new List<GroupAssociation> { SecurityUserFliterTestFixture.GroupElevated },
            securedModules: new List<SecuredModule>()
         );
         var testRoles = new List<Role> { testRole1 };

         //act
         var actual = SecurityUserFliter.GetAllRolesUserBleongsTo(userInformation, testRoles).ToList();

         //assert
         Assert.True(actual.Count == testRoles.Count);
         Assert.Collection(actual,
            role => Assert.Contains(testRole1.Name.Value, role.Name.Value));
      }

      [Fact]
      public void Filter_Uses_Role_Only_Of_Groups_User_Is_In()
      {
         //arrange
         var userInformation = Tuple.Create(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups);
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation>(),
            groups: new List<GroupAssociation> { SecurityUserFliterTestFixture.GroupElevated },
            securedModules: new List<SecuredModule>()
         );
         var testRole2 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole2"),
            description: new RoleDescription("TestRole2"),
            isActive: true,
            users: new List<UserAssociation>(),
            groups: new List<GroupAssociation> { SecurityUserFliterTestFixture.GroupNotIn },
            securedModules: new List<SecuredModule>()
         );
         var testRoles = new List<Role> { testRole1, testRole2 };

         //act
         var actual = SecurityUserFliter.GetAllRolesUserBleongsTo(userInformation, testRoles).ToList();

         //assert
         Assert.True(actual.Count == testRoles.Count - 1);
         Assert.Collection(actual,
            role => Assert.Contains(testRole1.Name.Value, role.Name.Value));
      }

      [Fact]
      public void Filter_Uses_Only_One_Role_If_Both_User_And_Group_User_Is_In_Role_Is_Provided()
      {
         //arrange
         var userInformation = Tuple.Create(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups);
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserElevated },
            groups: new List<GroupAssociation> { SecurityUserFliterTestFixture.GroupElevated },
            securedModules: new List<SecuredModule>()
         );
         var testRoles = new List<Role> { testRole1 };

         //act
         var actual = SecurityUserFliter.GetAllRolesUserBleongsTo(userInformation, testRoles).ToList();

         //assert
         Assert.True(actual.Count == testRoles.Count);
         Assert.Collection(actual,
            role => Assert.Contains(testRole1.Name.Value, role.Name.Value));
      }

      [Fact]
      public void Filter_Returns_Empty_Role_Collection_If_No_Roles_Can_Be_Found()
      {
         //arrange
         var userInformation = Tuple.Create(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups);
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserNotIn },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule>()
         );
         var testRole2 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole2"),
            description: new RoleDescription("TestRole2"),
            isActive: true,
            users: new List<UserAssociation>(),
            groups: new List<GroupAssociation> { SecurityUserFliterTestFixture.GroupNotIn },
            securedModules: new List<SecuredModule>()
         );
         var testRoles = new List<Role> { testRole1, testRole2 };

         //act
         var actual = SecurityUserFliter.GetAllRolesUserBleongsTo(userInformation, testRoles).ToList();

         //assert
         Assert.Empty(actual);
      }

      [Fact]
      public void Filter_Consolidates_Roles_User_And_Group_User_Is_In()
      {
         //arrange
         var userInformation = Tuple.Create(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups);
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserElevated },
            groups: new List<GroupAssociation> { SecurityUserFliterTestFixture.GroupElevated },
            securedModules: new List<SecuredModule>()
         );
         var testRole2 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole2"),
            description: new RoleDescription("TestRole2"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserElevated },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule>()
         );
         var testRoles = new List<Role> { testRole1, testRole2 };

         //act
         var actual = SecurityUserFliter.GetAllRolesUserBleongsTo(userInformation, testRoles).ToList();

         //assert
         Assert.True(actual.Count == testRoles.Count);
         Assert.Collection(actual,
            role => Assert.Contains(testRole1.Name.Value, role.Name.Value),
            role => Assert.Contains(testRole2.Name.Value, role.Name.Value));
      }

      [Fact]
      public async Task Filter_Ignores_Securables_Higher_Than_Highest_User_RoleLevel()
      {
         //arrange
         var classUnderTest = new SecurityUserFliter();
         var testModule = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommandElevated },
            new[] { SecurityUserFliterTestFixture.SecuredPropertyElevated }
         );
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule }
         );
         var testRoles = new List<Role> { testRole1 };

         //act
         var actual = (await classUnderTest.GetUserSpecificSecurityInformationAsync(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, new long[] { }, testRoles)).ToList();

         //assert
         Assert.Empty(actual.First().SecuredCommands);
         Assert.Empty(actual.First().SecuredProperties);
      }

      [Fact]
      public async Task Filter_Ignores_Securables_Higher_Than_Highest_Group_RoleLevel()
      {
         //arrange
         var classUnderTest = new SecurityUserFliter();
         var testModule = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommandElevated },
            new[] { SecurityUserFliterTestFixture.SecuredPropertyElevated }
         );
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation>(),
            groups: new List<GroupAssociation> { SecurityUserFliterTestFixture.GroupStandard },
            securedModules: new List<SecuredModule> { testModule }
         );
         var testRole2 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole2"),
            description: new RoleDescription("TestRole2"),
            isActive: true,
            users: new List<UserAssociation>(),
            groups: new List<GroupAssociation> { SecurityUserFliterTestFixture.GroupLimited },
            securedModules: new List<SecuredModule> { testModule }
         );
         var testRoles = new List<Role> { testRole1, testRole2 };

         //act
         var actual = (await classUnderTest.GetUserSpecificSecurityInformationAsync(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups, testRoles)).ToList();

         //assert
         Assert.Empty(actual.First().SecuredCommands);
         Assert.Empty(actual.First().SecuredProperties);
      }

      [Fact]
      public async Task Filter_Ignores_Securables_Higher_Than_Highest_RoleLevel_Found()
      {
         //arrange
         var classUnderTest = new SecurityUserFliter();
         var testModule = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommandElevated },
            new[] { SecurityUserFliterTestFixture.SecuredPropertyElevated }
         );
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation> { SecurityUserFliterTestFixture.GroupLimited },
            securedModules: new List<SecuredModule> { testModule }
         );
         var testRoles = new List<Role> { testRole1 };

         //act
         var actual = (await classUnderTest.GetUserSpecificSecurityInformationAsync(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups, testRoles)).ToList();

         //assert
         Assert.Empty(actual.First().SecuredCommands);
         Assert.Empty(actual.First().SecuredProperties);
      }

      [Fact]
      public async Task Filter_AccessPoints_Will_Be_At_Highest_AccessLevel_After_Role_And_Module_Aggregation()
      {
         //arange
         var classUnderTest = new SecurityUserFliter();
         var testModule = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Enabled,
            AccessState.Disabled,
            AccessState.Locked,
            new SecuredCommand[] { },
            new SecuredProperty[] { }
         );
         var testModule2 = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Locked,
            AccessState.Enabled,
            new SecuredCommand[] { },
            new SecuredProperty[] { }
         );
         var testModule3 = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Locked,
            AccessState.Enabled,
            AccessState.Disabled,
            new SecuredCommand[] { },
            new SecuredProperty[] { }
         );
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule, testModule2 }
         );
         var testRole2 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole2"),
            description: new RoleDescription("TestRole2"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule3 }
         );
         var testRoles = new List<Role> { testRole1, testRole2 };
         var highestAccessState = Enum.GetValues(typeof(AccessState)).Cast<AccessState>().Min(); //*note Min() is used due to the enumeration numbering of AccessState

         //act
         var actual = (await classUnderTest.GetUserSpecificSecurityInformationAsync(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups, testRoles)).ToList();

         //assert
         Assert.Equal(highestAccessState, actual.First().UserAccess);
         Assert.Equal(highestAccessState, actual.First().AdminAccess);
         Assert.Equal(highestAccessState, actual.First().BreakpointAccess);
      }

      [Fact]
      public async Task Filter_Collections_Return_Empty_If_No_Securables_Made_It_Past_Filtering()
      {
         //arange
         var classUnderTest = new SecurityUserFliter();
         var testModule = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new SecuredCommand[] { },
            new SecuredProperty[] { }
         );
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule }
         );
         var testRoles = new List<Role> { testRole1 };

         //act
         var actual = (await classUnderTest.GetUserSpecificSecurityInformationAsync(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups, testRoles)).ToList();

         //assert
         Assert.Empty(actual.First().SecuredCommands);
         Assert.Empty(actual.First().SecuredProperties);
      }

      [Fact]
      public async Task Filter_Collections_Returns_Single_Values_When_Duplicate_Securables_Enter_Filtering()
      {
         //arange
         var classUnderTest = new SecurityUserFliter();
         var testModule = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommandLimited },
            new[] { SecurityUserFliterTestFixture.SecuredPropertyLimited }
         );
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule }
         );
         var testRole2 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole2"),
            description: new RoleDescription("TestRole2"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule }
         );
         var testRoles = new List<Role> { testRole1, testRole2 };

         //act
         var actual = (await classUnderTest.GetUserSpecificSecurityInformationAsync(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups, testRoles)).ToList();

         //assert
         Assert.Single(actual.First().SecuredCommands, command => SecuredCommand.Comparer.Equals(command, SecurityUserFliterTestFixture.SecuredCommandLimited));
         Assert.Single(actual.First().SecuredProperties, property => SecuredProperty.Comparer.Equals(property, SecurityUserFliterTestFixture.SecuredPropertyLimited));
      }

      [Fact]
      public async Task Filter_Collections_Returns_Highest_Securables_When_Same_Type_Securables_Enter_Filtering()
      {
         //arange
         var classUnderTest = new SecurityUserFliter();
         var testModule = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommandLimited },
            new[] { SecurityUserFliterTestFixture.SecuredPropertyLimited, SecurityUserFliterTestFixture.SecuredProperty2Limited }
         );
         var testModule2 = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommandStandard },
            new[] { SecurityUserFliterTestFixture.SecuredPropertyStandard, SecurityUserFliterTestFixture.SecuredProperty2Standard }
         );
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule }
         );
         var testRole2 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole2"),
            description: new RoleDescription("TestRole2"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule2 }
         );
         var testRoles = new List<Role> { testRole1, testRole2 };

         //act
         var actual = (await classUnderTest.GetUserSpecificSecurityInformationAsync(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups, testRoles)).ToList();

         //assert
         Assert.Single(actual.First().SecuredCommands, command => SecuredCommand.Comparer.Equals(command, SecurityUserFliterTestFixture.SecuredCommandStandard));
         Assert.Single(actual.First().SecuredProperties, property => SecuredProperty.Comparer.Equals(property, SecurityUserFliterTestFixture.SecuredPropertyStandard));
         Assert.Single(actual.First().SecuredProperties, property => SecuredProperty.Comparer.Equals(property, SecurityUserFliterTestFixture.SecuredProperty2Standard));
      }

      [Fact]
      public async Task Filter_Returns_Highest_Highest_Securables_Module()
      {
         //arange
         var classUnderTest = new SecurityUserFliter();
         var testModule = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommandLimited },
            new[] { SecurityUserFliterTestFixture.SecuredPropertyLimited, SecurityUserFliterTestFixture.SecuredProperty2Limited }
         );
         var testModule2 = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommandStandard },
            new[] { SecurityUserFliterTestFixture.SecuredPropertyStandard, SecurityUserFliterTestFixture.SecuredProperty2Standard }
         );
         var testModule3 = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName2,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommand2Limited },
            new[] { SecurityUserFliterTestFixture.SecuredProperty3Limited, SecurityUserFliterTestFixture.SecuredProperty4Limited }
         );
         var testModule4 = new SecuredModule(
            SecurityUserFliterTestFixture.ModuleName2,
            AccessState.Disabled,
            AccessState.Disabled,
            AccessState.Disabled,
            new[] { SecurityUserFliterTestFixture.SecuredCommand2Standard },
            new[] { SecurityUserFliterTestFixture.SecuredProperty3Standard, SecurityUserFliterTestFixture.SecuredProperty4Standard }
         );
         var testRole1 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole1"),
            description: new RoleDescription("TestRole1"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule }
         );
         var testRole2 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole2"),
            description: new RoleDescription("TestRole2"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule2 }
         );
         var testRole3 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole3"),
            description: new RoleDescription("TestRole3"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule3 }
         );
         var testRole4 = new Role
         (
            identity: new RoleIdentity(SecurityUserFliterTestFixture.OrganizationIdentity, Guid.NewGuid()),
            factoryRoleIdentity: Guid.Empty,
            isFactoryRole: true,
            name: new RoleName("TestRole4"),
            description: new RoleDescription("TestRole4"),
            isActive: true,
            users: new List<UserAssociation> { SecurityUserFliterTestFixture.UserStandard },
            groups: new List<GroupAssociation>(),
            securedModules: new List<SecuredModule> { testModule4 }
         );
         var testRoles = new List<Role> { testRole1, testRole2, testRole3, testRole4 };

         //act
         var actual = (await classUnderTest.GetUserSpecificSecurityInformationAsync(SecurityUserFliterTestFixture.UserId, SecurityUserFliterTestFixture.OrgId, SecurityUserFliterTestFixture.Groups, testRoles)).ToList();

         //assert
         Assert.Single(actual.First(module => module.ModuleName.Equals(SecurityUserFliterTestFixture.ModuleName)).SecuredCommands, command => SecuredCommand.Comparer.Equals(command, SecurityUserFliterTestFixture.SecuredCommandStandard));
         Assert.Single(actual.First(module => module.ModuleName.Equals(SecurityUserFliterTestFixture.ModuleName)).SecuredProperties, property => SecuredProperty.Comparer.Equals(property, SecurityUserFliterTestFixture.SecuredPropertyStandard));
         Assert.Single(actual.First(module => module.ModuleName.Equals(SecurityUserFliterTestFixture.ModuleName)).SecuredProperties, property => SecuredProperty.Comparer.Equals(property, SecurityUserFliterTestFixture.SecuredProperty2Standard));

         Assert.Single(actual.First(module => module.ModuleName.Equals(SecurityUserFliterTestFixture.ModuleName2)).SecuredCommands, command => SecuredCommand.Comparer.Equals(command, SecurityUserFliterTestFixture.SecuredCommand2Standard));
         Assert.Single(actual.First(module => module.ModuleName.Equals(SecurityUserFliterTestFixture.ModuleName2)).SecuredProperties, property => SecuredProperty.Comparer.Equals(property, SecurityUserFliterTestFixture.SecuredProperty3Standard));
         Assert.Single(actual.First(module => module.ModuleName.Equals(SecurityUserFliterTestFixture.ModuleName2)).SecuredProperties, property => SecuredProperty.Comparer.Equals(property, SecurityUserFliterTestFixture.SecuredProperty4Standard));
      }
   }
}
