﻿using CIAT.DAPA.USAID.Forecast.Web.Models.Forecast;
using CIAT.DAPA.USAID.Forecast.Web.Models.Forecast.Entities;
using CIAT.DAPA.USAID.Forecast.Web.Models.Forecast.Repositories;
using CIAT.DAPA.USAID.Forecast.Web.Models.Forecast.Views;
using CIAT.DAPA.USAID.Forecast.Web.Models.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIAT.DAPA.USAID.Forecast.Web.Controllers
{
    public abstract class WebBaseController : Controller
    {
        protected IHostingEnvironment hostingEnvironment { get; set; }

        protected RepositoryWeatherStations rWS { get; set; }

        protected string Root { get; set; }

        protected IEnumerable<WeatherStationFull> WeatherStations { get; set; }
        protected List<WeatherStationFullCrop> WeatherStationsCrops { get; set; }
        //protected List<Country> Countries { get; set; }
        private string path { get; set; }

        /// <summary>
        /// Method Construct
        /// </summary>
        /// <param name="settings">Settings options</param>
        /// <param name="hostingEnvironment">Host Enviroment</param>
        public WebBaseController(IOptions<Settings> settings, IHostingEnvironment environment) : base()
        {
            hostingEnvironment = environment;
            Root = settings.Value.api_fs;
            path = hostingEnvironment.ContentRootPath + "\\Log\\";
            rWS = new RepositoryWeatherStations(Root, path);
            try
            {
                Task.Run(() => this.InitAsync()).Wait(350000);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Method that load the paths of the web apis
        /// </summary>
        protected void loadAPIs()
        {
            ViewBag.api_fs = Root;
        }        

        /// <summary>
        /// Method that stablish the months of the forecast
        /// </summary>
        protected void loadMonthsCrop()
        {
            string dates = string.Empty;
            string years = string.Empty;
            DateTime start = DateTime.Now.AddMonths(-1);
            for (int i = 1; i <= 2; i++)
            {
                start = start.AddMonths(1);
                dates += start.ToString("yyyy-MM") + ",";
            }
            ViewBag.gv_months = dates.Substring(0, dates.Length - 1);
        }

        /// <summary>
        /// Method which load all basic information
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> InitAsync()
        {
            try
            {
                // laoding data
                //Countries = await rWS.ListCountryAsync();
                WeatherStations = await rWS.ListAsync();
                WeatherStationsCrops = await rWS.ListByCropAsync();
                return true;
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllText(path + DateTime.Now.ToString("yyyyMMdd"), "2... " + ex.Message.ToString() + "\n" + ex.InnerException.ToString() + "\n" + ex.StackTrace.ToString() + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Method that transfer data to viewbag
        /// </summary>
        protected void SetWS(string countryId = null)
        {
            // Setting data
            if (countryId == null || countryId == "")
            {
                ViewBag.countryselect = countryId;
                //ViewBag.Countries = Countries;
                ViewBag.WeatherStations = WeatherStations;
                ViewBag.WeatherStationsCrops = WeatherStationsCrops;
            }
            else
            {
                ViewBag.countryselect = countryId;
                //ViewBag.Countries = Countries;
                WeatherStations = WeatherStations.Where(p => p.Country == countryId).ToList();
                WeatherStationsCrops = WeatherStationsCrops.Where(p => p.Country == countryId).ToList();
                ViewBag.WeatherStations = WeatherStations;
                ViewBag.WeatherStationsCrops = WeatherStationsCrops;
            }
        }

        protected WeatherStationFull SearchWS(string state, string municipality, string ws)
        {
            WeatherStationFull answer = WeatherStations.SingleOrDefault(p => p.State.Equals(state) 
                                                    && p.Municipality.Equals(municipality) 
                                                    && p.Name.Equals(ws));
            return answer;

        }

        /// <summary>
        /// Method that returns the first weather station provide for the API
        /// </summary>
        /// <returns></returns>
        public WeatherStationFull DefaultWeatherStation()
        {
            return WeatherStations.FirstOrDefault();
        }

        /// <summary>
        /// Method that returns the first weather station provide for the API
        /// </summary>
        /// <returns></returns>
        public WeatherStationFullCrop DefaultWeatherStationCrop()
        {
            return WeatherStationsCrops.FirstOrDefault();
        }

    }
}
