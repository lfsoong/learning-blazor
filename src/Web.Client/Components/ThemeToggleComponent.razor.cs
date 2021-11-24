﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Extensions;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public partial class ThemeToggleComponent
    {
        private string _buttonClass => AppState.IsDarkTheme ? "light" : "dark";
        private string _iconClass => AppState.IsDarkTheme ? "moon" : "sun";

        protected override async Task OnInitializedAsync() =>
            AppState.IsDarkTheme =
                await JavaScript.GetCurrentDarkThemePreferenceAsync(
                    this, nameof(UpdateDarkThemePreference));

        [JSInvokable]
        public Task UpdateDarkThemePreference(bool isDarkTheme) =>
            InvokeAsync(() =>
            {
                AppState.IsDarkTheme = isDarkTheme;

                StateHasChanged();
            });
    }
}
