namespace IPRep
{
    using System.Threading.Tasks;
    class Program
    {
        public static async Task Main(string[] args)
        {
            await IPRep.ProcessArgsAsync(args);
        }
    }
}