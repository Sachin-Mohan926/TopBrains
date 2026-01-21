using System;
class SumOfPositiveIntegers
{
    public static void Main(string[] args)
    {
        int n = 0;
        n = Convert.ToInt32(Console.ReadLine());
        int[] arr = new int[n];
        for(int i = 0; i<n; i++)
        {
            arr[i] = Convert.ToInt32(Console.ReadLine());
        }
        int sum = 0;
        for(int i = 0; i<n; i++)
        {
            if (arr[i] == 0)
            {
                break;
            }
            if(arr[i] < 0)
            {
                continue;
            }
            else
            {
                sum = sum+arr[i];
            }
        }
        Console.WriteLine($"Sum is: {sum}");
    }
}