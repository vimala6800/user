using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using PartnerPortal.Application.Common.Interfaces;
using PartnerPortal.Application.Common.Models;
using PartnerPortal.Application.Common.Mappings;
using PartnerPortal.Application.TodoItems.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnerPortal.Application.ProjectManagers.Queries
{
    public record GetProjectManagersWithPaginationQuery : IRequest<PaginatedList<ProjectManagerBriefDto>>
    {
        public string EmployeeID { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetProjectManagersWithPaginationQueryHandler : IRequestHandler<GetProjectManagersWithPaginationQuery, PaginatedList<ProjectManagerBriefDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProjectManagersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProjectManagerBriefDto>> Handle(GetProjectManagersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.ProjectManagers
                .Where(x => x.EmployeeID == request.EmployeeID)
                .OrderBy(x => x.ProjectManagerName)
                .ProjectTo<ProjectManagerBriefDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
