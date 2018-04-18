using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityFilter;

namespace GenericTests.Fixtures
{
   public static class SecurityUserFliterTestFixture
   {
      public const long UserId = 1;
      public const long OrgId = 1;
      public const long GroupId = 1;
      public static readonly long[] Groups = { GroupId };
      public static readonly OrganizationIdentity OrganizationIdentity = new OrganizationIdentity(OrgId);
      public static readonly UserAssociation UserLimited = new UserAssociation(UserId, RoleLevel.Limited);
      public static readonly UserAssociation UserStandard = new UserAssociation(UserId, RoleLevel.Standard);
      public static readonly UserAssociation UserElevated = new UserAssociation(UserId, RoleLevel.Elevated);
      public static readonly UserAssociation UserNotIn = new UserAssociation(-1, RoleLevel.Elevated);
      public static readonly GroupAssociation GroupLimited = new GroupAssociation(GroupId, RoleLevel.Limited);
      public static readonly GroupAssociation GroupStandard = new GroupAssociation(GroupId, RoleLevel.Standard);
      public static readonly GroupAssociation GroupElevated = new GroupAssociation(GroupId, RoleLevel.Elevated);
      public static readonly GroupAssociation GroupNotIn = new GroupAssociation(-1, RoleLevel.Elevated);
      public static readonly ModuleName ModuleName = new ModuleName("TestModule");
      public static readonly ModuleName ModuleName2 = new ModuleName("TestModule2");
      public static readonly SecuredCommand SecuredCommandLimited = SecuredCommand.Create(new TypeName("TestCommand"), RoleLevel.Limited);
      public static readonly SecuredCommand SecuredCommandStandard = SecuredCommand.Create(new TypeName("TestCommand"), RoleLevel.Standard);
      public static readonly SecuredCommand SecuredCommandElevated = SecuredCommand.Create(new TypeName("TestCommand"), RoleLevel.Elevated);
      public static readonly SecuredCommand SecuredCommand2Limited = SecuredCommand.Create(new TypeName("TestCommand2"), RoleLevel.Limited);
      public static readonly SecuredCommand SecuredCommand2Standard = SecuredCommand.Create(new TypeName("TestCommand2"), RoleLevel.Standard);
      public static readonly SecuredCommand SecuredCommand2Elevated = SecuredCommand.Create(new TypeName("TestCommand2"), RoleLevel.Elevated);
      public static readonly SecuredProperty SecuredPropertyLimited = SecuredProperty.Create(new TypeName("TestAggregate"), new TypeName("TestProperty"), RoleLevel.Limited, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredPropertyStandard = SecuredProperty.Create(new TypeName("TestAggregate"), new TypeName("TestProperty"), RoleLevel.Standard, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredPropertyElevated = SecuredProperty.Create(new TypeName("TestAggregate"), new TypeName("TestProperty"), RoleLevel.Elevated, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredProperty2Limited = SecuredProperty.Create(new TypeName("TestAggregate"), new TypeName("TestProperty2"), RoleLevel.Limited, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredProperty2Standard = SecuredProperty.Create(new TypeName("TestAggregate"), new TypeName("TestProperty2"), RoleLevel.Standard, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredProperty2Elevated = SecuredProperty.Create(new TypeName("TestAggregate"), new TypeName("TestProperty2"), RoleLevel.Elevated, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredProperty3Limited = SecuredProperty.Create(new TypeName("TestAggregate2"), new TypeName("TestProperty"), RoleLevel.Limited, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredProperty3Standard = SecuredProperty.Create(new TypeName("TestAggregate2"), new TypeName("TestProperty"), RoleLevel.Standard, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredProperty3Elevated = SecuredProperty.Create(new TypeName("TestAggregate2"), new TypeName("TestProperty"), RoleLevel.Elevated, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredProperty4Limited = SecuredProperty.Create(new TypeName("TestAggregate2"), new TypeName("TestProperty2"), RoleLevel.Limited, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredProperty4Standard = SecuredProperty.Create(new TypeName("TestAggregate2"), new TypeName("TestProperty2"), RoleLevel.Standard, new IsReadOnlyProperty());
      public static readonly SecuredProperty SecuredProperty4Elevated = SecuredProperty.Create(new TypeName("TestAggregate2"), new TypeName("TestProperty2"), RoleLevel.Elevated, new IsReadOnlyProperty());
   }
}
