using Microsoft.EntityFrameworkCore;
using SistemaDeProdutoWinFormsCRUD.Core.Entities;
using SistemaDeProdutoWinFormsCRUD.Infraestrutura;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaDeProdutoWinFormsCRUD.Core.ViewModel;

namespace SistemaDeProdutoWinFormsCRUD.ApplicationLayer.Services.Implementations
{
    public class ProdutoService
    {
        public List<Categoria> ListarCategorias()
        {
            try
            {
                var context = new Contexto();
                var command = context.Database.GetDbConnection().CreateCommand();

                command.CommandText = "select * from categorias";

                context.Database.OpenConnection();

                var listaCategorias = new List<Categoria>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Categoria();
                        item.idCategoria = (int)reader["IDCATEGORIA"];
                        item.categoria = (string)reader["CATEGORIA"];
                        listaCategorias.Add(item);
                    }
                }

                context.Database.CloseConnection();

                return listaCategorias;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<Marca> ListarMarcas()
        {
            try
            {
                var context = new Contexto();
                var command = context.Database.GetDbConnection().CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PROC_S_MARCA";

                context.Database.OpenConnection();

                var listaMarcas = new List<Marca>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Marca();
                        item.IdMarca = (int)reader["IDMARCA"];
                        item.NomeMarca = (string)reader["MARCA"];
                        listaMarcas.Add(item);
                    }
                }

                context.Database.CloseConnection();

                return listaMarcas;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private SqlDataReader LeerFilas;
        public DataTable ListarMarcas2()
        {
            var context = new Contexto();
            var command = context.Database.GetDbConnection().CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PROC_S_MARCA";

            context.Database.OpenConnection();

            DataTable Table = new DataTable();
            LeerFilas = (SqlDataReader)command.ExecuteReader();
            Table.Load(LeerFilas);

            context.Database.CloseConnection();
            return Table;
        }

        public List<ProdutoViewModel> ListarProdutos()
        {
            try
            {
                using (var context = new Contexto())
                {
                    var command = context.Database.GetDbConnection().CreateCommand();

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[PROC_S_PRODUTOS]";

                    context.Database.OpenConnection();

                    var listaProdutos = new List<ProdutoViewModel>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new ProdutoViewModel();
                            item.Id = (int)reader["ID"];
                            item.Categoria = (string)reader["CATEGORIA"];
                            item.Marca = (string)reader["MARCA"];
                            item.Descricao = (string)reader["DESCRICAO"];
                            item.Preco = (decimal)reader["PRECO"];
                            listaProdutos.Add(item);
                        }
                    }

                    return listaProdutos;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SalvarProduto(int IdCategoria, int IdMarca, string Descricao, Decimal preco)
        {
            using (var context = new Contexto())
            {

                var command = context.Database.GetDbConnection().CreateCommand();

                command.CommandText = "PROC_I_INSERIR_PRODUTO";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@IdCategoria", IdCategoria));
                command.Parameters.Add(new SqlParameter("@IdMarca", IdMarca));
                command.Parameters.Add(new SqlParameter("@Descricao", Descricao));
                command.Parameters.Add(new SqlParameter("@preco", preco));
                context.Database.OpenConnection();

                command.ExecuteNonQuery();
            }


        }

        public void EditarProduto(int idProd, int IdCategoria, int IdMarca, string Descricao, Decimal preco)
        {
            using (var context = new Contexto())
            {

                var command = context.Database.GetDbConnection().CreateCommand();

                command.CommandText = "[PROC_U_EDITAR_PRODUTO]";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Idprod", idProd));
                command.Parameters.Add(new SqlParameter("@IdCategoria", IdCategoria));
                command.Parameters.Add(new SqlParameter("@IdMarca", IdMarca));
                command.Parameters.Add(new SqlParameter("@Descricao", Descricao));
                command.Parameters.Add(new SqlParameter("@preco", preco));
                context.Database.OpenConnection();

                command.ExecuteNonQuery();
            }
        }

        public void ExcluirProduto(int idProd)
        {
            using (var context = new Contexto())
            {

                var command = context.Database.GetDbConnection().CreateCommand();

                command.CommandText = "delete from PRODUTOS where idProd = "+idProd;
                command.CommandType = CommandType.Text;
                context.Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        }

        public void ExcluirProduto(List<int> idsProds)
        {
            using (var context = new Contexto())
            {

                var command = context.Database.GetDbConnection().CreateCommand();

                command.CommandText = $"delete from PRODUTOS where idProd in ({String.Join(",",idsProds)})";
                command.CommandType = CommandType.Text;
                context.Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        }
    }

}
