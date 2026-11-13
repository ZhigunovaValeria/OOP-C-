namespace Core.Validation;

public static class Guard
{
    public static string NotBlank(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Значение должно содержать хотя бы один символ.", parameterName);
        return value;
    }
}
