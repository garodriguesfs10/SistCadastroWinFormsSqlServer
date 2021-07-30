using SistemaDeProdutoWinFormsCRUD.ApplicationLayer.Services.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaDeProdutoWinFormsCRUD.Apresentation
{
    public partial class Produtos : Form
    {
        public Produtos()
        {
            InitializeComponent();
        }

        private string Operacao = "Inserir";
        private string idProdView = "";

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Produtos_Load(object sender, EventArgs e)
        {
            ListarProdutos();
            PreencherComboBox();
            txtOperacao.Text = Operacao;
        }

        public void PreencherComboBox()
        {
            var produtoService = new ProdutoService();
            cmbCategoria.DataSource = produtoService.ListarCategorias();
            cmbCategoria.DisplayMember = "CATEGORIA";
            cmbCategoria.ValueMember = "IDCATEGORIA";

            cmbMarca.DataSource = produtoService.ListarMarcas();
            cmbMarca.DisplayMember = "NOMEMARCA";
            cmbMarca.ValueMember = "IDMARCA";
        }

        private void txtPreco_Leave(object sender, EventArgs e)
        {

            Double value;
            if (Double.TryParse(txtPreco.Text.Replace("R$", "").Trim(), out value))
                txtPreco.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", value);
            else
            {
                txtPreco.Text = "R$ 0,00";
                MessageBox.Show("Valor não aceito, use apenas números!");
            }

        }

        private void txtPreco_Enter(object sender, EventArgs e)
        {
            if (txtPreco.Text == "R$ 0,00")
                txtPreco.Text = String.Empty;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            try
            {
                var produtoService = new ProdutoService();
                var categoria = cmbCategoria.SelectedValue.ToString();
                var marca = cmbMarca.SelectedValue.ToString();
                var descricao = txtDescricao.Text;
                var preco = txtPreco.Text;
                if (categoria == "" || marca == "" || descricao == "" || preco == "R$ 0,00")
                {
                    MessageBox.Show("Preencha corretamente todos os campos!");
                    return;
                }
                var categoriaInt = Convert.ToInt32(cmbCategoria.SelectedValue);
                var marcaInt = Convert.ToInt32(cmbMarca.SelectedValue);
                var precoOnly = preco.Replace("R$", "").Trim();
                if (Operacao == "Inserir")
                {
                    produtoService.SalvarProduto(categoriaInt, marcaInt, descricao, Convert.ToDecimal(precoOnly));
                    MessageBox.Show("Dados inseridos com sucesso!");
                    LimparDados();
                    ListarProdutos();
                }
                else if (Operacao == "Editar")
                {
                    produtoService.EditarProduto(Convert.ToInt32(idProdView), categoriaInt, marcaInt, descricao, Convert.ToDecimal(precoOnly));
                    MessageBox.Show("Dados atualizados!");
                    AlterarEdicao("Inserir");
                    LimparDados();
                    ListarProdutos();
                }


            }
            catch (Exception er)
            {
                MessageBox.Show($"Erro: {er.Message}");
            }


        }

        public void ListarProdutos()
        {
            var produtoService = new ProdutoService();
            dataGridView1.DataSource = produtoService.ListarProdutos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("Selecione uma linha para editar");
                return;
            }

            var modo = btnEditar.Text;
            if (modo != "Editar")
            {
                AlterarEdicao("Inserir");
            }
            else
            {
                AlterarEdicao("Editar");
                cmbCategoria.Text = dataGridView1.CurrentRow.Cells["CATEGORIA"].Value.ToString();
                cmbMarca.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                txtDescricao.Text = dataGridView1.CurrentRow.Cells["DESCRICAO"].Value.ToString();
                txtPreco.Text = dataGridView1.CurrentRow.Cells["PRECO"].Value.ToString();
                idProdView = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            }


        }

        public void LimparDados()
        {
            cmbCategoria.SelectedIndex = 0;
            cmbMarca.SelectedIndex = 0;
            txtDescricao.Clear();
            txtPreco.Text = "R$ 0,00";
            idProdView = "";
        }
        public void AlterarEdicao(string operacaoTxt)
        {
            Operacao = operacaoTxt;

            switch (Operacao)
            {
                case "Editar":
                    txtOperacao.Text = Operacao;
                    btnSalvar.Text = "Atualizar";
                    btnEditar.Text = "Cancelar Edição";
                    break;
                case "Inserir":
                    LimparDados();
                    idProdView = "";
                    txtOperacao.Text = Operacao;
                    btnSalvar.Text = "Salvar";
                    btnEditar.Text = "Editar";
                    break;
                default:
                    MessageBox.Show("Erro");
                    break;
            }

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("Selecione uma linha para excluir");
                return;
            }
            try
            {

                if (dataGridView1.SelectedRows.Count > 1)
                {
                    DialogResult result = MessageBox.Show("Você tem certeza que quer deletar estes itens?", "Deletar", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                    if (result.ToString() == "OK")
                    {

                        var list = new List<int>();
                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            list.Add(Convert.ToInt32(row.Cells[0].Value));
                        }
                        var countList = list.Count;
                        var produtoService = new ProdutoService();
                        produtoService.ExcluirProduto(list);
                        MessageBox.Show($"{countList} itens excluídos!");
                        idProdView = "";
                        ListarProdutos();
                        return;
                    }

                }
                else
                {
                    DialogResult result = MessageBox.Show("Você tem certeza que quer deletar este item?", "Deletar", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                    if (result.ToString() == "OK")
                    {
                        idProdView = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                        var produtoService = new ProdutoService();
                        produtoService.ExcluirProduto(Convert.ToInt32(idProdView));
                        idProdView = "";
                        MessageBox.Show("Item excluído!");
                        ListarProdutos();
                    }

                }


            }
            catch (Exception er)
            {

                throw er;
            }


        }
    }
}
