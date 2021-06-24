using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Microsoft.Xaml.Behaviors;

namespace FritzboxDeskband
{
    public class ProgressBarAnimation : Behavior<ProgressBar>
    {
        private bool _IsAnimating = false;
        private double oldValue = double.NaN;
        private double newValue = double.NaN;

        protected override void OnAttached() => AssociatedObject.ValueChanged += ProgressBar_ValueChanged;

        protected override void OnDetaching() => AssociatedObject.ValueChanged -= ProgressBar_ValueChanged;

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Double> e)
        {
            if (this._IsAnimating)
            {
                this.newValue = e.NewValue;
                return;
            }
            this._IsAnimating = true;
            oldValue = e.OldValue;
            DoubleAnimation animation = new DoubleAnimation(e.OldValue, e.NewValue, new Duration(TimeSpan.FromMilliseconds(200)), FillBehavior.Stop);
            animation.Completed += Db_Completed;
            AssociatedObject.BeginAnimation(ProgressBar.ValueProperty, animation);
            e.Handled = true;
        }

        private void Db_Completed(object sender, EventArgs e)
        {
            this._IsAnimating = false;
        }
    }
}
