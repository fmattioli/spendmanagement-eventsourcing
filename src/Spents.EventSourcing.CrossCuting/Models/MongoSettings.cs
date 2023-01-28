namespace Spents.EventSourcing.CrossCuting.Models
{
    public class MongoSettings
    {
        public string Database { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}
