using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeRuleEngine
{
    public class PrecompiledRules
    {
        public static List<Func<T, bool>> CompileRuleForList<T>(List<T> targetEntity, List<Rule> rules)
        {
            var compiledRules = new List<Func<T, bool>>();

            // Loop through the rules and compile them against the properties of the supplied shallow object 
            rules.ForEach(rule =>
            {
                var targetType = typeof(T);
                var properties = targetType.GetProperties();
                var specificProperty = properties.First(x => x.Name == rule.ComparisonPredicate);
                var propertyType = specificProperty.PropertyType;
                var genericType = Expression.Parameter(typeof(T));

                var key = Expression.Property(genericType, rule.ComparisonPredicate);
                var value = Expression.Constant(Convert.ChangeType(rule.ComparisonValue, propertyType));
                var binaryExpression = Expression.MakeBinary(rule.ComparisonOperator, key, value);

                compiledRules.Add(Expression.Lambda<Func<T, bool>>(binaryExpression, genericType).Compile());
            });

            // Return the compiled rules to the caller
            return compiledRules;
        }

        public static List<Func<T, bool>> CompileRuleForSingleObject<T>(T targetEntity, List<Rule> rules)
        {
            var compiledRules = new List<Func<T, bool>>();

            rules.ForEach(rule =>
            {
                var targetType = typeof(T);
                var properties = targetType.GetProperties();
                var specificProperty = properties.First(x => x.Name == rule.ComparisonPredicate);
                var propertyType = specificProperty.PropertyType;
                var genericType = Expression.Parameter(typeof(T));

                var key = Expression.Property(genericType, rule.ComparisonPredicate);
                var value = Expression.Constant(Convert.ChangeType(rule.ComparisonValue, propertyType));
                var binaryExpression = Expression.MakeBinary(rule.ComparisonOperator, key, value);

                compiledRules.Add(Expression.Lambda<Func<T, bool>>(binaryExpression, genericType).Compile());
            });

            // Return the compiled rules to the caller
            return compiledRules;
        }
    }
}

//null object value vs null comparison value
//null object valus vs non null comparisson value

//non null object value vs null comparisson value
//non null object value vs non null comparisson value

//this was an attempt do direct compairson with nullable types
//var isNullableDate = (propertyType == typeof(Nullable<DateTime>));
//var hasNullComparisonValue = isNullableDate && ReferenceEquals(null, rule.ComparisonValue);
//var nonNullDateCompison = hasNullComparisonValue ? (DateTime?)DateTime.MinValue : rule.ComparisonValue;
//propertyType = hasNullComparisonValue ? typeof(DateTime) : propertyType;
//var value = Expression.Constant(Convert.ChangeType(hasNullComparisonValue ? nonNullDateCompison : rule.ComparisonValue, propertyType));


//EXAMPLE LOOKING FORWARD TOO
//using System;
//using System.Linq.Expressions;
//class Foo
//{
//    public int? Bar { get; set; }

//    static void Main()
//    {
//        var param = Expression.Parameter(typeof(Foo), "foo");
//        Expression member = Expression.PropertyOrField(param, "Bar");
//        Type typeIfNullable = Nullable.GetUnderlyingType(member.Type);
//        if (typeIfNullable != null)
//        {
//            member = Expression.Call(member, "GetValueOrDefault", Type.EmptyTypes);
//        }
//        var body = Expression.Lambda<Func<Foo, int>>(member, param);

//        var func = body.Compile();
//        int result1 = func(new Foo { Bar = 123 }),
//            result2 = func(new Foo { Bar = null });
//    }
//}