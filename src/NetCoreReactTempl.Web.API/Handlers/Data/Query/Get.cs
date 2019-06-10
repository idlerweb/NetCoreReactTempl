using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Handlers.Data.Query
{

    public class Get : BaseCommand<BaseResponse<Dto.Data>, Dto.Data>
    {
        public Get(long id, long userId, Dto.Data data)
            : base(id, userId, data) { }
    }

    public class GetHandler : IRequestHandler<Get, BaseResponse<Dto.Data>>
    {
        private readonly IMapper _mapper;
        private readonly IDataManager<DAL.Entities.Data> _dataManager;
        private readonly IDataManager<DAL.Entities.Field> _fieldsManager;

        public GetHandler(IDataManager<DAL.Entities.Data> dataManager, IDataManager<DAL.Entities.Field> fieldsManager, IMapper mapper)
        {
            _dataManager = dataManager;
            _fieldsManager = fieldsManager;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Dto.Data>> Handle(Get query, CancellationToken cancellationToken)
        {
            var data = await _dataManager.GetAsync(query.Id);
            var fields = await _fieldsManager.GetCollection().Where(f => f.DataId == data.Id).Select(f => new KeyValuePair<string, string>(f.Name, f.Value)).ToListAsync();
            var result = new Dto.Data() {
                Id = data.Id,
                UserId = data.UserId,
                Fields = fields.ToDictionary(a => a.Key, a => a.Value)
            };
            return new BaseResponse<Dto.Data>(result, null, 0);
        }
    }

    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id not empty");
        }
    }
}
