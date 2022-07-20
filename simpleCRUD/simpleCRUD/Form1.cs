using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;


namespace simpleCRUD
{
    public partial class Form1 : Form
    {

        //CONEXÃO COM O BANCO DE DADOS
        private string data_source = "datasource=localhost;username=root;password=12345;database=crud";
        private MySqlConnection conn;

        private int ?id_selecionado = null;


        public Form1()
        {
            InitializeComponent();

            listView.View = View.Details;
            listView.LabelEdit = true;
            listView.AllowColumnReorder = true;
            listView.FullRowSelect = true;
            listView.GridLines = true;

            listView.Columns.Add("Id", 50, HorizontalAlignment.Left);
            listView.Columns.Add("Nome", 180, HorizontalAlignment.Left);
            listView.Columns.Add("Sobrenome", 180, HorizontalAlignment.Left);
            listView.Columns.Add("E-mail", 180, HorizontalAlignment.Left);
            listView.Columns.Add("Telefone", 155, HorizontalAlignment.Left);

            carregar_lista();
        }



        //Criar Cadastro
        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            try
            {
                conn = new MySqlConnection(data_source);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;


                if(id_selecionado == null)
                {
                    cmd.CommandText = "INSERT INTO cadastro (nome,sobrenome,email,telefone) " +
                                  "VALUES (@nome,@sobrenome,@email,@telefone)";
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@sobrenome", txtSobrenome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("O cadastro de " + txtNome.Text + " foi salvo com sucesso!",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cmd.CommandText = "UPDATE cadastro SET " +
                        "nome=@nome, sobrenome=@sobrenome, email=@email, telefone=@telefone " +
                        "WHERE id=@id";
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@sobrenome", txtSobrenome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@id", id_selecionado);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("O cadastro de " + txtNome.Text + " foi atualizado com sucesso!",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                id_selecionado = null;
                txtNome.Text = String.Empty;
                txtEmail.Text = String.Empty;
                txtSobrenome.Text = String.Empty;
                txtTelefone.Text = String.Empty;

                carregar_lista();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        //Buscar na DB
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new MySqlConnection(data_source);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM cadastro WHERE nome LIKE @query OR email LIKE @query OR id LIKE @query";
                cmd.Parameters.AddWithValue("@query", "%" + txtBuscar.Text + "%");
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();

                listView.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4)
                    };

                    listView.Items.Add(new ListViewItem(row)); 
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void carregar_lista()
        {
            try
            {
                conn = new MySqlConnection(data_source);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM cadastro ORDER BY id DESC";
                cmd.Prepare();

                MySqlDataReader reader = cmd.ExecuteReader();

                listView.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4)
                    };

                    listView.Items.Add(new ListViewItem(row));
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_sel = listView.SelectedItems;

            foreach (ListViewItem item in itens_sel)
            {
                id_selecionado = Convert.ToInt32(item.SubItems[0].Text);

                txtNome.Text = item.SubItems[1].Text;
                txtSobrenome.Text = item.SubItems[2].Text;
                txtEmail.Text = item.SubItems[3].Text;
                txtTelefone.Text = item.SubItems[4].Text;

                btnExcluir.Visible = true;
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            id_selecionado = null;
            txtNome.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtSobrenome.Text = String.Empty;
            txtTelefone.Text = String.Empty;

            txtNome.Focus();

            btnExcluir.Visible = false;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            excluir_registro();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            excluir_registro();
        }

        private void excluir_registro()
        {
            try
            {
                DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir cadastro?",
                    "Excluir registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (conf == DialogResult.Yes)
                {
                    conn = new MySqlConnection(data_source);
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "DELETE FROM cadastro WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", id_selecionado);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("O cadastro de " + txtNome.Text + " foi EXCLUIDO com sucesso!",
                                    "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    carregar_lista();
                    btnExcluir.Visible = false;
                    id_selecionado = null;
                    txtNome.Text = String.Empty;
                    txtEmail.Text = String.Empty;
                    txtSobrenome.Text = String.Empty;
                    txtTelefone.Text = String.Empty;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}