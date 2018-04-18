using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericTests
{
    public class ProfileCreateRequest
    {
        [Required]
        public string ProfileUsername { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public long? BillingAccountNumber { get; set; }
    }
}
