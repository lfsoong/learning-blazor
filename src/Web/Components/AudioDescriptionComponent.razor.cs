﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Learning.Blazor.Extensions;
using Learning.Blazor.LocalStorage;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public sealed partial class AudioDescriptionComponent
    {
        private const string PreferencesStorageKey = "client-voice-preferences";

        private readonly IList<double> _voiceSpeeds =
            Enumerable.Range(0, 12).Select(i => (i + 1) * .25).ToList();

        private IList<SpeechSynthesisVoice> _voices = null!;
        private string _voice = "Auto";
        private double _voiceSpeed = 1;

        private ModalComponent _modal = null!;

        [Inject]
        public ILocalStorage LocalStorage { get; set; } = null!;

        [Inject]
        public IJSRuntime JavaScript { get; set; } = null!;

        [Inject]
        public ILogger<AudioDescriptionComponent> Logger { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var preferences =
                await LocalStorage.GetAsync<ClientVoicePreferences>(PreferencesStorageKey);
            if (preferences is not null)
            {
                (_voice, _voiceSpeed) = preferences;
            }

            await UpdateClientVoices(
                await JavaScript.GetClientVoices(this, nameof(UpdateClientVoices)));
        }

        [JSInvokable]
        public Task UpdateClientVoices(string voicesJson) =>
            InvokeAsync(() =>
            {
                var voices =
                    voicesJson.FromJson<List<SpeechSynthesisVoice>>();
                if (voices is { Count: > 0 })
                {
                    _voices = voices;

                    StateHasChanged();
                }
            });

        private void OnVoiceSpeedChange(ChangeEventArgs args) =>
            _voiceSpeed = double.TryParse(
                args?.Value?.ToString() ?? "1", out var speed) ? speed : 1;

        private async Task Show() => await _modal.Show();

        private async Task Confirm()
        {
            await LocalStorage.SetAsync(
                PreferencesStorageKey,
                new ClientVoicePreferences(_voice, _voiceSpeed));

            await _modal.Confirm();
        }

        private void OnDismissed(DismissalReason reason) =>
            Logger.LogWarning("User '{Reason}' the audio description modal.", reason);

        public record ClientVoicePreferences(
            [property: JsonPropertyName("voice")] string Voice,
            [property: JsonPropertyName("voiceSpeed")] double VoiceSpeed);
    }
}