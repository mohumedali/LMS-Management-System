using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseManagment.BLL.ViewModels.EnrollmentsVM
{
    public class EditEnrollmentViewModel
    {
        public int Id { get; set; }

        [Required]
        public EnrollmentStatus Status { get; set; }

        [Range(0, 100)]
        public int Progress { get; set; }
    }
}