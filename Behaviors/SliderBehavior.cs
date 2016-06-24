using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WpfMvvmSample.Behaviors
{
    public class SliderBehavior : Behavior<MediaElement>
    {
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(TimeSpan), typeof(SliderBehavior),
                                        new PropertyMetadata((d, e) =>
                                                             ((SliderBehavior)d).PropertyChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue)));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(SliderBehavior), new PropertyMetadata(default(double)));

       
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(PositionProperty); }
            set
            {
                SetValue(PositionProperty, value);
            }
        }

        private readonly Timer _timer = new Timer(1000);

        public void PropertyChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            AssociatedObject.Position = newValue;
            
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            _timer.AutoReset = true;
            _timer.Elapsed += _timer_Elapsed;
            Position = AssociatedObject.Position;
            AssociatedObject.MediaOpened += MediaOpened;
            _timer.Start();
        }

        private void MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                Maximum = AssociatedObject.NaturalDuration.TimeSpan.TotalSeconds;
            }
            catch (Exception)
            {
                Maximum = 5.0;
            }
        }

        private void DispatcherTimerResync()
        {
            Position = AssociatedObject.Position;
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(DispatcherTimerResync));
        }

        protected override void OnDetaching()
        {
            _timer.Stop();
            _timer.Elapsed -= _timer_Elapsed;
            base.OnDetaching();
        }
    }
}
