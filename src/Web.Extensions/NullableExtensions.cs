﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Learning.Blazor.Extensions
{
    public static class NullableExtensions
    {
        public static void Deconstruct<T>(
            this Nullable<T> nullable,
            out bool hasValue,
            out T value) where T : struct =>
            (hasValue, value) = (nullable.HasValue, nullable.GetValueOrDefault());
    }
}
