using CourseManagment.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.PaymentVm
{
    public class PaymentCardViewModel
    {
        public int Id { get; set; }

        public string CourseTitle { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
