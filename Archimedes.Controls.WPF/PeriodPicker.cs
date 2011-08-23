using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using Archimedes.Patterns.Types;
using System.Collections;

namespace Archimedes.Controls.WPF
{
    
    /// <summary>
    /// 
    /// </summary>
    public class PeriodPicker : Control
    {
        #region Fields

        ListBox _timeListBox;
        private DateTimeFormatInfo DateTimeFormatInfo { get; set; }
        internal static readonly Period EndTimeDefaultValue = Period.FromYears(1);
        internal static readonly Period StartTimeDefaultValue = Period.Zero;
        internal static readonly Period TimeIntervalDefaultValue = Period.FromMonths(1);

        #endregion //Members

        #region Properties

        #region AllowSpin

        public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register("AllowSpin", typeof(bool), typeof(PeriodPicker), new UIPropertyMetadata(true));
        public bool AllowSpin {
            get { return (bool)GetValue(AllowSpinProperty); }
            set { SetValue(AllowSpinProperty, value); }
        }

        #endregion //AllowSpin

        #region EndTime

        public static readonly DependencyProperty EndTimeProperty = DependencyProperty.Register(
            "EndTime", typeof(Period), typeof(PeriodPicker),
            new UIPropertyMetadata(EndTimeDefaultValue, new PropertyChangedCallback(OnEndTimeChanged),
                new CoerceValueCallback(OnCoerceEndTime)));

        private static object OnCoerceEndTime(DependencyObject o, object value) {
            PeriodPicker timePicker = o as PeriodPicker;
            if (timePicker != null)
                return timePicker.OnCoerceEndTime((Period)value);
            else
                return value;
        }

        private static void OnEndTimeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            PeriodPicker timePicker = o as PeriodPicker;
            if (timePicker != null)
                timePicker.OnEndTimeChanged((Period)e.OldValue, (Period)e.NewValue);
        }

        protected Period OnCoerceEndTime(Period value) {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected void OnEndTimeChanged(Period oldValue, Period newValue) {
            // TODO: Add your property changed side-effects. Descendants can override as well.
            UpdateItemsSource();
        }

        public Period EndTime {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get {
                return (Period)GetValue(EndTimeProperty);
            }
            set {
                SetValue(EndTimeProperty, value);
            }
        }


        #endregion //EndTime

        #region Format

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(PeriodFormat), typeof(PeriodPicker), new UIPropertyMetadata(PeriodFormat.Months, OnFormatChanged));
        public PeriodFormat Format {
            get { return (PeriodFormat)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        private static void OnFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            PeriodPicker timePicker = o as PeriodPicker;
            if (timePicker != null)
                timePicker.OnFormatChanged((PeriodFormat)e.OldValue, (PeriodFormat)e.NewValue);
        }

        protected void OnFormatChanged(PeriodFormat oldValue, PeriodFormat newValue) {

        }

        #endregion //Format

        #region FormatString

        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register("FormatString", typeof(string), typeof(PeriodPicker), new UIPropertyMetadata(default(String), OnFormatStringChanged));
        public string FormatString {
            get { return (string)GetValue(FormatStringProperty); }
            set { SetValue(FormatStringProperty, value); }
        }

        private static void OnFormatStringChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            PeriodPicker timePicker = o as PeriodPicker;
            if (timePicker != null)
                timePicker.OnFormatStringChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual void OnFormatStringChanged(string oldValue, string newValue) {
            if (string.IsNullOrEmpty(newValue))
                throw new ArgumentException("CustomFormat should be specified.", FormatString);
        }

        #endregion //FormatString

        #region IsOpen

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(PeriodPicker), new UIPropertyMetadata(false));
        public bool IsOpen {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        #endregion //IsOpen

        #region Maximum



        #endregion //Maximum

        #region Minimum



        #endregion //Minimum

        #region ShowButtonSpinner

        public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register("ShowButtonSpinner", typeof(bool), typeof(PeriodPicker), new UIPropertyMetadata(true));
        public bool ShowButtonSpinner {
            get { return (bool)GetValue(ShowButtonSpinnerProperty); }
            set { SetValue(ShowButtonSpinnerProperty, value); }
        }

        #endregion //ShowButtonSpinner

        #region StartTime

        public static readonly DependencyProperty StartTimeProperty = DependencyProperty.Register("StartTime", typeof(Period), typeof(PeriodPicker), new UIPropertyMetadata(StartTimeDefaultValue, new PropertyChangedCallback(OnStartTimeChanged), new CoerceValueCallback(OnCoerceStartTime)));

        private static object OnCoerceStartTime(DependencyObject o, object value) {
            PeriodPicker timePicker = o as PeriodPicker;
            if (timePicker != null)
                return timePicker.OnCoerceStartTime((Period)value);
            else
                return value;
        }

        private static void OnStartTimeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            PeriodPicker timePicker = o as PeriodPicker;
            if (timePicker != null)
                timePicker.OnStartTimeChanged((Period)e.OldValue, (Period)e.NewValue);
        }

        protected Period OnCoerceStartTime(Period value) {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected new void OnStartTimeChanged(Period oldValue, Period newValue) {
            // TODO: Add your property changed side-effects. Descendants can override as well.
            UpdateItemsSource();
        }

        public new Period StartTime {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get {
                return (Period)GetValue(StartTimeProperty);
            }
            set {
                SetValue(StartTimeProperty, value);
            }
        }


        #endregion //StartTime

        #region TimeInterval

        public static readonly DependencyProperty TimeIntervalProperty = DependencyProperty.Register("TimeInterval", typeof(Period), typeof(PeriodPicker), new UIPropertyMetadata(TimeIntervalDefaultValue, OnTimeIntervalChanged));
        public Period TimeInterval {
            get { return (Period)GetValue(TimeIntervalProperty); }
            set { SetValue(TimeIntervalProperty, value); }
        }

        private static void OnTimeIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            PeriodPicker timePicker = o as PeriodPicker;
            if (timePicker != null)
                timePicker.OnTimeIntervalChanged((Period)e.OldValue, (Period)e.NewValue);
        }

        protected virtual void OnTimeIntervalChanged(Period oldValue, Period newValue) {
            // TODO: Add your property changed side-effects. Descendants can override as well.
            UpdateItemsSource();
        }

        #endregion //TimeInterval

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Period?), typeof(PeriodPicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        public Period? Value {
            get { return (Period?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            PeriodPicker timePicker = o as PeriodPicker;
            if (timePicker != null)
                timePicker.OnValueChanged((Period?)e.OldValue, (Period?)e.NewValue);
        }

        protected virtual void OnValueChanged(Period? oldValue, Period? newValue) {
            //TODO: refactor this
            if (newValue.HasValue && _timeListBox != null) {
                var items = _timeListBox.ItemsSource;
                foreach (PeriodItem item in items) {
                    if (item.Time == newValue.Value) {
                        int index = _timeListBox.Items.IndexOf(item);
                        if (_timeListBox.SelectedIndex != index)
                            _timeListBox.SelectedIndex = index;
                        break;
                    }
                }
            }

            RoutedPropertyChangedEventArgs<object> args = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue);
            args.RoutedEvent = ValueChangedEvent;
            RaiseEvent(args);
        }

        #endregion //Value

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(PeriodPicker), new UIPropertyMetadata(null));
        public object Watermark {
            get { return (object)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        #endregion //Watermark

        #region WatermarkTemplate

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(PeriodPicker), new UIPropertyMetadata(null));
        public DataTemplate WatermarkTemplate {
            get { return (DataTemplate)GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }

        #endregion //WatermarkTemplate

        #endregion //Properties

        #region Constructors

        static PeriodPicker() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PeriodPicker), 
                new FrameworkPropertyMetadata(typeof(PeriodPicker)));
        }


        public PeriodPicker() {
            DateTimeFormatInfo = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture);
            Keyboard.AddKeyDownHandler(this, OnKeyDown);
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideCapturedElement);
        }

        #endregion //Constructors

        #region Base Class Overrides

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();

            _timeListBox = (ListBox)GetTemplateChild("PART_TimeListItems");
            _timeListBox.ItemsSource = GenerateTimeListItemsSource();
            _timeListBox.SelectionChanged += TimeListBox_SelectionChanged;
        }

        #endregion //Base Class Overrides

        #region Event Handlers

        private void OnKeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Escape:
                case Key.Tab: {
                    CloseTimePicker();
                    break;
                }
            }
        }

        private void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e) {
            CloseTimePicker();
        }

        void TimeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count > 0) {
                PeriodItem selectedTimeListItem = (PeriodItem)e.AddedItems[0];
                Value = selectedTimeListItem.Time; ;
            }
            CloseTimePicker();
        }

        #endregion //Event Handlers

        #region Events

        //Due to a bug in Visual Studio, you cannot create event handlers for nullable args in XAML, so I have to use object instead.
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(PeriodPicker));
        public event RoutedPropertyChangedEventHandler<object> ValueChanged {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        #endregion //Events

        #region Methods

        private void CloseTimePicker() {
            if (IsOpen)
                IsOpen = false;
            ReleaseMouseCapture();
        }

        protected void UpdateItemsSource() {
            if (_timeListBox != null) {
                _timeListBox.ItemsSource = GenerateTimeListItemsSource();
            }
        }


        public IEnumerable GenerateTimeListItemsSource() {
            Period time = StartTime;
            Period endTime = EndTime;

            if (endTime <= time) {
                endTime = EndTimeDefaultValue;
                time = StartTimeDefaultValue;
            }

            Period timeInterval = TimeInterval;

            if (time != null && endTime != null && timeInterval != null && !timeInterval.IsEmpty) {
                while (time <= endTime) {
                    yield return new PeriodItem(Format, time);
                    time = time + timeInterval;
                }
                yield break;
            }
        }

        //private string GetTimeFormat() {
        //    switch (Format) {
        //        case PeriodFormat.Custom:
        //        return FormatString;
        //        case PeriodFormat.LongTime:
        //        return DateTimeFormatInfo.LongTimePattern;
        //        case PeriodFormat.ShortTime:
        //        return DateTimeFormatInfo.ShortTimePattern;
        //        default:
        //        return DateTimeFormatInfo.ShortTimePattern;
        //    }
        //}

        #endregion //Methods
    }
}
