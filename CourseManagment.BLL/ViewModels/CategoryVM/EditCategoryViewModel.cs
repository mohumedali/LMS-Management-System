using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CategoriesVM
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = default!;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = default!;

        public string? Image { get; set; }

        public IFormFile? ImageFile { get; set; }

    }
}