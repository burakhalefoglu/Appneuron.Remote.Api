
using System;
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.InterstielAdHistoryModels.ValidationRules;
using MongoDB.Bson;

namespace Business.Handlers.InterstielAdHistoryModels.Commands
{


    public class UpdateInterstielAdHistoryModelCommand : IRequest<IResult>
    {
        public string ObjectId { get; set; }
        private ObjectId Id => new ObjectId(this.ObjectId);
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public float Version { get; set; }
        public int playerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }
        public DateTime StartTime { get; set; }
        public AdvStrategy[] AdvStrategies { get; set; }

        public class UpdateInterstielAdHistoryModelCommandHandler : IRequestHandler<UpdateInterstielAdHistoryModelCommand, IResult>
        {
            private readonly IInterstielAdHistoryModelRepository _interstielAdHistoryModelRepository;
            private readonly IMediator _mediator;

            public UpdateInterstielAdHistoryModelCommandHandler(IInterstielAdHistoryModelRepository interstielAdHistoryModelRepository, IMediator mediator)
            {
                _interstielAdHistoryModelRepository = interstielAdHistoryModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateInterstielAdHistoryModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateInterstielAdHistoryModelCommand request, CancellationToken cancellationToken)
            {
                var interstielAdHistoryModel = new InterstielAdHistoryModel();
                interstielAdHistoryModel.Name = request.Name;
                interstielAdHistoryModel.ProjectId = request.ProjectId;
                interstielAdHistoryModel.Version = request.Version;
                interstielAdHistoryModel.playerPercent = request.playerPercent;
                interstielAdHistoryModel.IsAdvSettingsActive = request.IsAdvSettingsActive;
                interstielAdHistoryModel.AdvStrategies = request.AdvStrategies;
                interstielAdHistoryModel.StarTime = request.StartTime;

                await _interstielAdHistoryModelRepository.UpdateAsync(request.Id, interstielAdHistoryModel);

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

