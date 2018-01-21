using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class Proc_MaterialEntryMas
    {
        public int Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? EDate { get; set; }

        public int ProcProjectId { get; set; }
        public  virtual ProcProject ProcProject { get; set; }
    }
}