public class GoodDao
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }

    public GoodDao(int id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
    }
}