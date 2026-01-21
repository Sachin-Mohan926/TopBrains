using System;
using Microsoft.VisualBasic;
class LargestInteger
{
    public static void Main(string[] args)
    {
        int firstNumber;
        Console.WriteLine("Enter the first Number: ");
        firstNumber = Convert.ToInt32(Console.ReadLine());
        int SecondNumber;
        Console.WriteLine("Enter the second Number: ");
        SecondNumber = Convert.ToInt32(Console.ReadLine());
        int thirdNumber;
        Console.WriteLine("Enter the third NUmber: ");
        thirdNumber = Convert.ToInt32(Console.ReadLine());

        if((firstNumber > SecondNumber) && (firstNumber > thirdNumber))
        {
            Console.WriteLine($"{firstNumber} is the largest of all");
        }
        else if((SecondNumber >firstNumber) && (SecondNumber > thirdNumber))
        {
            Console.WriteLine($"{SecondNumber} is the largest of all");
        }
        else
        {
            Console.WriteLine($"{thirdNumber} is the largest of all");
        }
    }
}