using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CategoriesVM
{
    public class CategoryManagementPageViewModel
    {
        public IEnumerable<GetCategoryViewModel> Categories { get; set; }
            = [];

        public string? Search { get; set; }
    }
}
