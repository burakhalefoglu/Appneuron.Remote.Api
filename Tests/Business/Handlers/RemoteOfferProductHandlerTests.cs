using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Handlers.RemoteOfferProductModels.Commands;
using Business.Handlers.RemoteOfferProductModels.Queries;
using Core.Utilities.MessageBrokers;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.RemoteOfferProductModels.Commands.CreateRemoteOfferProductModelCommand;
using static Business.Handlers.RemoteOfferProductModels.Queries.GetRemoteOfferProductModelsQuery;

namespace Tests.Business.Handlers;

[TestFixture]
public class RemoteOfferProductHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _remoteOfferModelRepository = new Mock<IRemoteOfferProductModelRepository>();
        _mediator = new Mock<IMediator>();
        _messageBroker = new Mock<IMessageBroker>();
    }

    private Mock<IRemoteOfferProductModelRepository> _remoteOfferModelRepository;
    private Mock<IMediator> _mediator;
    private Mock<IMessageBroker> _messageBroker;


    [Test]
    public async Task RemoteOfferProductModel_Create_Success()
    {
        var createRemoteOfferProductModelCommand = new CreateRemoteOfferProductModelCommand
        {
            Count = 1,
            Name = "test",
            Version = "1",
            ImageName = "test Image",
            RemoteOfferName = "test remote offer name"
        };

        _remoteOfferModelRepository.Setup(x => x.AddAsync(It.IsAny<RemoteOfferProductModel>()));
        var handler =
            new CreateRemoteOfferProductModelCommandHandler(_remoteOfferModelRepository.Object);

        //Act
        var x = await handler.Handle(createRemoteOfferProductModelCommand, new CancellationToken());

        //Asset
        x.Success.Should().BeTrue();
    }


    [Test]
    public async Task AdvStrategy_GetQuery_Success()
    {
        var getAdvStrategyQuery = new GetRemoteOfferProductModelsQuery
        {
            Version = "1.2",
            RemoteOfferName = "test offer"
        };

        _remoteOfferModelRepository
            .Setup(x => x.GetListAsync(It.IsAny<Expression<Func<RemoteOfferProductModel, bool>>>()))
            .ReturnsAsync(new EnumerableQuery<RemoteOfferProductModel>(new List<RemoteOfferProductModel>
            {
                new(),
                new()
            }));
        var handler =
            new GetRemoteOfferProductModelsQueryHandler(_remoteOfferModelRepository.Object);

        //Act
        var x = await handler.Handle(getAdvStrategyQuery, new CancellationToken());

        //Asset
        x.Success.Should().BeTrue();
        x.Data.ToList().Should().HaveCountGreaterThan(1);
    }
}