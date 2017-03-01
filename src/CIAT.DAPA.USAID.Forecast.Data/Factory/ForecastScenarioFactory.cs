﻿using CIAT.DAPA.USAID.Forecast.Data.Enums;
using CIAT.DAPA.USAID.Forecast.Data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIAT.DAPA.USAID.Forecast.Data.Factory
{
    /// <summary>
    /// This class allow to get information about scenario forecast collection
    /// </summary>
    public class ForecastScenarioFactory: FactoryDB<ForecastScenario>
    {
        /// <summary>
        /// Method Construct
        /// </summary>
        /// <param name="database">Database connected to mongo</param>
        public ForecastScenarioFactory(IMongoDatabase database): base(database, LogEntity.fs_forecast_scenario)
        {

        }

        public async override Task<bool> updateAsync(ForecastScenario entity, ForecastScenario newEntity)
        {
            var result = await collection.ReplaceOneAsync(Builders<ForecastScenario>.Filter.Eq("_id", entity.id), newEntity);
            return result.ModifiedCount > 0;
        }

        public async override Task<bool> deleteAsync(ForecastScenario entity)
        {
            throw new NotImplementedException();
        }

        public async override Task<ForecastScenario> insertAsync(ForecastScenario entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }
    }
}
