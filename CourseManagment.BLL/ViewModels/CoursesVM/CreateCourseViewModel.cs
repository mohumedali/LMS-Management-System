using CourseManagment.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CourseManagment.BLL.ViewModels.CoursesVM
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Course title is required.")]
        [StringLength(100, MinimumLength = 5)]
        
        public string Title { get; set; } = default!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, MinimumLength = 20)]
        public string Description { get; set; } = default!;

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Duration { get; set; }

        [Required]
        public CourseLevel Level { get; set; }

        [Required]
        [StringLength(50)]
        public string Language { get; set; } = default!;

        public string? ImageUrl { get; set; }


        public IFormFile? ImageFile { get; set; }
        [Required]

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        [Required]

        [Range(1, int.MaxValue, ErrorMessage = "Please select an instructor.")]
        public int InstructorId { get; set; }

        // ============================
        // Dropdown Lists
        // ============================

        public List<DropdownItemViewModel> Categories { get; set; } = new();

        public List<DropdownItemViewModel> Instructors { get; set; } = new();
    }
}