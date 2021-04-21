using Microsoft.AspNetCore.Http;
using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Service.Data.Models
{
    public class Promo : BaseEntity, IEntity
    {
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        //[MaxFileSize(1*1024 * 1024)]
        //[PermittedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".git"})]
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
        public string ImageStorageName { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodUntil { get; set; }
        [ForeignKey("voucher_id")]
        public long VoucherId { get; set; }
        public Voucher Voucher { get; set; }
    }
}
