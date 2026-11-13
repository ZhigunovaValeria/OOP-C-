using Core.Interfaces;
using Core.Validation;

namespace Shop.Bll.Entities;

public class GoodBlo : IPrimary
{
    private string _name;
    private string _code;

    public GoodBlo(int id, string name, string code)
    {
        Id = id;
        _name = Guard.NotBlank(name, nameof(name));
        _code = Guard.NotBlank(code, nameof(code));
    }

    public int Id { get; private set; }

    public string Name
    {
        get => _name;
        set => _name = Guard.NotBlank(value, nameof(Name));
    }

    public string Code
    {
        get => _code;
        set => _code = Guard.NotBlank(value, nameof(Code));
    }
}
