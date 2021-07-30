using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeProdutoWinFormsCRUD.Infraestrutura
{
    public class Contexto : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=CRUDProdutos;Trusted_Connection=True;");
        }
    }
}
