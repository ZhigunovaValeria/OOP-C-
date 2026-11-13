namespace Core.Interfaces;

/// <summary>Контракт хранимой сущности; идентификатор назначает репозиторий.</summary>
public interface IEntity : IPrimary
{
    new int Id { get; set; }
}
