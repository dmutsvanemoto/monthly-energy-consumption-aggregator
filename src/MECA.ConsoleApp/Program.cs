﻿using System.Threading.Tasks;
using MECA.ConsoleApp.Services;

namespace MECA.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var aggregator = new DataAggregatorService(new FileService(), new MonthlyAggregatorService());

            await aggregator.Aggregate();
        }
    }
}

