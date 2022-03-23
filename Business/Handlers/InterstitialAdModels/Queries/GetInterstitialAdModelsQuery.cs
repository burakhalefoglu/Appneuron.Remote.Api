using Business.BusinessAspects;
using Business.Handlers.AdvStrategies.Query;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;

namespace Business.Handlers.InterstitialAdModels.Queries;

public class GetInterstitialAdModelsQuery : IRequest<IDataResult<IEnumerable<InterstitialAdModelDto>>>
{
    public long ProjectId { get; set; }

    public class InterstitialAdModelsQueryHandler : IRequestHandler<
        GetInterstitialAdModelsQuery, IDataResult<IEnumerable<InterstitialAdModelDto>>>
    {
        private readonly IInterstielAdModelRepository _interstitialAdModelRepository;
        private readonly IMediator _mediator;

        public InterstitialAdModelsQueryHandler(
            IInterstielAdModelRepository interstitialAdModelRepository, IMediator mediator)
        {
            _interstitialAdModelRepository = interstitialAdModelRepository;
            _mediator = mediator;
        }

        [PerformanceAspect(5)]
        [CacheAspect(10)]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IDataResult<IEnumerable<InterstitialAdModelDto>>> Handle(
            GetInterstitialAdModelsQuery request, CancellationToken cancellationToken)
        {
            var interstitialAdModelDtos = new List<InterstitialAdModelDto>();
            var result = _interstitialAdModelRepository
                .GetListAsync().Result.Where(u =>
                    u.ProjectId == request.ProjectId &&
                    u.Status == true);
            
            foreach (var ınterstitialAdModel in result)
            {
                var resultAdvStrategies = await _mediator.Send(new GetAdvStrategyQuery
                {
                    StrategyId = ınterstitialAdModel.Id
                }, cancellationToken);
                var interstitialAdModelDto = new InterstitialAdModelDto
                {
                    Id = ınterstitialAdModel.Id,
                    Name = ınterstitialAdModel.Name,
                    Version = ınterstitialAdModel.Version,
                    PlayerPercent = ınterstitialAdModel.PlayerPercent,
                    ProjectId = ınterstitialAdModel.ProjectId,
                    IsActive = ınterstitialAdModel.IsActive,
                    AdvStrategies = resultAdvStrategies.Data.ToArray()
                };
                interstitialAdModelDtos.Add(interstitialAdModelDto);
            }

            return new SuccessDataResult<IEnumerable<InterstitialAdModelDto>>(interstitialAdModelDtos);
        }
    }
}