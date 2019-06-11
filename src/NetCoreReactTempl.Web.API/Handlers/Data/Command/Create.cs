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
        private readonly IDataManager<DAL.Entities.Data> _dataManager;
        private readonly IDataManager<DAL.Entities.Field> _fieldsManager;

        public CreateHandler(IDataManager<DAL.Entities.Data> dataManager, IDataManager<DAL.Entities.Field> fieldsManager)
        {
            _dataManager = dataManager;
            _fieldsManager = fieldsManager;
        }

        public async Task<BaseResponse<Dto.Data>> Handle(Create command, CancellationToken cancellationToken)
        {
            var data = await _dataManager.CreateAsync(new DAL.Entities.Data() {
                UserId = command.UserId
            });

            foreach (var field in command.Data.Fields) {
                 await _fieldsManager.CreateAsync(new DAL.Entities.Field()
                {
                    DataId = data.Id,
                    Name = field.Key,
                    Value = field.Value
                });
            }
            return new BaseResponse<Dto.Data>(new Dto.Data() {
                Id = data.Id,
                UserId = data.UserId,
                Fields =  command.Data.Fields
            }, null, 0);
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
