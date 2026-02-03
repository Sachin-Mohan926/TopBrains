using System;

public class RobotSafetyException : Exception
{
    public RobotSafetyException(string message) : base(message)
    {
    }
}

public class RobotHazardAuditor
{
    public double CalculateHazardRisk(double armPrecision, int workerDensity, string machineryState)
    {
        if (armPrecision < 0.0 || armPrecision > 1.0)
            throw new RobotSafetyException("Error: Arm precision must be 0.0-1.0");

        if (workerDensity < 1 || workerDensity > 20)
            throw new RobotSafetyException("Error: Worker density must be 1-20");

        double riskFactor;

        if (machineryState == "Worn")
            riskFactor = 1.3;
        else if (machineryState == "Faulty")
            riskFactor = 2.0;
        else if (machineryState == "Critical")
            riskFactor = 3.0;
        else
            throw new RobotSafetyException("Error: Unsupported machinery state");

        return ((1.0 - armPrecision) * 15.0) + (workerDensity * riskFactor);
    }
}

public class Program
{
    public static void Main()
    {
        try
        {
            Console.WriteLine("Enter Arm Precision (0.0 - 1.0):");
            double precision = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Enter Worker Density (1 - 20):");
            int density = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Machinery State (Worn/Faulty/Critical):");
            string state = Console.ReadLine();

            RobotHazardAuditor auditor = new RobotHazardAuditor();
            double score = auditor.CalculateHazardRisk(precision, density, state);

            Console.WriteLine("Robot Hazard Risk Score: " + score);
        }
        catch (RobotSafetyException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}