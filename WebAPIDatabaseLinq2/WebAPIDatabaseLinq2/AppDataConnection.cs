using LinqToDB;
using LinqToDB.Data;

namespace WebAPIDatabaseLinq2
{
    public class AppDataConnection : DataConnection
    {
        public AppDataConnection(string str) : base(str) { }
        public AppDataConnection(DataOptions<AppDataConnection> option) : base(option.Options) { }
    }
}
