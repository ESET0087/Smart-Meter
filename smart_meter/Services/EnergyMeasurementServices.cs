using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Model.DTOs;
//using smart_meter.Model.DTOs;

namespace smart_meter.Services
{
    public class EnergyMeasurementServices
    {
        public readonly AppDbContext _context;

        public EnergyMeasurementServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultsDto> EnergyConsumtion(EnergyConsumtionMeasurment energyDto)
        {
            try
            {
                var energyConsume = await _context.Meterreadings
                                           .Where(m => m.Meterserialno == energyDto.meterserialno && m.Readingdatetime >= energyDto.Datefrom && m.Readingdatetime <= energyDto.Dateto)
                                           .GroupBy(m => m.Meterserialno)
                                           .Select(m => new
                                           {
                                               MeterSerialNo = m.Key,
                                               TotalEnergyConsumed = m.Sum(x => x.Energyconsumed),
                                               AverageVoltage = m.Average(x => x.Voltage),
                                               AverageCurrent = m.Average(x => x.Current),
                                               count = m.Count(),
                                               AverageEnergy = m.Average(x => x.Energyconsumed)
                                           }
                                            )
                                           .FirstOrDefaultAsync();
                var apparentpower = (energyConsume.AverageCurrent * energyConsume.AverageVoltage) / 1000;
                var realpower = (energyConsume.TotalEnergyConsumed / (energyConsume.count * 30  / 60));

                var result = new
                {
                    TotalEnergyConsumed = energyConsume.TotalEnergyConsumed,
                    AverageVoltage = energyConsume.AverageVoltage,
                    AverageCurrent = energyConsume.AverageCurrent,
                    apparentpower = apparentpower,
                    realpower = realpower,
                    PowerFactor = realpower / apparentpower,
                    AverageEnergy = energyConsume.AverageEnergy
                };

                return new ResultsDto { message = result.ToString(), result = true };
            }
            catch (Exception ex)
            {
                return new ResultsDto { message = ex.ToString(), result = false };
            }

        }
    }
}
