using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Diagnostics;

namespace PersonApp
{
    // ПУНКТ 1: Сущность, состоящая из 5 полей. 
    // Используем struct вместо class для экстремальной экономии памяти (критично для 10^8)
    public readonly struct Person
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime BirthDate { get; }
        public string Email { get; }

        public Person(int id, string firstName, string lastName, DateTime birthDate, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Email = email;
        }
    }

    public static class PersonManager
    {
        private static readonly string[] FirstNames = { "Алексей", "Мария", "Дмитрий", "Ольга", "Иван", "Елена", "Сергей", "Анна" };
        private static readonly string[] LastNames = { "Иванов", "Петрова", "Сидоров", "Смирнова", "Кузнецов", "Попова", "Соколов", "Морозова" };
        private static readonly string[] FirstNamesLower = { "алексей", "мария", "дмитрий", "ольга", "иван", "елена", "сергей", "анна" };
        private static readonly string[] LastNamesLower = { "иванов", "петрова", "сидоров", "смирнова", "кузнецов", "попова", "соколов", "морозова" };
        private static readonly Random RandomGen = new Random(); // Единый генератор для скорости

        // ПУНКТ 2: Функция-генератор. Параметр: кол-во, Возврат: массив элементов
        public static Person[] GeneratePersons(int count)
        {
            if (count <= 0) return Array.Empty<Person>();

            // Строгая предаллокация массива (работает мгновенно благодаря struct)
            Person[] persons = new Person[count];
            int fnMax = FirstNames.Length;
            int lnMax = LastNames.Length;
            DateTime today = DateTime.Today;

            for (int i = 0; i < count; i++)
            {
                int fnIndex = RandomGen.Next(fnMax);
                int lnIndex = RandomGen.Next(lnMax);
                persons[i] = new Person(
                    i + 1,
                    FirstNames[fnIndex],
                    LastNames[lnIndex],
                    today.AddDays(-RandomGen.Next(18 * 365, 65 * 365)),
                    $"{FirstNamesLower[fnIndex]}.{LastNamesLower[lnIndex]}{i + 1}@example.com"
                );
            }
            return persons;
        }

        // ПУНКТ 3: Запись в текстовый файл
        public static void SavePersonsToFile(Person[] persons, string filePath)
        {
            using var writer = new StreamWriter(filePath, false, Encoding.UTF8, 65536);
            writer.WriteLine("ID;Имя;Фамилия;Дата рождения;Email");
            foreach (Person p in persons)
            {
                writer.Write(p.Id); writer.Write(';');
                writer.Write(p.FirstName); writer.Write(';');
                writer.Write(p.LastName); writer.Write(';');
                writer.Write(p.BirthDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)); writer.Write(';');
                writer.WriteLine(p.Email);
            }
        }

        // ПУНКТ 4: Запись в бинарный файл
        public static void SavePersonsToBinaryFile(Person[] persons, string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 65536);
            using var writer = new BinaryWriter(fs, Encoding.UTF8);
            writer.Write(persons.Length);
            foreach (Person p in persons)
            {
                writer.Write(p.Id);
                writer.Write(p.FirstName);
                writer.Write(p.LastName);
                writer.Write(p.BirthDate.Ticks);
                writer.Write(p.Email);
            }
        }

        // ПУНКТ 5: Считывание из текстового файла
        public static Person[] LoadPersonsFromFile(string filePath, int expectedCount)
        {
            if (!File.Exists(filePath)) return Array.Empty<Person>();
            Person[] persons = new Person[expectedCount];
            int index = 0;

            using var reader = new StreamReader(filePath, Encoding.UTF8, true, 65536);
            reader.ReadLine(); // Пропуск заголовка
            string? line;

            while ((line = reader.ReadLine()) != null && index < expectedCount)
            {
                int p1 = line.IndexOf(';');
                int p2 = line.IndexOf(';', p1 + 1);
                int p3 = line.IndexOf(';', p2 + 1);
                int p4 = line.IndexOf(';', p3 + 1);

                if (p1 > 0 && p2 > p1 && p3 > p2 && p4 > p3)
                {
                    persons[index++] = new Person(
                        int.Parse(line.AsSpan(0, p1)),
                        line.Substring(p1 + 1, p2 - p1 - 1),
                        line.Substring(p2 + 1, p3 - p2 - 1),
                        DateTime.ParseExact(line.AsSpan(p3 + 1, p4 - p3 - 1), "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        line.Substring(p4 + 1)
                    );
                }
            }
            return persons;
        }

        // ПУНКТ 6: Считывание из бинарного файла
        public static Person[] LoadPersonsFromBinaryFile(string filePath, int expectedCount)
        {
            if (!File.Exists(filePath)) return Array.Empty<Person>();
            Person[] persons = new Person[expectedCount];

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 65536);
            using var reader = new BinaryReader(fs, Encoding.UTF8);

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                persons[i] = new Person(
                    reader.ReadInt32(),
                    reader.ReadString(),
                    reader.ReadString(),
                    new DateTime(reader.ReadInt64()),
                    reader.ReadString()
                );
            }
            return persons;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ВЫПОЛНЕНИЕ TASK 0 (Release + x64) ===");
            Console.WriteLine("Используется struct для работы с 10^8 без OutOfMemory.\n");

            // ПУНКТ 7: Строго заданные количества элементов из задания
            int[] counts = { 10, 1000, 100000, 1000000, 10000000, 100000000 };

            string txtFile = "benchmark_data.txt";
            string binFile = "benchmark_data.bin";

            // Прогрев (JIT-компиляция) для чистоты замеров
            Console.WriteLine("Прогрев системы...");
            Person[] warmup = PersonManager.GeneratePersons(100);
            PersonManager.SavePersonsToFile(warmup, txtFile);
            PersonManager.LoadPersonsFromFile(txtFile, 100);
            PersonManager.SavePersonsToBinaryFile(warmup, binFile);
            PersonManager.LoadPersonsFromBinaryFile(binFile, 100);
            Console.WriteLine("Прогрев завершен. Запуск бенчмарка.\n");

            // Заголовок для Excel (разделитель - табуляция \t)
            Console.WriteLine("Элементов\tЧтение TXT (мс)\tЧтение BIN (мс)");
            Console.WriteLine(new string('-', 60));

            foreach (int count in counts)
            {
                Console.Write($"{count,-8}\t");

                try
                {
                    long readTxt = 0, readBin = 0;
                    Stopwatch sw = new Stopwatch();

                    // Очистка памяти перед тестом
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    // Генерация и запись (подготовка данных, не замеряем)
                    Person[] data = PersonManager.GeneratePersons(count);
                    PersonManager.SavePersonsToFile(data, txtFile);
                    PersonManager.SavePersonsToBinaryFile(data, binFile);
                    data = null; // Освобождаем память перед чтением
                    GC.Collect();

                    // Замер чтения TXT
                    sw.Restart();
                    PersonManager.LoadPersonsFromFile(txtFile, count);
                    readTxt = sw.ElapsedMilliseconds;

                    GC.Collect();

                    // Замер чтения BIN
                    sw.Restart();
                    PersonManager.LoadPersonsFromBinaryFile(binFile, count);
                    readBin = sw.ElapsedMilliseconds;

                    Console.WriteLine($"{readTxt}\t\t{readBin}");
                }
                catch (OutOfMemoryException)
                {
                    // Если на очень слабом ПК 10^8 все же не влезет, таблица не сломается
                    Console.WriteLine("OOM\t\tOOM");
                }
            }

            Console.WriteLine("\n==========================================================");
            Console.WriteLine("ИНСТРУКЦИЯ ДЛЯ ПОСТРОЕНИЯ ГРАФИКА В EXCEL:");
            Console.WriteLine("1. Выделите мышкой таблицу выше (от 'Элементов' до последних цифр)");
            Console.WriteLine("2. Нажмите Ctrl+C");
            Console.WriteLine("3. Откройте Excel, выделите ячейку A1, нажмите Ctrl+V");
            Console.WriteLine("4. Выделите вставленные данные -> Вставка -> График (или Гистограмма)");
            Console.WriteLine("==========================================================");
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}