﻿// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Threading.Tasks;
using Learning.Blazor.Models;

namespace Learning.Blazor.Functions.Services
{
    public interface IWeatherService
    {
        Task<CurrentWeather?> GetCurrentWeatherAsync(
            Coordinates coordinates, string units);

        Task<ForecastWeather?> GetForecastWeatherAsync(
            Coordinates coordinates, string units);
    }
}