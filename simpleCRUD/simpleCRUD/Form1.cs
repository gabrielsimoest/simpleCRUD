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

        MySqlConnection conn;


        public Form1()
        {
            InitializeComponent();
        }


        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            try
            {
                string data_source = "datasource=localhost;username=root;password=12345;database=crud";
            
                conn = new MySqlConnection(data_source);

                string sql = "INSERT INTO cadastro (nome,sobrenome,email,telefone) " +
                             "VALUES('" + txtNome.Text + "', '" + txtSobrenome.Text + "', '" + txtEmail.Text + "', '" + txtTelefone.Text + "') ";

                MySqlCommand comando = new MySqlCommand(sql, conn);

                conn.Open();

                comando.ExecuteReader();

                MessageBox.Show("Deu tudo Certo, Inserido");
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
    }
}