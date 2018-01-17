using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using views;
using UnityEngine.UI;

namespace views
{
    public class ChangeSizeEventArgs : EventArgs
    {
        public int ChangeValue { get; private set; }

        public ChangeSizeEventArgs(int changeValue)
        {
            ChangeValue = changeValue;
        }
    }

    public interface ISizeView
    {
        event EventHandler<ChangeSizeEventArgs> OnChangeSize;
        int Size { set; }
        string Axis { set; }
    }
}

namespace impl
{
    public class SizeView : MonoBehaviour, ISizeView
    {
        [SerializeField]
        private Text _axis;

        [SerializeField]
        private Text _size;

        public event EventHandler<ChangeSizeEventArgs> OnChangeSize;

        //Fired by button
        public void FireChangeSize(int value)
        {
            if (OnChangeSize != null)
            {
                OnChangeSize(this, new ChangeSizeEventArgs(value));
            }
        }

        public string Axis
        {
            set{ _axis.text = value; }
        }

        public int Size
        {
            set { _size.text = value.ToString(); }
        }
    }
}
