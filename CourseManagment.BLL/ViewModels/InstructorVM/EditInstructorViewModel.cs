using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.InstructorsVM
{
    public class EditInstructorViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [Phone]
        public string Phone { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; } = default!;

        [Required]
        [StringLength(1000)]
        public string Bio { get; set; } = default!;
        public string? Image { get; set; }      

        public IFormFile? ImageFile { get; set; } 
    }
}