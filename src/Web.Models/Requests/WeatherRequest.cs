﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class WeatherRequest
{
    [JsonPropertyName("lang")] public string Language { get; set; } = null!;
    [JsonPropertyName("lat")] public decimal Latitude { get; set; }
    [JsonPropertyName("lon")] public decimal Longitude { get; set; }
    [JsonPropertyName("units")] public int Units { get; set; }
    [JsonIgnore] public string Key => $"WR:{Language}:{Latitude}:{Longitude}:{Units}";

    /// <summary>
    /// Returns the Azure Function URL for weather, after appling format value replacement.
    /// </summary>
    public string ToFormattedUrl(string weatherFunctionUrlFormat) =>
        // Example: ".../api/currentweather/{lang}/{latitude}/{longitude}/{units}"
        weatherFunctionUrlFormat
            .Replace("{lang}", Language)
            .Replace("{latitude}", Latitude.ToString())
            .Replace("{longitude}", Longitude.ToString())
            .Replace("{units}", ((MeasurementSystem)Units).ToString());
}
