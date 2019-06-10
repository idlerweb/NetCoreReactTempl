using AutoMapper;
using FluentValidation;
using MediatR;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Handlers.Data.Command
{

    public class Create : BaseCommand<BaseResponse<Dto.Data>, Dto.Data>
    {
        public Create(long id, long userId, Dto.Data data)
            : base(id, userId, data) { }
    }

    public class CreateHandler : IRequestHandler<Create, BaseResponse<Dto.Data>>
    {
        private readonly IMapper _mapper;
        private readonly IDataManager<DAL.Entities.Data> _dataManager;

        public CreateHandler(IDataManager<DAL.Entities.Data> dataManager, IMapper mapper)
        {
            _dataManager = dataManager;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Dto.Data>> Handle(Create command, CancellationToken cancellationToken)
        {
            var enter = _mapper.Map<DAL.Entities.Data>(command.Data);
            enter.UserId = command.UserId;
            var entity = await _dataManager.CreateAsync(enter);
            return new BaseResponse<Dto.Data>(_mapper.Map<Dto.Data>(entity), null, 0);
        }
    }

    public class CreateValidator : AbstractValidator<Create>
    {
        public CreateValidator()
        {
            RuleFor(c => c.Data).NotEmpty().WithMessage("Model not empty");
        }
    }
}
