
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Business.Handlers.InterstielAdModels.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteInterstielAdModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }

        public class DeleteInterstielAdModelCommandHandler : IRequestHandler<DeleteInterstielAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstielAdModelRepository;
            private readonly IMediator _mediator;

            public DeleteInterstielAdModelCommandHandler(IInterstielAdModelRepository interstielAdModelRepository, IMediator mediator)
            {
                _interstielAdModelRepository = interstielAdModelRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteInterstielAdModelCommand request, CancellationToken cancellationToken)
            {
                await _interstielAdModelRepository.DeleteAsync(i=> i.ProjectId == request.ProjectId && i.Name == request.Name && i.Version == request.Version);

                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

