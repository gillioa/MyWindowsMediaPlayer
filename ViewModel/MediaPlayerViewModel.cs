using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq.Expressions;
using WpfMvvmSample.Model;
using WpfMvvmSample.FrameworkMvvm;
using System.Windows.Input;
using WpfMvvmSample.Model.Classes;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace WpfMvvmSample.ViewModel
{
    public class MediaPlayerViewModel : ObservableObject
    {


        #region MediaRessource

        private MediaDirectory _mediaDirectory;
        private MediaDirectory _mediaDirectoryLib = new MediaDirectory(@"C:\Users\Public\libwmp.xml");
        private MediaElement _mediaElementObject;
        private Boolean _isInMuted = false;
        private Boolean _StatePlay = false;
        private Boolean _isShuffled = false;
        private Int32 _currentIndex = 0;
        private static int lastIdx = -1;

        ObservableCollection<Media> _lMedia = new ObservableCollection<Media>();
        ObservableCollection<Media> _lMediaLib = new ObservableCollection<Media>();

        public MediaDirectory getLib()
        {
            return _mediaDirectoryLib;
        }

        public void setLib(List<Media> lm)
        {
            _lMediaLib.Clear();
            foreach (Media m in lm)
                _lMediaLib.Add(m);
        }

        public MediaDirectory getPlaylist()
        {
            return _mediaDirectory;
        }

        public void setPlaylist(List<Media> lm)
        {
            _lMedia.Clear();
            foreach (Media m in lm)
                _lMedia.Add(m);
        }

        public ObservableCollection<Media> MediaBindind
        {
            get { return _lMedia; }
        }
        public ObservableCollection<Media> MediaBindindLib
        {
            get { return _lMediaLib; }
        }

        public Boolean IsShuffled
        {
            get { return _isShuffled; }
            set
            {
                _isShuffled = value;
                OnPropertyChanged("IsShuffled");
            }
        }

        public Boolean IsInMuted
        {
            get { return _isInMuted; }
            set
            {
                _isInMuted = value;
                OnPropertyChanged("IsInMuted");
            }
        }

        public Boolean StatePlay
        {
            get { return _StatePlay; }
            set
            {
                _StatePlay = value;
                OnPropertyChanged("StatePlay");
            }
        }

        private Uri _DataSource;
        public Uri DataSourceProperty
        {
            get { return _DataSource; }
            set
            {
                _DataSource = value;
                OnPropertyChanged("DataSourceProperty");
            }
        }

        #endregion

        #region CommandeBinding
        /// <summary>
        /// Définition des commandes 
        /// </summary>
        private RelayCommand _ActionStopCommand;
        private RelayCommand _ActionPlayPause;
        private RelayCommand _OpenPlaylistCommand;
        private RelayCommand _ActionNextCommand;
        private RelayCommand _ActionPrevCommand;
        private RelayCommand _OrderByTitleAction;
        private RelayCommand _OrderByAutorAction;
        private RelayCommand _OrderByAlbumAction;
        private RelayCommand _OrderByTypeAction;
        private RelayCommand _OrderByGenreAction;
        private RelayCommand _OrderByYearAction;
        private RelayCommand _ReverseCommand;
        private RelayCommand _ActionAddCommand;
        private RelayCommand _ActionDelCommand;
        private RelayCommand _SaveCommand;
        private RelayCommand _UpCommand;
        private RelayCommand _DownCommand;
        private RelayCommand _MuteCommand;
        private RelayCommand _AddAudioToPlaylistCommand;
        private RelayCommand _PlaylistSelectedCommand;
        private RelayCommand _NextMediaCommand;
        private RelayCommand _DelListFromLib;
        private RelayCommand _ShuffleCommand;

        public ICommand ShuffleCommand
        {
            get
            {
                if (_ShuffleCommand == null)
                    _ShuffleCommand = new RelayCommand(p => ShuffleMedia(), p => true);
                return _ShuffleCommand;
            }
        }

        public ICommand ActionDelCommand
        {
            get
            {
                if (_ActionDelCommand == null)
                    _ActionDelCommand = new RelayCommand(p => ActionDel(p), p => true);
                return _ActionDelCommand;
            }
        }

        public ICommand NextMediaCommand
        {
            get
            {
                if (_NextMediaCommand == null)
                    _NextMediaCommand = new RelayCommand(p => NextMedia(), p => true);
                return _NextMediaCommand;
            }
        }

        public ICommand PlaylistSelectedCommand
        {
            get
            {
                if (_PlaylistSelectedCommand == null)
                    _PlaylistSelectedCommand = new RelayCommand(p => PlaylistSelected(p), p => true);
                return _PlaylistSelectedCommand;
            }
        }

        public ICommand DelListFromLibCommand
        {
            get
            {
                if (_DelListFromLib == null)
                    _DelListFromLib = new RelayCommand(p => DelListFromLib(p), p => true);
                return _DelListFromLib;
            }
        }

        public ICommand AddLibToPlaylistCommand
        {
            get
            {
                if (_AddAudioToPlaylistCommand == null)
                    _AddAudioToPlaylistCommand = new RelayCommand(p => AddLibToPlayList(p), p => true);
                return _AddAudioToPlaylistCommand;
            }
        }


        public ICommand ActionMuteCommand
        {
            get
            {
                if (_MuteCommand == null)
                    _MuteCommand = new RelayCommand(p => Mute(p), p => true);
                return _MuteCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                    _SaveCommand = new RelayCommand(p => Save(), p => true);
                return _SaveCommand;
            }
        }

        public ICommand UpCommand
        {
            get
            {
                if (_UpCommand == null)
                    _UpCommand = new RelayCommand(p => UpOnPlaylist(p), p => true);
                return _UpCommand;
            }
        }

        public ICommand DownCommand
        {
            get
            {
                if (_DownCommand == null)
                    _DownCommand = new RelayCommand(p => DownOnPlaylist(p), p => true);
                return _DownCommand;
            }
        }

        public ICommand ActionAddCommand
        {
            get
            {
                if (_ActionAddCommand == null)
                    _ActionAddCommand = new RelayCommand(p => AddList(), p => true);
                return _ActionAddCommand;
            }
        }

        public ICommand ReverseCommand
        {
            get
            {
                if (_ReverseCommand == null)
                    _ReverseCommand = new RelayCommand(p => Reverse(), p => true);
                return _ReverseCommand;
            }
        }
        //        private RelayCommand _OrderByArtisteAction;
        public ICommand OrderByTitleAction
        {
            get
            {
                if (_OrderByTitleAction == null)
                    _OrderByTitleAction = new RelayCommand(p => OrderBy("title"), p => true);
                return _OrderByTitleAction;
            }
        }

        public ICommand OrderByAlbumAction
        {
            get
            {
                if (_OrderByAlbumAction == null)
                    _OrderByAlbumAction = new RelayCommand(p => OrderBy("album"), p => true);
                return _OrderByAlbumAction;
            }
        }

        public ICommand OrderByAutorAction
        {
            get
            {
                if (_OrderByAutorAction == null)
                    _OrderByAutorAction = new RelayCommand(p => OrderBy("autor"), p => true);
                return _OrderByAutorAction;
            }
        }

        public ICommand OrderByTypeAction
        {
            get
            {
                if (_OrderByTypeAction == null)
                    _OrderByTypeAction = new RelayCommand(p => OrderBy("type"), p => true);
                return _OrderByTypeAction;
            }
        }

        public ICommand OrderByGenreAction
        {
            get
            {
                if (_OrderByGenreAction == null)
                    _OrderByGenreAction = new RelayCommand(p => OrderBy("genre"), p => true);
                return _OrderByGenreAction;
            }
        }

        public ICommand OrderByYearAction
        {
            get
            {
                if (_OrderByYearAction == null)
                    _OrderByYearAction = new RelayCommand(p => OrderBy("year"), p => true);
                return _OrderByYearAction;
            }
        }

        public ICommand ActionNextCommand
        {
            get
            {
                if (_ActionNextCommand == null)
                    _ActionNextCommand = new RelayCommand(p => ActionNext(p), p => true);
                return _ActionNextCommand;
            }
        }

        public ICommand ActionPrevCommand
        {
            get
            {
                if (_ActionPrevCommand == null)
                    _ActionPrevCommand = new RelayCommand(p => ActionPrev(p), p => true);
                return _ActionPrevCommand;
            }
        }

        public ICommand OpenPlaylistCommand
        {
            get
            {
                if (_OpenPlaylistCommand == null)
                    _OpenPlaylistCommand = new RelayCommand(p => OpenPlaylist(), p => true);
                return _OpenPlaylistCommand;
            }
        }

        public ICommand ActionStopCommand
        {
            get
            {
                if (_ActionStopCommand == null)
                    _ActionStopCommand = new RelayCommand(p => Stop(p), p => true);
                return _ActionStopCommand;
            }
        }

        public ICommand ActionPlayPauseCommand
        {
            get
            {
                if (_ActionPlayPause == null)
                    _ActionPlayPause = new RelayCommand(p => PlayPause(p), p => true);
                return _ActionPlayPause;
            }
        }

        #endregion

        private void ShuffleMedia()
        {
            IsShuffled = (!_isShuffled);
        }

        private void ActionDel(object p)
        {
            System.Windows.Controls.ListView lv = (System.Windows.Controls.ListView)p;
            if (lv.SelectedIndex >= 0)
            {
                _mediaDirectory.mediaList.RemoveAt(lv.SelectedIndex);
                _lMedia.Clear();
                foreach (Media m in _mediaDirectory.mediaList)
                    _lMedia.Add(m);
            }
        }

        private void DelListFromLib(object p)
        {
            System.Windows.Controls.ListView lv = (System.Windows.Controls.ListView)p;
            if (lv.SelectedIndex >= 0)
            {
                _mediaDirectoryLib.mediaList.RemoveAt(lv.SelectedIndex);
                _lMediaLib.Clear();
                foreach (Media m in _mediaDirectoryLib.mediaList)
                    _lMediaLib.Add(m);
            }
            Model.Classes.Model.serialize(_mediaDirectoryLib, @"C:\Users\Public\libwmp.xml");
        }

        private void AddLibToPlayList(object p)
        {
            System.Windows.Controls.ListView lv = (System.Windows.Controls.ListView)p;
            if (lv.SelectedIndex >= 0)
            {
                _mediaDirectory.mediaList.Add(_mediaDirectoryLib.mediaList[lv.SelectedIndex]);
                _lMedia.Clear();
                foreach (Media m in _mediaDirectory.mediaList)
                    _lMedia.Add(m);
            }
        }

        private void Save()
        {
            SaveFileDialog openFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "XML Files (.xml)|*.xml";
            openFileDialog.FilterIndex = 1;
            openFileDialog.FileName = "playlist.xml";
            bool? userClickedOK = openFileDialog.ShowDialog();

            if (userClickedOK == true)
            {
                Model.Classes.Model.serialize(_mediaDirectory, @openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        public MediaPlayerViewModel()
        {
            _DataSource = null;
            _mediaDirectory = new MediaDirectory();
            StatePlay = false;
            List<Media> tmp = new List<Media>();
            tmp = _mediaDirectoryLib.mediaList.GroupBy(i => i.title).Select(g => g.First()).ToList();
            foreach (Media m in tmp)
                _lMediaLib.Add(m);

        }

        private void AddList()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Media File|*.mp3;*.flac;*.wma;*.wav;*.m3u;*.aac;*.wmx;*.wm;*.avi;*.mkv;*.wmv;*.wm;*.m2ts;*.m2t;*.mov;*.qt;*.divx;*.wtv;*.mp4;*.m4v;*.mpeg;*.mpg;*.mpe;*.m1v;*.mp2;*.mpv2;*.mod;*.vob;*.m1v;*.jpg;*.png*;*.bmp;*.gif;*.jpeg;*.tiff;*.tif;*.hpd;*.wdp;*.raw";
            openFileDialog.Filter += "|Image File (.jpg, .png, .bmp, .gif, .jpeg, .tiff, .tif, .hdp, .wdp, .raw)|*.jpg;*.png*;*.bmp;*.gif;*.jpeg;*.tiff;*.tif;*.hpd;*.wdp;*.raw";
            openFileDialog.Filter += "|Video File (.avi, .mkv, .wmv, .wm, .m2ts, .m2t, .mov, .qt, .divx, .wtv, .mp4, .m4v, .mpeg, .mpg, .mpe, .m1v, .mp2, .mpv2, .mod, .vob, .m1v)|*.avi;*.mkv;*.wmv;*.wm;*.m2ts;*.m2t;*.mov;*.qt;*.divx;*.wtv;*.mp4;*.m4v;*.mpeg;*.mpg;*.mpe;*.m1v;*.mp2;*.mpv2;*.mod;*.vob;*.m1v";
            openFileDialog.Filter += "|Music File (.mp3, .flac, .wma, .wav, .m3u, .aac, .wmx, .wm)|*.mp3;*.flac;*.wma;*.wav;*.m3u;*.aac;*.wmx;*.wm";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;
            bool? userClickedOK = openFileDialog.ShowDialog();

            if (userClickedOK == true)
            {
                Media media = new Media();
                media.path = openFileDialog.FileName;
                Model.Classes.Model.getFileMetaData(media);
                _lMedia.Add(media);
                _mediaDirectory.mediaList.Add(media);

                _lMediaLib.Add(media);
                _mediaDirectoryLib.mediaList.Clear();
                foreach (Media m in _lMediaLib)
                    _mediaDirectoryLib.mediaList.Add(m);
                _mediaDirectoryLib.mediaList = _mediaDirectoryLib.mediaList.GroupBy(i => i.title).Select(g => g.First()).ToList();
                _mediaDirectoryLib.orderBy("type", 1);
                Model.Classes.Model.serialize(_mediaDirectoryLib, @"C:\Users\Public\libwmp.xml");

            }
        }

        private void Mute(object param)
        {
            if ((_mediaElementObject = (MediaElement)param) != null)
            {
                if (_isInMuted == true)
                    _mediaElementObject.Volume = 0.5;
                else
                    _mediaElementObject.Volume = 0;
                IsInMuted = (!_isInMuted);
            }
        }

        private void OrderBy(String filter)
        {
            if (_mediaDirectory.mediaList.Count > 0)
            {
                Media tmp = _mediaDirectory.mediaList[_currentIndex];
                List<Media> lm = _mediaDirectory.orderBy(filter, 1);
                _lMedia.Clear();
                foreach (Media m in lm)
                    _lMedia.Add(m);
                _currentIndex = _mediaDirectory.mediaList.FindIndex(a => a == tmp);
                lastIdx = -1;
            }
        }

        private void UpOnPlaylist(object p)
        {
            System.Windows.Controls.ListView lv = (System.Windows.Controls.ListView)p;
            int idx = lv.SelectedIndex;
            if (idx < 0 && lastIdx >= 0)
                idx = lastIdx;
            if (idx >= 0)
            {
                int pos = (idx - 1);
                if (pos == -1)
                    pos = _mediaDirectory.mediaList.Count - 1;
                lastIdx = pos;
                _mediaDirectory.moveMediaToPos(_mediaDirectory.mediaList[idx], pos);
                _lMedia.Clear();
                foreach (Media m in _mediaDirectory.mediaList)
                    _lMedia.Add(m);
            }
        }

        private void DownOnPlaylist(object p)
        {
            System.Windows.Controls.ListView lv = (System.Windows.Controls.ListView)p;
            int idx = lv.SelectedIndex;
            if (idx < 0 && lastIdx >= 0)
                idx = lastIdx;
            if (idx >= 0)
            {
                int pos = (idx + 1) % _mediaDirectory.mediaList.Count;
                lastIdx = pos;
                _mediaDirectory.moveMediaToPos(_mediaDirectory.mediaList[idx], pos);
                _lMedia.Clear();
                foreach (Media m in _mediaDirectory.mediaList)
                    _lMedia.Add(m);
            }
        }

        private void Reverse()
        {
            if (_mediaDirectory.mediaList.Count > 0)
            {
                Media tmp = _mediaDirectory.mediaList[_currentIndex];
                _mediaDirectory.mediaList.Reverse();
                _lMedia.Clear();
                foreach (Media m in _mediaDirectory.mediaList)
                    _lMedia.Add(m);
                _currentIndex = _mediaDirectory.mediaList.FindIndex(a => a == tmp);
                lastIdx = -1;
            }
        }

        private void PlayPause(object param)
        {
            if ((_mediaElementObject = (MediaElement)param) != null)
            {
                if (!_StatePlay)
                {
                    if (DataSourceProperty == null && _mediaDirectory != null && _mediaDirectory.mediaList != null && _mediaDirectory.mediaList.Count > 0)
                    {
                        Media media = _mediaDirectory.mediaList[0];
                        DataSourceProperty = new Uri(@media.path);
                    }
                    _mediaElementObject.Play();
                }
                else
                    _mediaElementObject.Pause();
                StatePlay = (!_StatePlay);
            }
        }

        private void Stop(object param)
        {
            if ((_mediaElementObject = (MediaElement)param) != null)
            {
                _mediaElementObject.Stop();
                StatePlay = (!_StatePlay);
            }
        }

        private void ActionNext(object param)
        {
            if ((_mediaElementObject = (MediaElement)param) != null && _mediaDirectory.mediaList.Count > 0)
            {
                if (_isShuffled)
                    MediaShuffledPlay(_mediaElementObject);
                else
                {
                    List<Media> mediaList = _mediaDirectory.mediaList;

                    ++_currentIndex;
                    if (_currentIndex >= mediaList.Count)
                        _currentIndex = 0;
                    Media media = mediaList[(_currentIndex % mediaList.Count)];
                    String PathEncode = media.path;
                    DataSourceProperty = new Uri(@PathEncode);
                    _mediaElementObject.Play();
                    StatePlay = true;
                }
            }
        }

        public void SetMediaWithIndex(int index)
        {
            List<Media> mediaList = _mediaDirectory.mediaList;
            _currentIndex = index;
            Media media = mediaList[(_currentIndex % mediaList.Count)];
            String PathEncode = media.path;
            DataSourceProperty = new Uri(@PathEncode);
        }

        private void PlaylistSelected(object param)
        {
        }

        private void MediaShuffledPlay(MediaElement element)
        {
            List<Media> medialist = _mediaDirectory.mediaList;
            Random rand = new Random();
            int oldIndex = _currentIndex;

            _currentIndex = rand.Next(0, medialist.Count);
            if (_currentIndex == oldIndex)
            {
                _currentIndex++;
                if (_currentIndex >= medialist.Count)
                    _currentIndex = 0;
            }
            Media media = medialist[(_currentIndex % medialist.Count)];
            DataSourceProperty = new Uri(@media.path);
            element.Play();
            StatePlay = true;
        }

        private void MediaShuffled()
        {
            List<Media> medialist = _mediaDirectory.mediaList;
            Random rand = new Random();
            int oldIndex = _currentIndex;

            _currentIndex = rand.Next(0, medialist.Count);
            if (_currentIndex == oldIndex)
            {
                _currentIndex++;
                if (_currentIndex >= medialist.Count)
                    _currentIndex = 0;
            }
            Media media = medialist[(_currentIndex % medialist.Count)];
            DataSourceProperty = new Uri(@media.path);
        }

        private void NextMedia()
        {
            List<Media> medialist = _mediaDirectory.mediaList;

            if (medialist.Count > 0)
            {
                if (_isShuffled)
                    MediaShuffled();
                else
                {
                    ++_currentIndex;
                    if (_currentIndex >= medialist.Count)
                        _currentIndex = 0;
                    Media media = medialist[(_currentIndex % medialist.Count)];
                    DataSourceProperty = new Uri(@media.path);
                }
            }
        }

        private void ActionPrev(object param)
        {
            List<Media> mediaList = _mediaDirectory.mediaList;

            if ((_mediaElementObject = (MediaElement)param) != null && _mediaDirectory.mediaList.Count > 0)
            {
                if (_isShuffled)
                    MediaShuffledPlay(_mediaElementObject);
                else
                {
                    --_currentIndex;
                    if (_currentIndex < 0)
                        _currentIndex = mediaList.Count - 1;
                    Media media = mediaList[(_currentIndex % mediaList.Count)];
                    DataSourceProperty = new Uri(media.path);
                    _mediaElementObject.Play();
                    _StatePlay = true;
                }
            }
        }

        private void OpenPlaylist()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "XML Files (.xml)|*.xml";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;
            bool? userClickedOK = openFileDialog.ShowDialog();

            if (userClickedOK == true)
            {
                _mediaDirectory = WpfMvvmSample.Model.Classes.Model.deserialize(openFileDialog.FileName);
                _currentIndex = 0;
                _lMedia.Clear();
                List<Media> mediaList = _mediaDirectory.mediaList;

                foreach (Media m in mediaList)
                    _lMedia.Add(m);
                if (mediaList.Count > 0)
                {
                    Media media = mediaList[_currentIndex];
                    DataSourceProperty = new Uri(media.path);
                }
            }
        }
    }
}
