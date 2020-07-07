using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using NetCoreReactTempl.Domain.Models;

namespace NetCoreReactTempl.App.Handlers.Data.Command
{

    public class Create : BaseCommand<Domain.Models.Data>
    {
        public Create(long id, long userId, Domain.Models.Data data)
            : base(id, userId, data) { }
    }

    public class CreateHandler : IRequestHandler<Create, BaseModel>
    {
        private readonly IDataManager<Domain.Models.Data> _dataManager;
        private readonly IDataManager<Field> _fieldsManager;

        public CreateHandler(IDataManager<Domain.Models.Data> dataManager, IDataManager<Field> fieldsManager)
        {
            _dataManager = dataManager;
            _fieldsManager = fieldsManager;
        }

        public async Task<BaseModel> Handle(Create command, CancellationToken cancellationToken)
        {
            var data = await _dataManager.CreateAsync(new Domain.Models.Data()
            {
                UserId = command.UserId
            });

            foreach (var field in command.Data.Fields)
            {
                await _fieldsManager.CreateAsync(new Field()
                {
                    DataId = data.Id,
                    Name = field.Key,
                    Value = field.Value
                });
            }
            return new BaseModel
            {
                Id = data.Id                
            };
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
