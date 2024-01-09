using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace simpleCRUD
{
    public partial class SimpleCrud : System.Windows.Forms.Form
    {
        private string _connectionString = "DataSource=SimpleCrud.db;";

        private long? _selectedId = null;

        public SimpleCrud()
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

            LoadData();
        }

        private void ResetForm()
        {
            _selectedId = null;
            txtNome.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSobrenome.Text = string.Empty;
            txtTelefone.Text = string.Empty;
        }

        private void Create(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = conn;

                    if (txtNome.Text == "")
                    {
                        MessageBox.Show("O campo de NOME não pode ser enviado em branco ",
                            "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (txtEmail.Text == "")
                    {
                        MessageBox.Show("O campo de E-MAIL pode ser enviado em branco ",
                            "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (txtTelefone.Text == "(    )       - ")
                    {
                        MessageBox.Show("O campo de TELEFONE pode ser enviado em branco ",
                            "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (_selectedId == null)
                        {
                            cmd.CommandText = "INSERT INTO cadastro (nome,sobrenome,email,telefone) " +
                                              "VALUES (@nome,@sobrenome,@email,@telefone)";
                            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                            cmd.Parameters.AddWithValue("@sobrenome", txtSobrenome.Text);
                            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                            cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
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
                            cmd.Parameters.AddWithValue("@id", _selectedId);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("O cadastro de " + txtNome.Text + " foi atualizado com sucesso!",
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    ResetForm();
                    LoadData();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Erro ocorreu: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM cadastro ORDER BY id DESC", conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    listView.Items.Clear();

                    while (reader.Read())
                    {
                        string[] row = {
                            reader.GetInt64(0).ToString(),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                        };

                        listView.Items.Add(new ListViewItem(row));
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Erro ocorreu: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListViewOnChange(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_sel = listView.SelectedItems;

            foreach (ListViewItem item in itens_sel)
            {
                _selectedId = Convert.ToInt32(item.SubItems[0].Text);

                txtNome.Text = item.SubItems[1].Text;
                txtSobrenome.Text = item.SubItems[2].Text;
                txtEmail.Text = item.SubItems[3].Text;
                txtTelefone.Text = item.SubItems[4].Text;
            }

            btnExcluir.Visible = true;
        }

        private void NewForm(object sender, EventArgs e)
        {
            ResetForm();
            txtNome.Focus();
            btnExcluir.Visible = false;
        }

        private void DeleteRecord(object sender, EventArgs e)
        {
            try
            {
                DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir cadastro?",
                    "Excluir registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (conf == DialogResult.Yes)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                    {
                        conn.Open();
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.Connection = conn;

                        cmd.CommandText = "DELETE FROM cadastro WHERE id=@id";
                        cmd.Parameters.AddWithValue("@id", _selectedId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("O cadastro de " + txtNome.Text + " foi EXCLUIDO com sucesso!",
                                        "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        btnExcluir.Visible = false;
                        ResetForm();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Erro ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Search(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "SELECT * FROM cadastro WHERE UPPER(nome) LIKE UPPER(@query) OR UPPER(email) LIKE UPPER(@query) OR UPPER(id) LIKE UPPER(@query)";

                    cmd.Parameters.AddWithValue("@query", "%" + txtBuscar.Text + "%");

                    SQLiteDataReader reader = cmd.ExecuteReader();

                    listView.Items.Clear();

                    while (reader.Read())
                    {
                        string[] row =
                            {
                                reader.GetInt64(0).ToString(),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4)
                            };

                        listView.Items.Add(new ListViewItem(row));
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Erro ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidateEmail(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                Regex reg = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                if (!reg.IsMatch(txtEmail.Text))
                {
                    errorProvider1.SetError(this.txtEmail, "Insira um E-mail valido");
                }
                else
                {
                    errorProvider1.Clear();
                }
            }

        }
    }
}