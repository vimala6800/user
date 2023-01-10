using MediatR;
using PartnerPortal.Application.Common.Exceptions;
using PartnerPortal.Application.Common.Interfaces;
using PartnerPortal.Application.TodoItems.Commands;
using PartnerPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PartnerPortal.Application.ProjectManagers.Commands
{
    public record UpdateProjectManagerCommand : IRequest
    {
        public Guid ProjectManagerID { get; set; }
        public string ProjectManagerName { get; set; }
        public string EmployeeID { get; set; }
        public DateTime JoiningDate { get; set; }
        //public string PMEmailID { get; set; }
        public string PMPhoneNumber { get; set; }
        public byte[] PMPhoto { get; set; }
        public Int16 PMStatus { get; set; }
        //public Guid? PMUserID { get; set; }
        public Guid SkillID { get; set; }
    }
    public class UpdateProjectManagerCommandHandler : IRequestHandler<UpdateProjectManagerCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProjectManagerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProjectManagerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ProjectManagers
                .FindAsync(new object[] { request.ProjectManagerID }, cancellationToken);
            var pskil = _context.projectManagerSkills.FirstOrDefault(e => e.ProjectManagerID == request.ProjectManagerID);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProjectManager), request.ProjectManagerID);
            }
            entity.ProjectManagerName = request.ProjectManagerName;
            entity.EmployeeID = request.EmployeeID;
            entity.JoiningDate = request.JoiningDate;
            
            entity.PMPhoneNumber = request.PMPhoneNumber;
            entity.PMPhoto = request.PMPhoto;
            entity.PMStatus = request.PMStatus;
            
            pskil.SkillID=request.SkillID;
            
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
