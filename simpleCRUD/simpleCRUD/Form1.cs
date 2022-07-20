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
        }



        //Criar Cadastro
        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            try
            {
                conn = new MySqlConnection(data_source);

                string sql = "INSERT INTO cadastro (nome,sobrenome,email,telefone) " +
                             "VALUES('" + txtNome.Text + "', '" + txtSobrenome.Text + "', '" + txtEmail.Text + "', '" + txtTelefone.Text + "') ";

                MySqlCommand comando = new MySqlCommand(sql, conn);

                conn.Open();

                comando.ExecuteReader();

                MessageBox.Show("Os dados de " + txtNome.Text + " foram salvos com sucesso: ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                string query = "'%" + txtBuscar.Text + "%'";

                conn = new MySqlConnection(data_source);

                string sql = "SELECT * FROM cadastro " +
                            "WHERE nome LIKE " + query + "OR email LIKE" + query + "OR id LIKE" + query;

                conn.Open();

                MySqlCommand comando = new MySqlCommand(sql, conn);

                MySqlDataReader reader = comando.ExecuteReader();

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

                    var l_listview = new ListViewItem(row);

                    listView.Items.Add(l_listview); 
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}