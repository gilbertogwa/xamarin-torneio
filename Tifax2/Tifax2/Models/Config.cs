using System;
using System.Collections.Generic;
using System.Text;

namespace TIFA.Models
{
    public class Config
    {
        public string Versao { get; set; }

        public string RegraDefault { get; set; }

        public static Config Current { get; set; }
    }
}
