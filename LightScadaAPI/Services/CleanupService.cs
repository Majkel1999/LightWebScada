using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;

/// <summary>
/// Table cleanup service.
/// Runs periodically and ensures that all data tables have maximum MaximumRows rows.
/// </summary>
public class CleanupService : BackgroundService
{
    /// <summary>
    /// Maximum rows per data table per organization
    /// </summary>
    private const int MaximumRows = 500;

    private string m_connectionString;

    public CleanupService(IConfiguration configuration)
    {
        m_connectionString = configuration.GetConnectionString("UserContextConnection");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CheckTablesPerodically(stoppingToken);
    }

    /// <summary>
    /// Method works for the whole lifetime of the hosted API.
    /// </summary>
    /// <param name="stoppingToken">CancellationToken for stopping the service</param>
    /// <returns>Awaitable task</returns>
    private async Task CheckTablesPerodically(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using IDbConnection db = new NpgsqlConnection(m_connectionString);
            List<int> ids = db.Query<int>(@"SELECT ""OrganizationId"" FROM common.organization").ToList();
            foreach (int id in ids)
            {
                try
                {
                    int count = db.Query<int>($@"SELECT count(*) FROM public.org_{id}_data").First();
                    if (count > MaximumRows)
                    {
                        int countToDelete = count - MaximumRows + 50;
                        Console.WriteLine($"Table org_{id}_data has {count} rows / {countToDelete} to be removed");
                        db.Query($@"DELETE FROM public.org_{id}_data WHERE ""Id"" IN (
                        SELECT ""Id"" FROM public.org_{id}_data 
                        ORDER BY ""Timestamp""
                        LIMIT {countToDelete})");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception " + e.Message);
                    continue;
                }
            }
            Console.WriteLine(DateTime.Now + " : Tables cleaned up");
            await Task.Delay(300000, stoppingToken);
        }
    }
}