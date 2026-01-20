using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string str = Console.ReadLine();
        Console.WriteLine($"String: {fun(str)}");
    }

    static string fun(string str)
    {
        StringBuilder st = new StringBuilder();
        st.Append(str[0]);

        for (int i = 1; i < str.Length; i++)
        {
            if (str[i] != str[i - 1])
            {
                st.Append(str[i]);
            }
        }

        // Capitalize first character
        char firstChar = char.ToUpper(st[0]);
        st.Remove(0, 1);
        st.Insert(0, firstChar);

        // Capitalize character after space
        for (int i = 0; i < st.Length - 1; i++)
        {
            if (st[i] == ' ')
            {
                char firstChar1 = char.ToUpper(st[i + 1]);
                st.Remove(i + 1, 1);
                st.Insert(i + 1, firstChar1);
            }
        }

        return st.ToString().Trim();
    }
}