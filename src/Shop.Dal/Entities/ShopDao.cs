using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Interfaces;

namespace Shop.Dal.Entities;

[Table("shops")]
public class ShopDao : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("id")]
    public int Id { get; set; }

    [Required, MaxLength(200), Column("name")]
    public string Name { get; set; } = "";

    [Required, MaxLength(50), Column("code")]
    public string Code { get; set; } = "";
}
