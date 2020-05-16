using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BillMealMVC.Models
{
    public class MenuHead
    {
        [Key]
        public int MenuHeadId { get; set; }
        public string Name { get; set; }
        public ICollection<MenuRow> MenuRows { get; set; }
    }
}