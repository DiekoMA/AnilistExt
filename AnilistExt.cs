// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using AnilistExt.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Serilog;
using Serilog.Core;

namespace AnilistExt;

[Guid("b5c71e57-cb39-46b2-91f7-bc94634785f5")]
public sealed partial class AnilistExt : IExtension, IDisposable
{
    private readonly ManualResetEvent _extensionDisposedEvent;

    private readonly AnilistExtCommandsProvider _provider = new();
    public static SettingsManager AppSettings { get; private set; } = new();
    
    public AnilistExt(ManualResetEvent extensionDisposedEvent)
    {
        this._extensionDisposedEvent = extensionDisposedEvent;
        var savedToken = AppSettings.AccessToken;
        if (!string.IsNullOrEmpty(savedToken))
        {
            AnilistHelper.Instance.UpdateToken(savedToken).GetAwaiter().GetResult();
        }
    }

    public object? GetProvider(ProviderType providerType)
    {
        return providerType switch
        {
            ProviderType.Commands => _provider,
            _ => null,
        };
    }

    public void Dispose() => this._extensionDisposedEvent.Set();
}
