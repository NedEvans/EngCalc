using EngineeringCalc.Data;
using Microsoft.EntityFrameworkCore;

namespace EngineeringCalc;

public static class ResetData
{
    public static async Task DeleteAllTestData(ApplicationDbContext context)
    {
        // Delete in correct order due to foreign keys
        await context.Database.ExecuteSqlRawAsync("DELETE FROM CardInstances");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM CalculationRevisions");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Calculations");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM GlobalConstants");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Jobs");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Projects");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Cards");

        Console.WriteLine("✓ All test data deleted. Application will reseed on next startup.");
    }
}
