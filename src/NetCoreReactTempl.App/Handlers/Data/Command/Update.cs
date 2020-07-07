using AutoMapper;
using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;


namespace NetCoreReactTempl.App.Handlers.Data.Command
{

    public class Update : BaseCommand<Domain.Models.Data>
    {
        public Update(long id, long userId, Domain.Models.Data data)
            : base(id, userId, data) { }
    }

    public class UpdateHandler : IRequestHandler<Update, BaseModel>
    {
        private readonly IMapper _mapper;
        private readonly IDataManager<Domain.Models.Data> _dataManager;

        public UpdateHandler(IDataManager<Domain.Models.Data> dataManager, IMapper mapper)
        {
            _dataManager = dataManager;
            _mapper = mapper;
        }

        public async Task<BaseModel> Handle(Update command, CancellationToken cancellationToken)
        {
            var enter = _mapper.Map<Domain.Models.Data>(command.Data);
            enter.UserId = command.UserId;
            var entity = await _dataManager.UpdateAsync(enter);
            return new BaseModel
            {
                Id = entity.Id
            };
        }
    }

    public class UpdateValidator : AbstractValidator<Update>
    {
        public UpdateValidator(IDataManager<Domain.Models.Data> dataManager)
        {
            RuleFor(c => c.Data).NotEmpty().WithMessage("Model not empty");
            RuleFor(c => c).MustAsync(async (c, ct) => (await dataManager.GetAsync(c.Id)).UserId == c.UserId).WithMessage("You not autor");
        }
    }
}

