using LinqToDB.Mapping;

namespace WebAPIDatabaseLinq2
{
    [Table("employees")]
    public class Employee
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("position")]
        public string Position { get; set; }
        [Column("salary")]
        public decimal Salary { get; set; }
    }
}
