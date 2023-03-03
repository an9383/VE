using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// clsCommon의 요약 설명입니다.
/// </summary>
public class clsCommon
{
    public static string getString(object obj)
    {
        if (Convert.IsDBNull(obj))
        {
            return "";
        }
        else
        {
            if (obj.GetType().Name == "DateTime")
            {
                DateTime dtime = (DateTime)obj;
                return dtime.ToShortDateString();
            }
            else
            {
                return obj.ToString();
            }
        }
    }

    public static string getInsertString(object obj)
    {
        if (Convert.IsDBNull(obj))
        {
            return "";
        }
        else
        {
            if (obj.GetType().Name == "DateTime")
            {
                DateTime dtime = (DateTime)obj;
                return dtime.ToShortDateString();
            }
            else
            {
                if (obj.ToString() == "")
                {
                    return "null";
                }
                else
                {
                    return "'" + obj.ToString() + "'";
                }
            }

        }
    }

    public static bool IsLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    public static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    public static bool IsSymbol(char c)
    {
        return c > 32 && c < 127 && !IsDigit(c) && !IsLetter(c);
    }

    public static bool IsValidPassword(string password)
    {
        return
           password.Any(c => IsLetter(c)) &&
           password.Any(c => IsDigit(c)) &&
           password.Any(c => IsSymbol(c));
    }

    // SHA256  256bit 암호화
    public static string SHA256Hash(string Data)
    {
        SHA256 sha = new SHA256Managed();
        byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

        StringBuilder stringBuilder = new StringBuilder();

        foreach (byte b in hash)
        {
            stringBuilder.AppendFormat("{0:x2}", b);
        }

        return stringBuilder.ToString();
    }
}
