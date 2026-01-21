using System;
class DisplayHeight
{
    public static void Main(string[] args)
    {
        int heightInCm;
        Console.WriteLine("Enter height in cm: ");
        heightInCm = Convert.ToInt32(Console.ReadLine());

        if(heightInCm > 180)
        {
            Console.WriteLine("Tall");
        }
        else if(heightInCm >= 150 && heightInCm < 180)
        {
            Console.WriteLine("Average");
        }
        else
        {
            Console.WriteLine("Short");
        }
    }
}