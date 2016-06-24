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
using WpfMvvmSample.ViewModel;
using WpfMvvmSample.Model.Classes;

namespace WpfMvvmSample.View
{
    public partial class MediaPlayerView : Window
    {
   
        public MediaPlayerView()
        {
            InitializeComponent();         
        }

        private void myMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            var mediaPlayerViewModel = (WpfMvvmSample.ViewModel.MediaPlayerViewModel)DataContext;
            if (mediaPlayerViewModel.NextMediaCommand.CanExecute(null))
                mediaPlayerViewModel.NextMediaCommand.Execute(null);

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InputTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            var mediaPlayerViewModel = (WpfMvvmSample.ViewModel.MediaPlayerViewModel)DataContext;
            MediaDirectory md = mediaPlayerViewModel.getLib();
            List<Media> lm = md.filterBy("all", tb.Text);
            mediaPlayerViewModel.setLib(lm);
        }

        private void DropList_DragEnter(object sender, DragEventArgs e)
        {
        }


        private void DropList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var mediaPlayerViewModel = (WpfMvvmSample.ViewModel.MediaPlayerViewModel)DataContext;
                MediaDirectory md = mediaPlayerViewModel.getPlaylist();

                foreach (string f in files)
                {
                    Media m = new Media();
                    m.path = f;
                    Model.Classes.Model.getFileMetaData(m);
                    md.mediaList.Add(m);
                }
                mediaPlayerViewModel.setPlaylist(md.mediaList);
            }
        }

        private void DropListLib_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void DropListLib_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var mediaPlayerViewModel = (WpfMvvmSample.ViewModel.MediaPlayerViewModel)DataContext;
                MediaDirectory md = mediaPlayerViewModel.getLib();

                foreach (string f in files)
                {
                    Media m = new Media();
                    m.path = f;
                    Model.Classes.Model.getFileMetaData(m);
                    md.mediaList.Add(m);
                }
                mediaPlayerViewModel.setLib(md.mediaList);
                Model.Classes.Model.serialize(mediaPlayerViewModel.getLib(), @"C:\Users\Public\libwmp.xml");
            }
        }

        private void lvPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mediaPlayerViewModel = (WpfMvvmSample.ViewModel.MediaPlayerViewModel)DataContext;

            if (lvPlaylist.SelectedItem != null)
            {
                mediaPlayerViewModel.SetMediaWithIndex(lvPlaylist.SelectedIndex);
                myMediaElement.Play();
                mediaPlayerViewModel.StatePlay = true;
            }
        }

        private void lvPlaylistLib_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mediaPlayerViewModel = (WpfMvvmSample.ViewModel.MediaPlayerViewModel)DataContext;
            if (mediaPlayerViewModel.AddLibToPlaylistCommand.CanExecute(lvPlaylistLib))
                mediaPlayerViewModel.AddLibToPlaylistCommand.Execute(lvPlaylistLib);
            
        }

    }
}
