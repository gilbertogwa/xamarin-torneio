using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TIFA.Models
{

    public class ClassificacaoInicial : Classificacao
    {
        public ClassificacaoInicial Clone()
        {
            return  (ClassificacaoInicial)MemberwiseClone();
        }
    }

}
