using PartnerPortal.Application.Requisitions.Commands;
using PartnerPortal.Application.TodoLists.Queries.ExportTodos;
using PartnerPortal.Domain.Entities;

namespace PartnerPortal.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        //byte[] BuildSkillsFile(List<SkillsRecord> records);
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
        byte[] BuildRequisitionsFile(IEnumerable<RequisitionRecord> records);
    }
}
