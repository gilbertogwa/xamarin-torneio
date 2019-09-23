using System;
using System.Collections.Generic;
using System.Text;

namespace TIFA.Models
{
    public class Jogador
    {

        public Jogador()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Nome { get; set; }

    }
}
