using SQLiteSample.DataAccess.Helper;

namespace SQLiteSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string tableName = "firstTable";
            tableName.CreateTable();
            tableName.Get();
        }
    }
}