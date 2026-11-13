using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Core.Interfaces;

namespace Shop.Dal.Entities;

[Table("clients")]
public class ClientDao : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("id")]
    public int Id { get; set; }

    [Required, MaxLength(100), Column("first_name")]
    public string FirstName { get; set; } = "";

    [Required, MaxLength(100), Column("last_name")]
    public string LastName { get; set; } = "";

    [MaxLength(100), Column("middle_name")]
    public string? MiddleName { get; set; }

    [Column("birth_date")]
    public DateTime BirthDate { get; set; } = new(1900, 1, 1);

    [NotMapped, JsonIgnore, XmlIgnore]
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age))
                age--;
            return age;
        }
    }
}
