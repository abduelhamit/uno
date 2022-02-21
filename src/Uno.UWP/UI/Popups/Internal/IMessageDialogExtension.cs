﻿using System.Threading.Tasks;

namespace Windows.UI.Popups.Internal;

internal interface IMessageDialogExtension
{
	Task<IUICommand> ShowAsync();
}
