using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using NeonScripting;

namespace QuickBrowse
{
    /// <summary>
    /// Interaction logic for QuickBrowseWindow.xaml
    /// </summary>
    public partial class QuickBrowseWindow : Window
    {
        private const string QbLeftPos = "QUICKBROWSE_LEFT";
        private const string QbTopPos = "QUICKBROWSE_TOP";
        private const string QbWidth = "QUICKBROWSE_WIDTH";
        private const string QbHeight = "QUICKBROWSE_HEIGHT";

        private bool _alwaysOnTop = true;
        private readonly DispatcherTimer _mainFilterTimer;
        private QuickBrowseViewModel _vm = new QuickBrowseViewModel();
        public QuickBrowseWindow()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            _vm.ListBox = ResultsLb;
            DataContext = _vm;

            _mainFilterTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(750) };
            _mainFilterTimer.Tick += OnTimerTick;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _mainFilterTimer.Stop();
            if (FilterTextBox.Text != _vm.FilterString)
            {
                _vm.FilterString = FilterTextBox.Text;
                RefreshWithFilter();
            }
        }

        private void RefreshWithFilter()
        {
            _vm.RefreshWithFilter();
            ResultsLb.ItemsSource = _vm.Tracks;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (textBox.Text.Length >= 3)
            {
                _mainFilterTimer.Stop();
                _mainFilterTimer.Start();
            }
            else
            {
                if (_vm.FilterString != string.Empty)
                {
                    _vm.FilterString = string.Empty;
                    RefreshWithFilter();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _vm.CloseCommand.Execute(null);
            Close();
        }

        private void ResultsLb_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ResultsLb.SelectedItem != null)
            {
                var trk = (NeonScriptTrack) ResultsLb.SelectedItem;
                PluginHostHandler.Instance.ScriptHost.RemoteCalls.EnqueueAndPlay(new List<NeonScriptTrack>{ trk });
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var mi = (MenuItem) sender;
            _alwaysOnTop = !_alwaysOnTop;
            Topmost = _alwaysOnTop;
            mi.IsChecked = _alwaysOnTop;
        }

        private void QuickBrowseWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var remoteCalls = PluginHostHandler.Instance.ScriptHost.RemoteCalls;
            var leftPos = remoteCalls.GetIntValue(QbLeftPos);
            if (leftPos == -1)
            {
                return;
            }
            var topPos = remoteCalls.GetIntValue(QbTopPos);
            var width = remoteCalls.GetIntValue(QbWidth);
            var height = remoteCalls.GetIntValue(QbHeight);
            Left = leftPos;
            Top = topPos;
            Width = width;
            Height = height;
        }

        private void QuickBrowseWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var remoteCalls = PluginHostHandler.Instance.ScriptHost.RemoteCalls;
            remoteCalls.SetIntValue(QbLeftPos, (int)Left);
            remoteCalls.SetIntValue(QbTopPos, (int)Top);
            remoteCalls.SetIntValue(QbWidth, (int)Width);
            remoteCalls.SetIntValue(QbHeight, (int)Height);
        }
    }
}
