namespace ShopApp
{
    // 2. Сущность Shop реализует интерфейс IPrimary
    public class Shop : IPrimary
    {
        public Shop(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}