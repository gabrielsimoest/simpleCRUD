using simpleCRUD.Services;

namespace simpleCRUD
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Method to Create Sqlite Database
            var verify = new CreateAndVerifyDb("DataSource=SimpleCrud.db");
            verify.VerifyIfExistsDB();

            ApplicationConfiguration.Initialize();
            Application.Run(new SimpleCrud());
        }
    }
}