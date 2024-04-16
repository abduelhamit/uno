﻿#nullable enable

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.UI.Xaml.Controls.Extensions;

internal interface IOverlayTextBoxView
{
	event TextControlPasteEventHandler? Paste;

	bool IsDisplayed { get; }

	string Text { get; set; }

	(int start, int length) Selection { get; set; }

	/// <summary>
	/// On some platforms (namely Skia.WPF) KeyDown is fired after Selection is already set to the new value.
	/// This property is provided to allow access to the selection value right before KeyDown.
	/// </summary>
	(int start, int length) SelectionBeforeKeyDown { get; }

	/// <summary>
	/// Returns a value indicating whether this TextBoxView is compatible with the given TextBox state.
	/// </summary>
	/// <param name="textBox">TextBox.</param>
	/// <returns>True if compatible.</returns>
	bool IsCompatible(TextBox textBox);

	void SetFocus();

	void SetPasswordRevealState(PasswordRevealState passwordRevealState);

	void AddToTextInputLayer(XamlRoot xamlRoot);

	void RemoveFromTextInputLayer();

	void SetPosition(double x, double y);

	void SetSize(double width, double height);

	void UpdateProperties(TextBox textBox);

	IDisposable ObserveTextChanges(EventHandler onChanged);
}
