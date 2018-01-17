using System;

namespace models
{
    public class SizeChangedEventArgs : EventArgs
    {
        public int SizeValue { get; private set; }
  
        public SizeChangedEventArgs(int intValue)
        {
            SizeValue = intValue;
        }
    }

    public class SizeModel
    {
        //Create variable
        private int _size;
        public string Axis;

        public event EventHandler<SizeChangedEventArgs> OnSizeChanged;

        public int SizeValue
        {
            get { return _size; } 
            set
            {
                _size = value;

                FireOnSizeChanged(_size);
            }
        }

        private void FireOnSizeChanged(int v)
        {
            if (OnSizeChanged != null)
            {
                OnSizeChanged(this, new SizeChangedEventArgs(v));
            }
        }
    }
}
