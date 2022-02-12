using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Handlers.RemoteOfferProductModels.Queries;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;

namespace Business.Handlers.RemoteOfferModels.Queries
{
    public class GetRemoteOfferModelsDtoByProjectIdQuery : IRequest<IDataResult<IEnumerable<RemoteOfferModelDto>>>
    {
        public long ProjectId { get; set; }

        public class GetRemoteOfferModelsDtoByProjectIdQueryHandler : IRequestHandler<
            GetRemoteOfferModelsDtoByProjectIdQuery,
            IDataResult<IEnumerable<RemoteOfferModelDto>>>
        {
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;
            private readonly IMediator _mediator;

            public GetRemoteOfferModelsDtoByProjectIdQueryHandler(
                IRemoteOfferModelRepository remoteOfferModelRepository, IMediator mediator)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]

            public async Task<IDataResult<IEnumerable<RemoteOfferModelDto>>> Handle(
                GetRemoteOfferModelsDtoByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _remoteOfferModelRepository
                    .GetListAsync(r => r.ProjectId == request.ProjectId && r.Status == true);
                var remoteOfferModelDtos = new List<RemoteOfferModelDto>();

                if (!result.Any()) return new SuccessDataResult<IEnumerable<RemoteOfferModelDto>>(remoteOfferModelDtos);
                foreach (var remoteOfferModel in result)
                {
                    var resultProductModels = await _mediator.Send(new GetRemoteOfferProductModelsQuery
                    {
                        Version = remoteOfferModel.Version,
                        RemoteOfferName = remoteOfferModel.Name
                    }, cancellationToken);
                    
                    var remoteOfferModelDto = new RemoteOfferModelDto
                    {
                        Id = remoteOfferModel.Id,
                        Name = remoteOfferModel.Name,
                        Version = remoteOfferModel.Version,
                        FinishTime = remoteOfferModel.FinishTime,
                        FirstPrice = remoteOfferModel.FirstPrice,
                        GiftTexture = remoteOfferModel.GiftTexture,
                        IsActive = remoteOfferModel.IsActive,
                        IsGift = remoteOfferModel.IsGift,
                        LastPrice = remoteOfferModel.LastPrice,
                        PlayerPercent = remoteOfferModel.PlayerPercent,
                        ProjectId = remoteOfferModel.ProjectId,
                        StartTime = remoteOfferModel.StartTime,
                        ValidityPeriod = remoteOfferModel.ValidityPeriod
                    };
                    if (resultProductModels.Data.Any())
                        remoteOfferModelDto.RemoteOfferProductModels = resultProductModels.Data.ToArray();
                    remoteOfferModelDtos.Add(remoteOfferModelDto);
                }
                return new SuccessDataResult<IEnumerable<RemoteOfferModelDto>>(remoteOfferModelDtos);
            }
        }
    }
}