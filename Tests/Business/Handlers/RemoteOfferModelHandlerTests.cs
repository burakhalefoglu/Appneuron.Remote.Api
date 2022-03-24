using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Avro.Util;
using Business.Constants;
using Business.Handlers.RemoteOfferModels.Commands;
using Business.Handlers.RemoteOfferModels.Queries;
using Business.Handlers.RemoteOfferProductModels.Queries;
using Core.Utilities.MessageBrokers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.RemoteOfferModels.Queries.GetRemoteOfferModelsDtoQuery;
using static Business.Handlers.RemoteOfferModels.Commands.CreateRemoteOfferModelCommand;
using static Business.Handlers.RemoteOfferModels.Commands.UpdateRemoteOfferModelCommand;
using static Business.Handlers.RemoteOfferModels.Commands.DeleteRemoteOfferModelCommand;

namespace Tests.Business.Handlers;

[TestFixture]
public class RemoteOfferModelHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _remoteOfferModelRepository = new Mock<IRemoteOfferModelRepository>();
        _mediator = new Mock<IMediator>();
        _messageBroker = new Mock<IMessageBroker>();
    }

    private Mock<IRemoteOfferModelRepository> _remoteOfferModelRepository;
    private Mock<IMediator> _mediator;
    private Mock<IMessageBroker> _messageBroker;


    [Test]
    public async Task RemoteOfferModel_GetQueries_Success()
    {
        //Arrange
        var query = new GetRemoteOfferModelsDtoQuery
        {
            ProjectId = 13
        };

        _remoteOfferModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
            .ReturnsAsync(new List<RemoteOfferModel>
            {
                new()
                {
                    Version = "2",
                    FinishTime = DateTime.Now.Ticks,
                    FirstPrice = 12,
                    GiftTexture = new byte[7]{12,22,34,23,65,111,44},
                    Id = 1,
                    IsGift = true,
                    LastPrice = 10,
                    Name = "Test",
                    PlayerPercent = 20,
                    ProjectId = 13,
                    StartTime = DateTime.Now.Ticks,
                    ValidityPeriod = 24,
                    Status = true,
                    CreatedAt = DateTime.Now,
                    IsActive = false
                }
            }.AsQueryable());

        _mediator.Setup(x => x.Send(It.IsAny<GetRemoteOfferProductModelsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new SuccessDataResult<IEnumerable<RemoteOfferProductModel>>(new RemoteOfferProductModel[]
                {
                    new()
                    {
                        Count = 1,
                        Id = 1,
                        Image = new byte[7]{12,22,34,23,65,111,44},
                        Name = "test",
                        Status = true,
                        CreatedAt = DateTime.Now,
                        ImageName = "test",
                        StrategyId = 13
                    },
                    new()
                    {
                        Count = 1,
                        Id = 2,
                        Image = new byte[7]{12,22,34,23,65,111,44},
                        Name = "test",
                        Status = true,
                        CreatedAt = DateTime.Now,
                        ImageName = "test",
                        StrategyId = 13
                    }
                }));

        var handler =
            new GetRemoteOfferModelsDtoQueryHandler(_remoteOfferModelRepository.Object, _mediator.Object);

        //Act
        var x = await handler.Handle(query, new CancellationToken());

        //Asset
        x.Success.Should().BeTrue();
        x.Data.ToArray()[0].RemoteOfferProductModels.Count.Should().BeGreaterThan(1);
    }

    [Test]
    public async Task RemoteOfferModel_CreateCommand_Success()
    {
        //Arrange
        var command = new CreateRemoteOfferModelCommand
        {
            FinishTime = new DateTime().Ticks,
            FirstPrice = 12,
            GiftTexture = "",
            IsGift = true,
            LastPrice = 2,
            Name = "Test",
            PlayerPercent = 15,
            ProductDtos = Array.Empty<RemoteOfferProductModelDto>(),
            ProjectId = 3,
            StartTime = DateTime.Now.Ticks
        };

        _remoteOfferModelRepository.Setup(x =>
                x.Any(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
            .Returns(false);

        _remoteOfferModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferModel>()));

        var handler =
            new CreateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
        var x = await handler.Handle(command, new CancellationToken());


        x.Success.Should().BeTrue();
        x.Message.Should().Be(Messages.Added);
    }

    [Test]
    public async Task RemoteOfferModel_CreateCommand_NameAlreadyExist()
    {
        //Arrange
        var command = new CreateRemoteOfferModelCommand
        {
            FinishTime = new DateTime().Ticks,
            FirstPrice = 12,
            GiftTexture = "",
            IsGift = true,
            LastPrice = 2,
            Name = "Test",
            PlayerPercent = 15,
            ProductDtos = Array.Empty<RemoteOfferProductModelDto>(),
            ProjectId = 3,
            StartTime = DateTime.Now.Ticks
        };

        _remoteOfferModelRepository.Setup(x =>
                x.AnyAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
            .ReturnsAsync(true);

        _remoteOfferModelRepository.Setup(x =>
            x.Add(It.IsAny<RemoteOfferModel>()));

        var handler =
            new CreateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
        var x = await handler.Handle(command, new CancellationToken());

        x.Success.Should().BeFalse();
        x.Message.Should().Be(Messages.NameAlreadyExist);
    }

    [Test]
    public async Task RemoteOfferModel_UpdateCommand_NoContent()
    {
        //Arrange
        var command = new UpdateRemoteOfferModelCommand
        {
            Name = "Test",
            Version = "1",
            PlayerPercent = 20,
            ProjectId = 3
        };

        _remoteOfferModelRepository.Setup(x =>
                x.Any(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
            .Returns(false);

        _remoteOfferModelRepository.Setup(x =>
                x.GetAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
            .ReturnsAsync(new RemoteOfferModel());

        _remoteOfferModelRepository.Setup(x => x.UpdateAsync(It.IsAny<RemoteOfferModel>()));

        var handler =
            new UpdateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
        var x = await handler.Handle(command, new CancellationToken());


        x.Success.Should().BeFalse();
        x.Message.Should().Be(Messages.NoContent);
    }

    [Test]
    public async Task RemoteOfferModel_UpdateCommand_Success()
    {
        //Arrange
        var command = new UpdateRemoteOfferModelCommand
        {
            Name = "Test",
            Version = "1",
            PlayerPercent = 20,
            ProjectId = 1
        };

        _remoteOfferModelRepository.Setup(x =>
                x.AnyAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
            .ReturnsAsync(true);

        _remoteOfferModelRepository.Setup(x =>
                x.GetAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
            .ReturnsAsync(new RemoteOfferModel());

        _mediator.Setup(x => x.Send(It.IsAny<GetRemoteOfferProductModelsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new SuccessDataResult<IEnumerable<RemoteOfferProductModel>>(new RemoteOfferProductModel[] { }));

        _remoteOfferModelRepository.Setup(x => x.UpdateAsync(It.IsAny<RemoteOfferModel>()));

        _mediator.Setup(x => x.Send(new object(), new CancellationToken()))
            .ReturnsAsync(new SuccessResult(Messages.Added));

        var handler =
            new UpdateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
        var x = await handler.Handle(command, new CancellationToken());

        x.Success.Should().BeTrue();
        x.Message.Should().Be(Messages.Updated);
    }

    [Test]
    public async Task RemoteOfferModel_DeleteCommand_Success()
    {
        //Arrange
        var command = new DeleteRemoteOfferModelCommand
        {
            Name = "Test",
            ProjectId = 1,
            Version = "1"
        };
        _remoteOfferModelRepository.Setup(x =>
                x.GetAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
            .ReturnsAsync(new RemoteOfferModel());

        _remoteOfferModelRepository.Setup(x =>
            x.UpdateAsync(It.IsAny<RemoteOfferModel>()));

        var handler =
            new DeleteRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
        var x = await handler.Handle(command, new CancellationToken());

        x.Success.Should().BeTrue();
        x.Message.Should().Be(Messages.Deleted);
    }
}