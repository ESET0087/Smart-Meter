using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;
using System.Text.Json;

namespace smart_meter.Services
{
    public class EnergyMeasurementServices
    {
        public readonly AppDbContext _context;

        public EnergyMeasurementServices(AppDbContext context)
        {
            _context = context;
        }

        // Method to insert a new meter reading
        public async Task<Meterreading> AddMeterReadingAsync(MeterReading dto)
        {
            // Create new entity from DTO
            var meterReading = new Meterreading
            {
                Meterserialno = dto.Meterserialno,
                Readingdatetime = dto.Readingdatetime,
                Energyconsumed = dto.Energyconsumed,
                Voltage = dto.Voltage,
                Current = dto.Current
            };

            // Add entity to DB context
            _context.Meterreadings.Add(meterReading);

            // Save changes asynchronously
            await _context.SaveChangesAsync();

            return meterReading;
        }

        // to find energy consumption
        public async Task<ResultsDto> EnergyConsumtion(EnergyConsumtionMeasurment energyDto)
        {
            try
            {
                // --- Find Readings Within the Range ---
                var readingsInRange = await _context.Meterreadings
                    .Where(m => m.Meterserialno == energyDto.meterserialno &&
                                m.Readingdatetime >= energyDto.Datefrom &&
                                m.Readingdatetime <= energyDto.Dateto)
                    .OrderBy(m => m.Readingdatetime)
                    .ToListAsync(); 

                // --- Find Bounding Readings (if they exist) ---
                var startBound = await _context.Meterreadings
                    .Where(m => m.Meterserialno == energyDto.meterserialno && m.Readingdatetime < energyDto.Datefrom) // Strictly BEFORE
                    .OrderByDescending(m => m.Readingdatetime)
                    .FirstOrDefaultAsync();

                var endBound = await _context.Meterreadings
                    .Where(m => m.Meterserialno == energyDto.meterserialno && m.Readingdatetime > energyDto.Dateto)   // Strictly AFTER
                    .OrderBy(m => m.Readingdatetime)
                    .FirstOrDefaultAsync();

                // --- Determine Start and End Readings for Consumption ---
                Meterreading startReadingForConsumption;
                Meterreading endReadingForConsumption;

                if (!readingsInRange.Any())
                {
                    return new ResultsDto { message = "No readings found within the specified date range.", result = false };
                }

                startReadingForConsumption = startBound ?? readingsInRange.First();
                endReadingForConsumption = endBound ?? readingsInRange.Last();


                // --- Validate Start/End Readings ---
                if (startReadingForConsumption.Readingdatetime >= endReadingForConsumption.Readingdatetime)
                {
                    return new ResultsDto { message = "Cannot calculate: Valid start and end readings for consumption calculation not found or are identical.", result = false };
                }

                // --- Calculate Total Energy Consumed (kWh) ---
                decimal totalEnergyConsumedKwh = endReadingForConsumption.Energyconsumed - startReadingForConsumption.Energyconsumed;
                if (totalEnergyConsumedKwh < 0)
                {
                    return new ResultsDto { message = "Calculated negative consumption. Meter readings might be inconsistent or meter reset.", result = false };
                }

                // --- Calculate Duration (Hours) ---
                TimeSpan duration = endReadingForConsumption.Readingdatetime - startReadingForConsumption.Readingdatetime;
                double totalHours = duration.TotalHours;
                if (totalHours <= 0)
                {
                    return new ResultsDto { message = "Cannot calculate Power Factor: Duration between consumption readings is zero or negative.", result = false };
                }

                // --- Calculate Average Voltage & Current ---
                decimal averageVoltage = 0;
                decimal averageCurrent = 0;

                if (readingsInRange.Any()) 
                {
                    averageVoltage = readingsInRange.Average(x => x.Voltage);
                    averageCurrent = readingsInRange.Average(x => x.Current);
                }

                // --- Calculate Power Factor ---
                decimal? powerFactor = null;

                decimal apparentPowerKva = (averageVoltage * averageCurrent) / 1000;

                if (apparentPowerKva > 0)
                {
                    decimal realPowerKw = totalEnergyConsumedKwh / (decimal)totalHours;
                    powerFactor = realPowerKw / apparentPowerKva;
                    powerFactor = Math.Max(-1, Math.Min(1, powerFactor.Value));
                    powerFactor = Math.Round(powerFactor.Value, 2);
                }

                // --- Prepare Result ---
                var result = new 
                {
                    MeterSerialNo = energyDto.meterserialno,
                    ActualStartTime = startReadingForConsumption.Readingdatetime, // The timestamp used for start consumption
                    ActualEndTime = endReadingForConsumption.Readingdatetime,   // The timestamp used for end consumption
                    TotalEnergyConsumedKwh = totalEnergyConsumedKwh,
                    AveragePowerFactor = powerFactor
                };

                string resultJson = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

                return new ResultsDto { message = resultJson, result = true };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in GetEnergyConsumedAndPfFlexible: {ex.ToString()}");
                return new ResultsDto { message = $"An error occurred: {ex.Message}", result = false };
            }
        }
    }
}