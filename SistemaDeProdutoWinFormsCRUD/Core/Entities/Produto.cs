using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeProdutoWinFormsCRUD.Core.Entities
{
    public class Produto
    {
        public int IdProd { get; set; }
        public int IdCategoria { get; set; }
        public int IdMarca { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
