using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemos.Core.Entities
{
    public class UserRefreshTokens 
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public string Token { get; set; }
        public DateTime LastIssuedDate {  get; set; }
        public DateTime ExpiresOn {  get; set; }
    }
}
