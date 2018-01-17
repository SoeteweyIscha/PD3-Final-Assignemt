using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using views;
using impl;
using models;

public class SizeController
{

    public ISizeView View { private get; set; }

    public SizeModel Model { private get; set; }

    public void Init()
    {
        View.Size = Model.SizeValue;
        View.Axis = Model.Axis;

        View.OnChangeSize += ChangeValue;

        Model.OnSizeChanged += SizeChanged;
    }

    private void ChangeValue(object sender, ChangeSizeEventArgs eventArgs)
    {
        Model.SizeValue = Model.SizeValue + eventArgs.ChangeValue;
    }

    private void SizeChanged(object sender, SizeChangedEventArgs eventArgs)
    {
        View.Size = eventArgs.SizeValue;
    }
}
