using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public abstract class BaseFileRepository<T>
{
    protected string FileName;

    public BaseFileRepository(string fileName)
    {
        FileName = fileName;
    }

    public List<T> ReadAll()
    {
        List<T> items = new List<T>();
        if (!File.Exists(FileName)) return items;

        string[] lines = File.ReadAllLines(FileName);
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                items.Add(ParseLine(line));
            }
        }
        return items;
    }

    public void Create(T item)
    {
        using (StreamWriter writer = File.AppendText(FileName))
        {
            writer.WriteLine(ToLine(item));
        }
    }

    public abstract void Update(T item);

    public void Delete(T item)
    {
        List<T> allItems = ReadAll();
        allItems.RemoveAll(i => IsSame(i, item));
        SaveAll(allItems);
    }

    protected void SaveAll(List<T> items)
    {
        using (StreamWriter writer = new StreamWriter(FileName, false))
        {
            foreach (T item in items)
            {
                writer.WriteLine(ToLine(item));
            }
        }
    }

    protected abstract T ParseLine(string line);
    protected abstract string ToLine(T item);
    protected abstract bool IsSame(T existing, T target);
}