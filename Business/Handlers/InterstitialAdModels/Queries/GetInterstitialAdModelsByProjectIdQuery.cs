using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.InterstitialAdModels.Queries
{
    public class GetInterstitialAdModelsByProjectIdQuery : IRequest<IDataResult<IEnumerable<InterstitialAdModel>>>
    {
        public string ProjectId { get; set; }

        public class GetInterstitialAdModelsByProjectIdQueryHandler : IRequestHandler<
            GetInterstitialAdModelsByProjectIdQuery, IDataResult<IEnumerable<InterstitialAdModel>>>
        {
            private readonly IInterstielAdModelRepository _interstitialAdModelRepository;

            public GetInterstitialAdModelsByProjectIdQueryHandler(
                IInterstielAdModelRepository interstitialAdModelRepository)
            {
                _interstitialAdModelRepository = interstitialAdModelRepository;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<InterstitialAdModel>>> Handle(
                GetInterstitialAdModelsByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _interstitialAdModelRepository
                    .GetListAsync(i => i.ProjectId == request.ProjectId && i.Status == true);
                return new SuccessDataResult<IEnumerable<InterstitialAdModel>>(result);
            }
        }
    }
}