using System.IO;
namespace Formular.DataSources
{
    public enum ProductState
    {
        Found,
        Invalid,
        FoundInSource
    }

    public static class Products
    {
        private static readonly string fileName = "FisierProduse.txt";
        public static string[] Entries { get; private set; }

        static Products()
        {
            Entries = File.ReadAllLines(fileName);
        }
    }
}
