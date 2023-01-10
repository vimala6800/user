using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using PartnerPortal.Application.Common.Interfaces;
using PartnerPortal.Application.Common.Mappings;
using PartnerPortal.Application.Common.Models;
using PartnerPortal.Application.TodoItems.Queries;
using PartnerPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnerPortal.Application.ProjectManagers.Queries
{
    public class ProjectManagerBriefDto : IMapFrom<ProjectManager>
    {
        public string ProjectManagerName { get; set; }
        public string EmployeeID { get; set; }
        public DateTime JoiningDate { get; set; }
    }
    
}
