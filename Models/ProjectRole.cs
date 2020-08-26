using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DESI8N.com.Models
{
    public class ProjectRole
    {
        public string Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
