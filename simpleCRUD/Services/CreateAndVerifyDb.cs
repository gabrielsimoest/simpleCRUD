using System.Data.SQLite;

namespace simpleCRUD.Services
{
    public class CreateAndVerifyDb
    {
        private string _connectionString;

        public CreateAndVerifyDb(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public void VerifyIfExistsDB()
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Criação da tabela de tarefas
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Cadastro (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nome NVARCHAR(100),
                            Sobrenome NVARCHAR(100),
                            Email NVARCHAR(100),
                            Telefone NVARCHAR(20)
                        );";
                SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection);
                createTableCommand.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
