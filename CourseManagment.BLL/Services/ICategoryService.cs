using CourseManagment.BLL.ViewModels.CategoriesVM;
using CourseManagment.BLL.ViewModels.CategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDropDownViewModel>> GetAllAsync(CancellationToken ct);

        Task<bool> ExistsAsync(int id, CancellationToken ct);

        Task<List<GetCategoryViewModel>> GetAllCategoriesForManagementAsync(
       CancellationToken ct);

        Task<CategoryDetailsViewModel?> GetCategoryDetailsAsync(
            int id,
            CancellationToken ct);

        Task<GetCategoryViewModel?> GetCategoryByIdAsync(
            int id,
            CancellationToken ct);

        Task<EditCategoryViewModel?> GetCategoryForEditAsync(
            int id,
            CancellationToken ct);

        Task<int> AddCategoryAsync(
            CreateCategoryViewModel model,
            CancellationToken ct);

        Task<int> UpdateCategoryAsync(
            int id,
            EditCategoryViewModel model,
            CancellationToken ct);

        Task<int> DeleteCategoryAsync(
            int id,
            CancellationToken ct);

    }
}
