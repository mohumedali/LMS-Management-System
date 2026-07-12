using CourseManagment.BLL.ViewModels.InstructorsVM;
using CourseManagment.BLL.ViewModels.InstructorVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorDropDownViewModel>> GetAllAsync(CancellationToken ct);

        Task<bool> ExistsAsync(int id, CancellationToken ct);

        Task<InstructorManagementPageViewModel> GetAllInstructorsForManagementAsync(
            CancellationToken ct);

        Task<InstructorDetailsViewModel?> GetInstructorDetailsAsync(
            int id,
            CancellationToken ct);

        Task<EditInstructorViewModel?> GetInstructorForEditAsync(
            int id,
            CancellationToken ct);

        Task<int> AddInstructorAsync(
            CreateInstructorViewModel model,
            CancellationToken ct);

        Task<int> UpdateInstructorAsync(
            int id,
            EditInstructorViewModel model,
            CancellationToken ct);

        Task<int> DeleteInstructorAsync(
            int id,
            CancellationToken ct);


    }
}
