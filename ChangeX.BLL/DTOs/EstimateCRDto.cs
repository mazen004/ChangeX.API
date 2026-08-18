using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs
{
    public class EstimateCRDto
    {
        public decimal EstimatedManHour { get; set; }
        public decimal ManHourRate { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly FinishDate { get; set; }
    }
}
