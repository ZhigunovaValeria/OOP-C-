using System; // Необходимо для работы с DateTime

namespace ShopApp
{
    // 3. Сущность Client реализует интерфейс IPrimary
    public class Client : IPrimary
    {
        public Client(int id, string firstName, string lastName, string middleName, DateTime birthDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            BirthDate = birthDate;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (today < BirthDate.AddYears(age))
                {
                    age--;
                }
                return age;
            }
        }

        public bool IsAdult => Age >= 18;

        public string GetFullName()
        {
            return $"{LastName} {FirstName} {MiddleName}".Trim();
        }
    }
}