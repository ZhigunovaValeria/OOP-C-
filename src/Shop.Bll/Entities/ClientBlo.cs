using Core.Interfaces;
using Core.Validation;

namespace Shop.Bll.Entities;

public class ClientBlo : IPrimary
{
    private string _firstName;
    private string _lastName;
    private DateTime _birthday;

    public ClientBlo(int id, string firstName, string lastName, string? patronymic = null)
    {
        Id = id;
        _firstName = Guard.NotBlank(firstName, nameof(firstName));
        _lastName = Guard.NotBlank(lastName, nameof(lastName));
        Patronymic = patronymic;
        Birthday = new DateTime(1900, 1, 1);
    }

    public int Id { get; private set; }

    public string FirstName
    {
        get => _firstName;
        set => _firstName = Guard.NotBlank(value, nameof(FirstName));
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = Guard.NotBlank(value, nameof(LastName));
    }

    public string? Patronymic { get; set; }

    public DateTime Birthday
    {
        get => _birthday;
        set => _birthday = value == default
            ? throw new ArgumentException("Дата рождения не должна быть пустой.", nameof(Birthday))
            : value.Date;
    }
}
