using AutoMapper;
using FluentValidation;
using MediatR;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
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

        public GetHandler(IDataManager<DAL.Entities.Data> dataManager, IMapper mapper)
        {
            _dataManager = dataManager;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Dto.Data>> Handle(Get query, CancellationToken cancellationToken)
        {
            return new BaseResponse<Dto.Data>(_mapper.Map<Dto.Data>(await _dataManager.GetAsync(query.Id)), null, 0);
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
