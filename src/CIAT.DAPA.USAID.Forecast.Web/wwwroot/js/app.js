﻿'use strict';

/**
 * @ngdoc overview
 * @name ForecastApp
 * @description
 * # ForecastApp
 *
 * Main module of the application.
 */
angular
  .module('ForecastApp', [])
  .value('config', {
      api_fs: $('#api_fs').val(),
      api_fs_geographic: $('#api_fs_geographic').val(),
      api_fs_agronomic: $('#api_fs_agronomic').val(),
      api_fs_forecast: $('#api_fs_forecast').val(),
      api_fs_historical: $('#api_fs_historical').val(),
      month_names: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
      days_names: ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'],
      climate_vars: [{ name: 'Precipitación', value: 'prec', metric:'mm2', description: 'Precipitación' },
                     { name: 'Temperatura máxima', value: 't_max', metric: 'mm2', description: 'Temperatura máxima' },
                     { name: 'Temperatura minima', value: 't_min', metric: 'mm2', description: 'Temperatura minima' },
                     { name: 'Radiación solar', value: 'sol_rad', metric: 'mm2', description: 'Radiación solar' }]
  })
  .factory('tools', function () {
      var _tools = {};
      _tools.search = function (name) {
          var url = window.location.href;
          name = name.replace(/[\[\]]/g, "\\$&");
          var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"), results = regex.exec(url);
          if (!results) return null;
          if (!results[2]) return '';
          return decodeURIComponent(results[2].replace(/\+/g, " "));
      }
      return _tools;
  });