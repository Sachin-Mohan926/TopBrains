using System;
class Program
{
    static int add(int num)
    {
        int sum = 0;
        while (num > 0)
        {
            sum += num%10;
            num/=10;
        }
        return sum;
    }
    static void Main(string[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine());
        int m = Convert.ToInt32(Console.ReadLine());
        int count = 0;
        for(int i = n; i <= m; i++)
        {
            int sum1 = add(i) * add(i);
            int sum2 = add(i*i);
            if(sum1 == sum2) count++; 
        }
        Console.WriteLine($"Count: {count}");
    }
}