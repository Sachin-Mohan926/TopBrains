// using System;

// class Program
// {
//     static void Main(string[] args)
//     {
//         Console.WriteLine("Enter First Word: ");
//         string str1 = Console.ReadLine();
//         Console.WriteLine("Enter Second Word: ");
//         string str2 = Console.ReadLine();
//         string str3 = Remove_Common_Consonants(str1.ToLower(),str2.ToLower());
//         Remove_Consecutive_Duplicate_Characters(str3);
//     }
//     static string Remove_Common_Consonants(string str1,string str2)
//     {
//         HashSet<char> arr = new HashSet<char>();
//         for(int i = 0; i < str2.Length; i++)
//         {
//             if(str2[i] != 'a' && str2[i] != 'e' && str2[i] != 'i' && str2[i] != 'o' && str2[i] != 'u')
//             {
//                 arr.Add(str2[i]);
//             }
//         }
//         string str3 = "";
//         for(int i = 0; i < str1.Length; i++)
//         {
//             if (!arr.Contains(str1[i]))
//             {
//                 str3+= str1[i];
//             }
//         }
//         return str3;
//     }
//     static void Remove_Consecutive_Duplicate_Characters(string str3)
//     {
//         HashSet<char> arr2 = new HashSet<char>();
//         for(int i = 0; i < str3.Length; i++)
//         {
//             arr2.Add(str3[i]);
//         }
//         Console.WriteLine("Output: ");
//         foreach(var ch in arr2)
//         {
//             Console.Write($"{ch}");
//         }
//     }
// }