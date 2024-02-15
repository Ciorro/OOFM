﻿using Microsoft.Xaml.Behaviors;
using System.Windows.Input;

namespace OOFM.Ui.Events;
internal class BackButtonEventTrigger : EventTrigger
{
    protected override void OnEvent(EventArgs eventArgs)
    {
        if (eventArgs is not MouseButtonEventArgs mouseEventArgs)
            return;

        if (mouseEventArgs.ChangedButton == MouseButton.XButton1)
            InvokeActions(eventArgs);
    }
}