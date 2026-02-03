// using System;
// using System.Reflection.Metadata;
// using Microsoft.VisualBasic;
// class MultiplicationTable
// {
//     public static void Main(string[] args)
//     {
//         int n;
//         int upto;
//         Console.WriteLine("Enter the number: ");
//         n = Convert.ToInt32(Console.ReadLine());
//         Console.WriteLine("Enter the limit upto which the multiples are required: ");
//         upto = Convert.ToInt32(Console.ReadLine());

//         int[] MultiplicationArray = new int[upto];
//         for(int i = 0; i<upto; i++)
//         {
//             MultiplicationArray[i] = n*(i+1);
//         }
//         for(int i = 0; i < upto; i++)
//         {
//             Console.Write(MultiplicationArray[i]);
//         }
//     }
// }