﻿
using Business.Handlers.InterstielAdHistoryModels.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.InterstielAdHistoryModels.Queries.GetInterstielAdHistoryModelByProjectIdQuery;
using Entities.Concrete;
using static Business.Handlers.InterstielAdHistoryModels.Commands.CreateInterstielAdHistoryModelCommand;
using Business.Handlers.InterstielAdHistoryModels.Commands;
using Business.Constants;
using MediatR;
using System.Linq;
using FluentAssertions;
using MongoDB.Bson;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class InterstielAdHistoryModelHandlerTests
    {
        Mock<IInterstielAdHistoryModelRepository> _interstielAdHistoryModelRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _interstielAdHistoryModelRepository = new Mock<IInterstielAdHistoryModelRepository>();
            _mediator = new Mock<IMediator>();
        }

      
        [Test]
        public async Task InterstielAdHistoryModel_GetQueries_Success()
        {
            //Arrange
            var query = new GetInterstielAdHistoryModelByProjectIdQuery();
            query.ProjectId = "121212";

            _interstielAdHistoryModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<InterstielAdHistoryModel, bool>>>()))
                        .ReturnsAsync(new List<InterstielAdHistoryModel> { new InterstielAdHistoryModel()
                        {
                            ProjectId = "121212",
                            IsAdvSettingsActive = true,
                            Id = new ObjectId(),
                            Name = "test",
                            StarTime = DateTime.Now,
                            Version = 1,
                            playerPercent = 10
                             
                        }, 
                            new InterstielAdHistoryModel()
                        {
                            ProjectId = "121212",
                            IsAdvSettingsActive = true,
                            Id = new ObjectId(),
                            Name = "test",
                            StarTime = DateTime.Now,
                            Version = 2,
                            playerPercent = 20
                             
                        },

                        }.AsQueryable());

            var handler = new GetInterstielAdHistoryModelByProjectIdQueryHandler(_interstielAdHistoryModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);
            x.Data.ToList().Count.Should().Be(2);

        }

        [Test]
        public async Task InterstielAdHistoryModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateInterstielAdHistoryModelCommand();
            command.ProjectId = "121212";
            command.IsAdvSettingsActive = true;
            command.Name = "test";
            command.playerPercent = 20;

            _interstielAdHistoryModelRepository.Setup(x => 
                x.AddAsync(It.IsAny<InterstielAdHistoryModel>()));

            var handler = new CreateInterstielAdHistoryModelCommandHandler(_interstielAdHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
        
    }
}
