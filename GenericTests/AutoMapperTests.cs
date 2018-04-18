using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericTests
{
    [TestClass]
    public class AutoMapperTests
    {
        private const int _testValue = 1000;

        public AutoMapperTests()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<MapperSource, MapperDestination>()
                      .ForCtorParam("differentValue", value => value.MapFrom(source => _testValue));
                      /* DOES NOT WORK *///.ForMember(destination => destination.Sub.SubProperty, property => property.MapFrom(source => source.SubProperty));
                config.CreateMap<MapperSource, SubDestiniation>();
            });
        }

        [TestMethod]
        public void OverriddenConstructorTest()
        {
            //arrange
            var source = new MapperSource() { MyProperty = 5 };

            //act
            var desitiation = Mapper.Map<MapperDestination>(source);

            //assert
            Assert.AreEqual(source.MyProperty, desitiation.MyProperty);
            Assert.AreEqual(_testValue, desitiation.Different);
        }

        [TestMethod]
        public void SubClassMapping()
        {
            //arrange
            var source = new MapperSource()
            {
                MyProperty = 5,
                SubProperty = 10
            };

            //act
            var desitiation = Mapper.Map<MapperDestination>(source);
            desitiation.Sub = Mapper.Map<SubDestiniation>(source);

            //assert
            Assert.AreEqual(source.SubProperty, desitiation.Sub.SubProperty);
        }

    }
}
