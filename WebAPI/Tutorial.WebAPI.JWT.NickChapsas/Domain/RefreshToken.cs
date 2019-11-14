using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Tutorial.WebAPI.JWT.NickChapsas.Domain
{
    public class RefreshToken
    {
        [Key] // Instead of ID
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
        public bool Invalidated { get; set; }



        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        #region Navigation Property
        public string UserId { get; set; }
        #endregion
    }
}
