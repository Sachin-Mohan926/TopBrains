// using System;

// public class Student
// {
//     public string Name{get;set;}
//     public int Age{get;set;}
//     public int Marks{get;set;}
// }
// public class MarksComparer : IComparer<Student>
// {
//     public int Compare(Student s1,Student s2)
//     {
//         int res = s2.Marks.CompareTo(s1.Marks);
//         if(res == 0)
//         {
//             return s1.Age.CompareTo(s1.Age);
//         }
//         return res;
//     }
// }
// class Program
// {
//     static void Main(string[] args)
//     {
//         List<Student> st = new List<Student>
//         {
//             new Student(){Name ="Sachin",Age = 24,Marks = 36},
//             new Student(){Name ="Mohan",Age = 23,Marks = 32},
//             new Student(){Name ="Rahul",Age = 35,Marks = 45},
//         };
//         st.Sort(new MarksComparer());
//         foreach(var it in st)
//         {
//             Console.WriteLine($"{it.Name} | {it.Age} | {it.Marks}");
//         }
//     }
// }