using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace YarTransportAndroidGUI
{
    class CustomTextView
    {
        public AutoCompleteTextView textView;
        public string Text { get { return textView.Text; } set { textView.Text = value; } }

        public CustomTextView(AutoCompleteTextView textView, ArrayAdapter adapter)
        {
            this.textView = textView;
            this.textView.Adapter = adapter;
            this.textView.TextChanged += StartSpinner_TextChanged;
            this.textView.FocusChange += _FocusChange;
        }

        private void StartSpinner_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (textView.Text.Contains("\n"))
                textView.Text = textView.Text.Replace("\n", "");
        }

        private void _FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (textView.HasFocus)
                textView.Text = "";
        }
    }
}