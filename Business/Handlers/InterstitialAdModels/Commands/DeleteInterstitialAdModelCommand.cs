﻿using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.InterstitialAdModels.Commands
{
    /// <summary>
    /// </summary>
    public class DeleteInterstitialAdModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public class DeleteInterstitialAdModelCommandHandler : IRequestHandler<DeleteInterstitialAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstitialAdModelRepository;

            public DeleteInterstitialAdModelCommandHandler(IInterstielAdModelRepository interstitialAdModelRepository)
            {
                _interstitialAdModelRepository = interstitialAdModelRepository;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteInterstitialAdModelCommand request,
                CancellationToken cancellationToken)
            {
                var isThereInterstitialAdModelRecord = await _interstitialAdModelRepository.GetAsync(u =>
                    u.Name == request.Name && u.ProjectId == request.ProjectId && u.Version == request.Version && u.Status == true);

                if (isThereInterstitialAdModelRecord is null)
                    return new ErrorResult(Messages.NotFound);

                isThereInterstitialAdModelRecord.Status = false;
                
                await _interstitialAdModelRepository.UpdateAsync(isThereInterstitialAdModelRecord, i =>
                    i.ProjectId == request.ProjectId && i.Name == request.Name && i.Version == request.Version);

                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}