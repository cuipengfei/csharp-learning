using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace csharp_learning
{
    //学会使用ExpressionApi
    public class MoreEffectiveCItem46
    {
        [Fact]
        public void ShouldConvertToTargetPerson()
        {
            var sourceContact = new SourceContact
            {
                name = "sourceName"
            };
            var dynamicConverter = new DynamicConverter<SourceContact, TargetContact>();
            var targetContact = dynamicConverter.ConvertFrom(sourceContact);
            Assert.Equal("sourceName",targetContact.name);
            Assert.Null(targetContact.addr);
        }

        public class DynamicConverter<TSource, TDest> where TDest : new()
        {
            private Func<TSource, TDest> _converter;

            public TDest ConvertFrom(TSource source)
            {
                CreateConverterIfNeeded();
                return _converter(source);
            }

            private void CreateConverterIfNeeded()
            {
                if (_converter == null)
                {
                    var source = Expression.Parameter(typeof(TSource), "source");
                    var dest = Expression.Variable(typeof(TDest), "dest");

                    var assignments = from srcProp in typeof(TSource).GetProperties(
                            BindingFlags.Public | BindingFlags.Instance)
                        where srcProp.CanRead
                        let destProp =
                            typeof(TDest).GetProperty(srcProp.Name, BindingFlags.Public | BindingFlags.Instance)
                        where (destProp != null) && (destProp.CanWrite)
                        select Expression.Assign(
                            Expression.Property(dest, destProp), Expression.Property(source, srcProp));

                    //put together the body
                    var body = new List<Expression>();
                    body.Add(Expression.Assign(dest, Expression.New(typeof(TDest))));
                    body.AddRange(assignments);
                    body.Add(dest);

                    var expr = Expression.Lambda<Func<TSource, TDest>>(
                        Expression.Block(
                            new[] {dest},
                            body.ToArray()),
                        source);

                    var func = expr.Compile();
                    _converter = func;
                }
            }
        }

        public class SourceContact
        {
            public string name { get; set; }
            public string city { get; set; }
        }

        public class TargetContact
        {
            public TargetContact()
            {
            }

            public string name { get; set; }
            public string addr { get; set; } 
        }
    }
}