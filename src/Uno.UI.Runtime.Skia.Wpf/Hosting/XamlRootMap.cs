﻿#nullable enable

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Uno.UI.Runtime.Skia.Wpf.Hosting
{
	internal static class XamlRootMap
	{
		private static readonly Dictionary<XamlRoot, IWpfHost> _map = new();

		internal static void Register(XamlRoot xamlRoot, IWpfHost host)
		{
			if (xamlRoot is null)
			{
				throw new ArgumentNullException(nameof(xamlRoot));
			}

			if (host is null)
			{
				throw new ArgumentNullException(nameof(host));
			}

			_map[xamlRoot] = host;
		}

		internal static void Unregister(XamlRoot xamlRoot)
		{
			if (xamlRoot is null)
			{
				throw new ArgumentNullException(nameof(xamlRoot));
			}

			_map.Remove(xamlRoot);
		}

		internal static IWpfHost? GetHostForRoot(XamlRoot xamlRoot) =>
			_map.TryGetValue(xamlRoot, out var host) ? host : null;
	}
}
