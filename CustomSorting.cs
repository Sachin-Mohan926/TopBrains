using System;

public class Student
{
    public string Name{get;set;}
    public int Age{get;set;}
    public int Marks{get;set;}
}
public class MarksComparer : IComparer<Student>
{
    public int Compare(Student s1,Student s2)
    {
        int res = s2.Marks.CompareTo(s1.Marks);
        if(res == 0)
        {
            return s1.Age.CompareTo(s1.Age);
        }
        return res;
    }
}
class Program
{
    static void Main(string[] args)
    {
        List<Student> st = new List<Student>
        {
            new Student(){Name ="Ankit",Age = 20,Marks = 30},
            new Student(){Name ="Rohan",Age = 21,Marks = 30},
            new Student(){Name ="Sohan",Age = 30,Marks = 40},
        };
        st.Sort(new MarksComparer());
        foreach(var it in st)
        {
            Console.WriteLine($"{it.Name} | {it.Age} | {it.Marks}");
        }
    }
}