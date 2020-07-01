using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using NeonScripting;
using QuickBrowse.Properties;

namespace QuickBrowse
{
    public class QuickBrowseViewModel : INotifyPropertyChanged
    {
        public ListBox ListBox { get; set; }
        public RelayCommand CloseCommand { get; }
        public RelayCommand EnqueueAndPlayCommand { get; }
        public RelayCommand EnqueueNextCommand { get; }
        public RelayCommand EnqueueLastCommand { get; }

        private HashSet<NeonScriptTrack> _allTracks;
        private ObservableCollection<NeonScriptTrack> _tracks;

        public ObservableCollection<NeonScriptTrack> Tracks
        {
            get { return _tracks; }
            set { _tracks = value;OnPropertyChanged(); }
        }

        private IEnumerable<NeonScriptTrack> SelectedTracks
        {
            get
            {
                var res = new List<NeonScriptTrack>();
                foreach (var item in ListBox.SelectedItems)
                {
                    res.Add((NeonScriptTrack)item);
                }
                return res;
            }
        }

        public QuickBrowseViewModel()
        {
            CloseCommand = new RelayCommand(o =>
            {
                PluginHostHandler.Instance.PluginCloseAction?.Invoke(PluginHostHandler.Instance.ThisPlugin);
            }, o => { return true; });
            EnqueueAndPlayCommand = new RelayCommand(o =>
            {
                PluginHostHandler.Instance.ScriptHost.RemoteCalls.EnqueueAndPlay(SelectedTracks);
            }, o => { return ListBox.SelectedItems.Count > 0; });
            EnqueueNextCommand = new RelayCommand(o =>
            {
                PluginHostHandler.Instance.ScriptHost.RemoteCalls.EnqueueNext(SelectedTracks);
            }, o => { return ListBox.SelectedItems.Count > 0; });
            EnqueueLastCommand = new RelayCommand(o =>
            {
                PluginHostHandler.Instance.ScriptHost.RemoteCalls.EnqueueLast(SelectedTracks);
            }, o => { return ListBox.SelectedItems.Count > 0; });
        }

        private string _filterString;
        public string FilterString
        {
            get { return _filterString; }
            set { _filterString = value; OnPropertyChanged(); }
        }

        public void RefreshWithFilter()
        {
            if (_allTracks == null)
            {
                _allTracks = new HashSet<NeonScriptTrack>(PluginHostHandler.Instance.ScriptHost.Database.Tracks);
            }
            var filteredTracks = _allTracks.Where(x => x.Title.IndexOf(_filterString, StringComparison.CurrentCultureIgnoreCase) != -1);
            Tracks = new ObservableCollection<NeonScriptTrack>(filteredTracks);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
