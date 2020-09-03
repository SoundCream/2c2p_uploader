using _2C2P.FileUploader.Data;
using _2C2P.FileUploader.Interfaces.Data;
using _2C2P.FileUploader.Managers;
using _2C2P.FileUploader.Mappers.MapperProfile;
using _2C2P.FileUploader.UnitTest.Fakes;
using Autofac;
using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace _2C2P.FileUploader.UnitTest
{
    public abstract class UnitTestBase
    {
        protected AutoMock AutoMockContainer;

        protected virtual void PrepareTestFixture()
        {
        }

        [SetUp]
        public void Setup()
        {
            AutoMockContainer = AutoMock.GetLoose((builder) => {
                var mapperingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new TransactionEntiryMapperProfile());
                    mc.AddProfile(new TransactionUploadMapperProfile());
                });

                var mapper = mapperingConfig.CreateMapper();
                builder.RegisterInstance(mapper).As<IMapper>();

                builder.RegisterType<FakeLogger<FileUploadManager>>().As<ILogger>();
                builder.RegisterType<FakeLogger<TransactionManager>>().As<ILogger>();
            });

            PrepareTestFixture();
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
