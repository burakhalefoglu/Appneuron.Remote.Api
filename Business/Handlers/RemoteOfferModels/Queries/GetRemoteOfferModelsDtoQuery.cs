using System.Text;
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

namespace Business.Handlers.RemoteOfferModels.Queries;

public class GetRemoteOfferModelsDtoQuery : IRequest<IDataResult<IEnumerable<RemoteOfferCustomerModelDto>>>
{
    public long ProjectId { get; set; }
    
    public class GetRemoteOfferModelsDtoQueryHandler : IRequestHandler<
        GetRemoteOfferModelsDtoQuery,
        IDataResult<IEnumerable<RemoteOfferCustomerModelDto>>>
    {
        private readonly IMediator _mediator;
        private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;

        public GetRemoteOfferModelsDtoQueryHandler(
            IRemoteOfferModelRepository remoteOfferModelRepository, IMediator mediator)
        {
            _remoteOfferModelRepository = remoteOfferModelRepository;
            _mediator = mediator;
        }

        [PerformanceAspect(5)]
        [CacheAspect(10)]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IDataResult<IEnumerable<RemoteOfferCustomerModelDto>>> Handle(
            GetRemoteOfferModelsDtoQuery request, CancellationToken cancellationToken)
        {
            var result = _remoteOfferModelRepository
                .GetListAsync().Result.Where(r => r.ProjectId == request.ProjectId &&
                                                  r.Status == true);
            var remoteOfferModelDtos = new List<RemoteOfferCustomerModelDto>();
            foreach (var remoteOfferModel in result)
            {
                var resultProductModels = await _mediator.Send(new GetRemoteOfferProductModelsQuery
                {
                 StrategyId = remoteOfferModel.Id
                }, cancellationToken);

                var remoteOfferModelDto = new RemoteOfferCustomerModelDto
                {
                    Id = remoteOfferModel.Id,
                    Name = remoteOfferModel.Name,
                    Version = remoteOfferModel.Version,
                    FinishTime = remoteOfferModel.FinishTime,
                    FirstPrice = remoteOfferModel.FirstPrice,
                    GiftTexture = Encoding.UTF8.GetString(remoteOfferModel.GiftTexture),
                    IsGift = remoteOfferModel.IsGift,
                    LastPrice = remoteOfferModel.LastPrice,
                    PlayerPercent = remoteOfferModel.PlayerPercent,
                    ProjectId = remoteOfferModel.ProjectId,
                    StartTime = remoteOfferModel.StartTime,
                    ValidityPeriod = remoteOfferModel.ValidityPeriod,
                    IsActive = remoteOfferModel.IsActive,
                };
                foreach (var remoteOfferProductModel in resultProductModels.Data)
                {
                    var remoteOfferProductDto = new RemoteOfferProductCustomerModelDto();
                    remoteOfferProductDto.Count = remoteOfferProductModel.Count;
                    remoteOfferProductDto.Image =  Encoding.UTF8.GetString(remoteOfferProductModel.Image);
                    remoteOfferProductDto.Name = remoteOfferProductModel.Name;
                    remoteOfferProductDto.ImageName = remoteOfferProductModel.ImageName;
                    remoteOfferModelDto.RemoteOfferProductModels.Add(remoteOfferProductDto);
                }
                remoteOfferModelDtos.Add(remoteOfferModelDto);
            }
            return new SuccessDataResult<IEnumerable<RemoteOfferCustomerModelDto>>(remoteOfferModelDtos);
        }
    }
}