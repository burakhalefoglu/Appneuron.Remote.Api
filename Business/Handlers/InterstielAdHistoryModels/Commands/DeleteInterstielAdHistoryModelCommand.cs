
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

namespace Business.Handlers.InterstielAdHistoryModels.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteInterstielAdHistoryModelCommand : IRequest<IResult>
    {
        public string ObjectId { get; set; }
        private ObjectId Id => new ObjectId(this.ObjectId);

        public class DeleteInterstielAdHistoryModelCommandHandler : IRequestHandler<DeleteInterstielAdHistoryModelCommand, IResult>
        {
            private readonly IInterstielAdHistoryModelRepository _interstielAdHistoryModelRepository;
            private readonly IMediator _mediator;

            public DeleteInterstielAdHistoryModelCommandHandler(IInterstielAdHistoryModelRepository interstielAdHistoryModelRepository, IMediator mediator)
            {
                _interstielAdHistoryModelRepository = interstielAdHistoryModelRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteInterstielAdHistoryModelCommand request, CancellationToken cancellationToken)
            {


                await _interstielAdHistoryModelRepository.DeleteAsync(request.Id);

                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

