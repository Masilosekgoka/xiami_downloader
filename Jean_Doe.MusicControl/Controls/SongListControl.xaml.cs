﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using Artwork.MessageBus;
using Jean_Doe.Common;
namespace Jean_Doe.MusicControl
{
    /// <summary>
    /// Interaction logic for SongListControl.xaml
    /// </summary>
    public partial class SongListControl : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string prop)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
        #region list methods
        public virtual void Add(SongViewModel song)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                items.Add(song);
            }));
        }
        public virtual void Insert(int index,SongViewModel song)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                items.Insert(index,song);
            }));
        }
        public void Remove(SongViewModel song)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                items.Remove(song);
            }));
        }
        public SongViewModel GetItemById(string id)
        {
            var res = items.FirstOrDefault(x => x.Id == id) as SongViewModel;
            return res;
        }
        public void Clear()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                items.Clear();
            }));
        }
        #endregion

        MusicViewModelList items;
        public MusicViewModelList Items { get { return items; } }
        private int itemsCount;
        public int ItemsCount { get { return itemsCount; } set { itemsCount = value; Notify("ItemsCount"); } }
        private int selectCount;
        public int SelectCount { get { return selectCount; } set { selectCount = value; Notify("SelectCount"); } }
        public IEnumerable<SongViewModel> SelectedItems
        {
            get
            {
                return dataGrid.SelectedItems.OfType<SongViewModel>();
            }
        }

        public SongListControl()
        {
            InitializeComponent();
            MessageBus.Instance.Subscribe(this);
            items = new MusicViewModelList();
            items.CollectionChanged += items_CollectionChanged;
            dataGrid.ItemsSource = items;
            dataGrid.SelectionChanged += dataGrid_SelectionChanged;

        }


        public string SavePath { get { return Items.SavePath; } set { Items.SavePath = value; } }
        public virtual void Save()
        {
            items.Save();
        }
        public virtual void Load()
        {
            items.Load();
        }
       
        protected virtual void btn_open_click(object sender, RoutedEventArgs e)
        {
            var t = sender as FrameworkElement;
            if(t == null) return;
            var x = t.DataContext as SongViewModel;
            if(x == null) return;
            x.Open();
        }

        protected virtual async void link_album(object sender, RoutedEventArgs e)
        {
            var t = (sender as Hyperlink).DataContext as IHasAlbum;
            if(t == null) return;
            var id = t.AlbumId;
            await SearchManager.GetSongOfType(t.AlbumId, EnumXiamiType.album);
        }

        void fitToContent()
        {
            foreach(DataGridColumn column in dataGrid.Columns)
            {
                //if you want to size ur column as per the cell content
                column.Width = DataGridLength.SizeToCells;
            }
        } 

        protected virtual async void link_artist(object sender, RoutedEventArgs e)
        {
            var t = (sender as Hyperlink).DataContext as IHasArtist;
            if(t == null) return;
            await SearchManager.GetSongOfType(t.ArtistId, EnumXiamiType.artist);
        }
        protected virtual async void link_collection(object sender, RoutedEventArgs e)
        {
            var t = (sender as Hyperlink).DataContext as IHasCollection;
            if (t == null) return;
            await SearchManager.GetSongOfType(t.CollectionId, EnumXiamiType.collect);
        }

        protected virtual  void go_song(object sender, RoutedEventArgs e)
        {
            var t = (sender as Hyperlink).DataContext as MusicViewModel;
            if (t == null) return;
            RunProgramHelper.RunProgram(XiamiUrl.GoSong(t.Id), null);
        }
        protected virtual  void go_artist(object sender, RoutedEventArgs e)
        {
            var t = (sender as Hyperlink).DataContext as IHasArtist;
            if (t == null) return;
            RunProgramHelper.RunProgram(XiamiUrl.GoArtist(t.ArtistId), null);
        }
        protected virtual  void go_album(object sender, RoutedEventArgs e)
        {
            var t = (sender as Hyperlink).DataContext as IHasAlbum;
            if (t == null) return;
            RunProgramHelper.RunProgram(XiamiUrl.GoAlbum(t.AlbumId), null);
        }
        protected virtual  void go_collect(object sender, RoutedEventArgs e)
        {
            var t = (sender as Hyperlink).DataContext as MusicViewModel;
            if (t == null) return;
            RunProgramHelper.RunProgram(XiamiUrl.GoCollect(t.Id), null);
        }
        void items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ItemsCount = Items.Count;
                fitToContent();
            }));
            Save();
        }
        void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
          {
              SelectCount = SelectedItems.Count();
          }));
        }
        private void select_all_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.SelectAll();
        }
        private void unselect_all_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.UnselectAll();
        }

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as SongViewModel;
            if(item == null) return;
            PlayItem = item;
        }
        private static SongViewModel playItem = null;

        public static SongViewModel PlayItem
        {
            get { return playItem; }
            set
            {
                if(playItem == value)
                {
                    playItem.TogglePlay();
                    return;
                }
                if(playItem != null)
                    playItem.Stop();
                playItem = value;
                if(playItem != null)
                    playItem.Play();
            }
        }

    }
}