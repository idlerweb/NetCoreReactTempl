using AutoMapper;
using FluentValidation;
using MediatR;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System.Threading;
using System.Threading.Tasks;


namespace NetCoreReactTempl.Web.API.Handlers.Data.Command
{

    public class Update : BaseCommand<BaseResponse<Dto.Data>, Dto.Data>
    {
        public Update(long id, long userId, Dto.Data data)
            : base(id, userId, data) { }
    }

    public class UpdateHandler : IRequestHandler<Update, BaseResponse<Dto.Data>>
    {
        private readonly IMapper _mapper;
        private readonly IDataManager<DAL.Entities.Data> _dataManager;

        public UpdateHandler(IDataManager<DAL.Entities.Data> dataManager, IMapper mapper)
        {
            _dataManager = dataManager;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Dto.Data>> Handle(Update command, CancellationToken cancellationToken)
        {
            var enter = _mapper.Map<DAL.Entities.Data>(command.Data);
            enter.UserId = command.UserId;
            var entity = await _dataManager.UpdateAsync(enter);
            return new BaseResponse<Dto.Data>(_mapper.Map<Dto.Data>(entity), null, 0);
        }
    }

    public class UpdateValidator : AbstractValidator<Update>
    {
        public UpdateValidator(IDataManager<DAL.Entities.Data> dataManager)
        {
            RuleFor(c => c.Data).NotEmpty().WithMessage("Model not empty");
            RuleFor(c => c).MustAsync(async (c, ct) => (await dataManager.GetAsync(c.Id)).UserId == c.UserId).WithMessage("You not autor");
        }
    }
}

