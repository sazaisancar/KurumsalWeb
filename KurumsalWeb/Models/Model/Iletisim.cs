﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace KurumsalWeb.Models.Model
{
    [Table("Iletisim")]
    public class Iletisim
    {
        [Key]
        public int IletisimId { get; set; }
        [StringLength(250, ErrorMessage ="250 karakter olmaldır")]
        public string Adres { get; set; }
        [StringLength(250, ErrorMessage = "250 karakter olmaldır")]
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Whatsapp { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }

    }


    
}