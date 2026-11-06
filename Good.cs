namespace ShopApp
{
    // 4. Сущность Good реализует интерфейс IPrimary
    public class Good : IPrimary
    {
        public Good(int id, string name, string code)
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