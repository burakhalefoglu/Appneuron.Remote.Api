using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Handlers.AdvStrategies.Command;
using Business.Handlers.AdvStrategies.Query;
using Core.Utilities.MessageBrokers;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.AdvStrategies.Command.CreateAdvStrategyCommand;
using static Business.Handlers.AdvStrategies.Query.GetAdvStrategyQuery;


namespace Tests.Business.Handlers;

[TestFixture]
public class AdvStrategyHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _advStrategyRepository = new Mock<IAdvStrategyRepository>();
        _mediator = new Mock<IMediator>();
        _messageBroker = new Mock<IMessageBroker>();
    }

    private Mock<IAdvStrategyRepository> _advStrategyRepository;
    private Mock<IMediator> _mediator;
    private Mock<IMessageBroker> _messageBroker;


    [Test]
    public async Task AdvStrategy_Create_Success()
    {
        var createAdvStrategyCommand = new CreateAdvStrategyCommand
        {
            Count = 1,
            Name = "test"
        };

        _advStrategyRepository.Setup(x => x.AddAsync(It.IsAny<AdvStrategy>()));
        var handler =
            new CreateAdvStrategyCommandHandler(_advStrategyRepository.Object);

        //Act
        var x = await handler.Handle(createAdvStrategyCommand, new CancellationToken());

        //Asset
        x.Success.Should().BeTrue();
    }


    [Test]
    public async Task AdvStrategy_GetQuery_Success()
    {
        var getAdvStrategyQuery = new GetAdvStrategyQuery
        {
            Name = "test",
            Version = "1"
        };

        _advStrategyRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<AdvStrategy, bool>>>()))
            .ReturnsAsync(new EnumerableQuery<AdvStrategy>(new List<AdvStrategy>
            {
                new(),
                new()
            }));
        var handler =
            new GetAdvStrategyQueryHandler(_advStrategyRepository.Object);

        //Act
        var x = await handler.Handle(getAdvStrategyQuery, new CancellationToken());

        //Asset
        x.Success.Should().BeTrue();
        x.Data.ToList().Should().HaveCountGreaterThan(1);
    }
}