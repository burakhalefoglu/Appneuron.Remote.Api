
using Business.Handlers.RemoteOfferHistoryModels.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.RemoteOfferHistoryModels.Queries.GetOfferHistoryModelsByProjectIdQuery;
using Entities.Concrete;
using static Business.Handlers.RemoteOfferHistoryModels.Commands.CreateRemoteOfferHistoryModelCommand;
using Business.Handlers.RemoteOfferHistoryModels.Commands;
using Business.Constants;
using MediatR;
using System.Linq;
using FluentAssertions;
using MongoDB.Bson;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class RemoteOfferHistoryModelHandlerTests
    {
        Mock<IRemoteOfferHistoryModelRepository> _remoteOfferHistoryModelRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _remoteOfferHistoryModelRepository = new Mock<IRemoteOfferHistoryModelRepository>();
            _mediator = new Mock<IMediator>();
        }

    
        [Test]
        public async Task RemoteOfferHistoryModel_GetByProjectIdQueries_Success()
        {
            //Arrange
            var query = new GetOfferHistoryModelsByProjectIdQuery();
            query.ProjectId = "121212";

            _remoteOfferHistoryModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<RemoteOfferHistoryModel, bool>>>()))
                        .ReturnsAsync(new List<RemoteOfferHistoryModel> { new RemoteOfferHistoryModel()
                        {
                            Version = 1,
                            FinishTime = DateTime.Now.Ticks,
                            FirstPrice = 12,
                            GiftTexture = new Byte[]{},
                            Id = new ObjectId(),
                            IsActive = true,
                            IsGift = true,
                            LastPrice = 10,
                            Name = "Test",
                            PlayerPercent = 20,
                            ProductList = new ProductModel[]{},
                            ProjectId = "121212",
                            StartTime = DateTime.Now.Ticks,
                            ValidityPeriod = 24
                            
                        },new RemoteOfferHistoryModel()
                        {
                            Version = 2,
                            FinishTime = DateTime.Now.Ticks,
                            FirstPrice = 12,
                            GiftTexture = new Byte[]{},
                            Id = new ObjectId(),
                            IsActive = true,
                            IsGift = true,
                            LastPrice = 0,
                            Name = "Test",
                            PlayerPercent = 20,
                            ProductList = new ProductModel[]{},
                            ProjectId = "121212",
                            StartTime = DateTime.Now.Ticks,
                            ValidityPeriod = 24
                            
                        },new RemoteOfferHistoryModel()
                        {
                            Version = 3,
                            FinishTime = DateTime.Now.Ticks,
                            FirstPrice = 12,
                            GiftTexture = new Byte[]{},
                            Id = new ObjectId(),
                            IsActive = true,
                            IsGift = true,
                            LastPrice = 4,
                            Name = "Test",
                            PlayerPercent = 20,
                            ProductList = new ProductModel[]{},
                            ProjectId = "121212",
                            StartTime = DateTime.Now.Ticks,
                            ValidityPeriod = 48
                            
                        },

                        }.AsQueryable());

            var handler = new GetOfferHistoryModelsByProjectIdQueryHandler(_remoteOfferHistoryModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task RemoteOfferHistoryModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateRemoteOfferHistoryModelCommand();
            command.FinishTime = new DateTime().Ticks;
            command.FirstPrice = 12;
            command.GiftTexture = new byte[] { };
            command.IsActive = true;
            command.IsGift = true;
            command.LastPrice = 2;
            command.Name = "Test";
            command.PlayerPercent = 15;
            command.ProductList = new ProductModel[] { };
            command.ProjectId = "121212";
            command.StartTime = DateTime.Now.Ticks;


            _remoteOfferHistoryModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferHistoryModel>()));

            var handler = new CreateRemoteOfferHistoryModelCommandHandler(_remoteOfferHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

    }
}

