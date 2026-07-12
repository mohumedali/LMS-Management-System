using CourseManagment.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CoursesVM
{
    public class EditCourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course title is required.")]
        [StringLength(100,
            MinimumLength = 5,
            ErrorMessage = "Title must be between 5 and 100 characters.")]
        public string Title { get; set; } = default!;

        [Required(ErrorMessage = "Course description is required.")]
        [StringLength(2000,
            MinimumLength = 20,
            ErrorMessage = "Description must be between 20 and 2000 characters.")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 100000,
            ErrorMessage = "Price must be between 0 and 100000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, 500,
            ErrorMessage = "Duration must be between 1 and 500 hours.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Please select a course level.")]
        public CourseLevel Level { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        [StringLength(50,
            ErrorMessage = "Language can't exceed 50 characters.")]
        public string Language { get; set; } = default!;

        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "Please select a valid category.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please select an instructor.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "Please select a valid instructor.")]
        public int InstructorId { get; set; }

        public List<DropdownItemViewModel> Categories { get; set; } = [];

        public List<DropdownItemViewModel> Instructors { get; set; } = [];

    }
}
