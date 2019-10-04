using System;

public class PatternExpressions
{
    // Switch Expression
    public static RGBColor FromRainbow(Rainbow colorBand) =>
        colorBand switch
        {
            Rainbow.Red => new RGBColor(0xFF, 0x00, 0x00),
            Rainbow.Orange => new RGBColor(0xFF, 0x7F, 0x00),
            Rainbow.Yellow => new RGBColor(0xFF, 0xFF, 0x00),
            Rainbow.Green => new RGBColor(0x00, 0xFF, 0x00),
            Rainbow.Blue => new RGBColor(0x00, 0x00, 0xFF),
            Rainbow.Indigo => new RGBColor(0x4B, 0x00, 0x82),
            Rainbow.Violet => new RGBColor(0x94, 0x00, 0xD3),
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand)),
        };

    // Property Pattern
    public static decimal ComputeSalesTax(Address location, decimal salePrice) =>
        location switch
        {
            { State: "WA" } => salePrice * 0.06M,
            { State: "MN" } => salePrice * 0.75M,
            { State: "MI" } => salePrice * 0.05M,
            // other cases removed for brevity...
            _ => 0M
        };

    // Tuple Pattern
    public static string RockPaperScissors(string first, string second)
        => (first, second) switch
        {
            ("rock", "paper") => "rock is covered by paper. Paper wins.",
            ("rock", "scissors") => "rock breaks scissors. Rock wins.",
            ("paper", "rock") => "paper covers rock. Paper wins.",
            ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
            ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
            ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
            (_, _) => "tie"
        };

    // Positional patterns
    public static Quadrant GetQuadrant(Point point) => point switch
    {
        (0, 0) => Quadrant.Origin,
        var (x, y) when x > 0 && y > 0 => Quadrant.One,
        var (x, y) when x < 0 && y > 0 => Quadrant.Two,
        var (x, y) when x < 0 && y < 0 => Quadrant.Three,
        var (x, y) when x > 0 && y < 0 => Quadrant.Four,
        var (_, _) => Quadrant.OnBorder,
        _ => Quadrant.Unknown
    };

    public static void Run()
    {
        // Advanced
        var tollCalc = new TollCalculator();

        var car = new Car();
        var taxi = new Taxi();
        var bus = new Bus();
        var truck = new DeliveryTruck();

        Console.WriteLine($"The toll for a car is {tollCalc.CalculateToll(car)}");
        Console.WriteLine($"The toll for a taxi is {tollCalc.CalculateToll(taxi)}");
        Console.WriteLine($"The toll for a bus is {tollCalc.CalculateToll(bus)}");
        Console.WriteLine($"The toll for a truck is {tollCalc.CalculateToll(truck)}");

        try
        {
            tollCalc.CalculateToll("this will fail");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Caught an argument exception when using the wrong type");
        }
        try
        {
            tollCalc.CalculateToll(null);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("Caught an argument exception when using null");
        }
    }
}

public class Car
{
    public int Passengers { get; set; }
}

public class DeliveryTruck
{
    public int GrossWeightClass { get; set; }
}

public class Taxi
{
    public int Fares { get; set; }
}

public class Bus
{
    public int Capacity { get; set; }
    public int Riders { get; set; }
}

public class TollCalculator
{
    public decimal CalculateToll(object vehicle) =>
        vehicle switch
        {
            Car c => 2.00m,
            // Car { Passengers: 0 } => 2.00m + 0.50m,
            // Car { Passengers: 1 } => 2.0m,
            // Car { Passengers: 2 } => 2.0m - 0.50m,
            // Car c => 2.00m - 1.0m,

            Taxi t => 3.50m,
            // Taxi { Fares: 0 } => 3.50m + 1.00m,
            // Taxi { Fares: 1 } => 3.50m,
            // Taxi { Fares: 2 } => 3.50m - 0.50m,
            // Taxi t => 3.50m - 1.00m,

            Bus b => 5.00m,
            // Bus b when ((double)b.Riders / (double)b.Capacity) < 0.50 => 5.00m + 2.00m,
            // Bus b when ((double)b.Riders / (double)b.Capacity) > 0.90 => 5.00m - 1.00m,
            // Bus b => 5.00m,

            DeliveryTruck t => 10.00m,
            // DeliveryTruck t when (t.GrossWeightClass > 5000) => 10.00m + 5.00m,
            // DeliveryTruck t when (t.GrossWeightClass < 3000) => 10.00m - 2.00m,
            // DeliveryTruck t => 10.00m,
            { } => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle)),
            null => throw new ArgumentNullException(nameof(vehicle))
        };
    private static bool IsWeekDay(DateTime timeOfToll) =>
        timeOfToll.DayOfWeek switch
        {
            DayOfWeek.Monday => true,
            DayOfWeek.Tuesday => true,
            DayOfWeek.Wednesday => true,
            DayOfWeek.Thursday => true,
            DayOfWeek.Friday => true,
            DayOfWeek.Saturday => false,
            DayOfWeek.Sunday => false
            // _ => true
        };

    private enum TimeBand
    {
        MorningRush,
        Daytime,
        EveningRush,
        Overnight
    }

    private static TimeBand GetTimeBand(DateTime timeOfToll)
    {
        int hour = timeOfToll.Hour;
        if (hour < 6)
            return TimeBand.Overnight;
        else if (hour < 10)
            return TimeBand.MorningRush;
        else if (hour < 16)
            return TimeBand.Daytime;
        else if (hour < 20)
            return TimeBand.EveningRush;
        else
            return TimeBand.Overnight;
    }

    public decimal PeakTimePremiumFull(DateTime timeOfToll, bool inbound) =>
    (IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
    {
        (true, TimeBand.MorningRush, true) => 2.00m,
        (true, TimeBand.MorningRush, false) => 1.00m,
        (true, TimeBand.Daytime, true) => 1.50m,
        (true, TimeBand.Daytime, false) => 1.50m,
        (true, TimeBand.EveningRush, true) => 1.00m,
        (true, TimeBand.EveningRush, false) => 2.00m,
        (true, TimeBand.Overnight, true) => 0.75m,
        (true, TimeBand.Overnight, false) => 0.75m,
        (false, TimeBand.MorningRush, true) => 1.00m,
        (false, TimeBand.MorningRush, false) => 1.00m,
        (false, TimeBand.Daytime, true) => 1.00m,
        (false, TimeBand.Daytime, false) => 1.00m,
        (false, TimeBand.EveningRush, true) => 1.00m,
        (false, TimeBand.EveningRush, false) => 1.00m,
        (false, TimeBand.Overnight, true) => 1.00m,
        (false, TimeBand.Overnight, false) => 1.00m,
        // (true, TimeBand.Overnight,   _)     => 0.75m,
        // (true, TimeBand.Daytime,     _)     => 1.5m,
        // (true, TimeBand.MorningRush, true)  => 2.0m,
        // (true, TimeBand.EveningRush, false) => 2.0m,
        // (_,    _,                    _)     => 1.0m,
    };
}