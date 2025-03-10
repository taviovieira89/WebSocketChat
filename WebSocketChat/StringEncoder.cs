using System;
using System.Text;

public static class StringEncoder
{
    public static string Encode(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("A string de entrada não pode ser nula ou vazia.");
        }

        byte[] bytesToEncode = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(bytesToEncode);
    }

    public static string Decode(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("A string codificada não pode ser nula ou vazia.");
        }

        byte[] decodedBytes = Convert.FromBase64String(input);
        return Encoding.UTF8.GetString(decodedBytes);
    }
}