using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VModels.DTOS.Auth
{
    public class ChangeUserPasswordByIdDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;  
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
        public bool AdminForce { get; set; } = false;
    }
}
