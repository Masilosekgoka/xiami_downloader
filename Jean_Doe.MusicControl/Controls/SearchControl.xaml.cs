﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Artwork.MessageBus;
using Artwork.MessageBus.Interfaces;
using Jean_Doe.Common;

namespace Jean_Doe.MusicControl
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl, IHandle<MsgSearchStateChanged>
    {
        public SearchControl()
        {
            InitializeComponent();
            MessageBus.Instance.Subscribe(this);
            refresh();
            btn_search.DataContext = this;
            btn_search.Click += btn_search_Click;
        }
        void refresh()
        {
            if(hs == null) return;
            hs.SearchType = SearchType;
            hs.SavePath = SavePath;
            hs.Load();
        }

        private EnumXiamiType musicType;

        public EnumXiamiType MusicType
        {
            get { return musicType; }
            set { musicType = value; }
        }

        private EnumSearchType searchType;
        public EnumSearchType SearchType
        {
            get { return searchType; }
            set
            {
                searchType = value;
                refresh();
            }
        }

        public string SavePath { get { return System.IO.Path.Combine(Global.BasePath, "history_" + SearchType.ToString() + ".xml"); } }

        static T FindParentOfType<T>(DependencyObject e) where T : DependencyObject
        {
            var p = LogicalTreeHelper.GetParent(e);
            if(p == null) return null;
            if(p is T) return p as T;
            return FindParentOfType<T>(p);
        }

        async void btn_search_Click(object sender, RoutedEventArgs e)
        {
            if(SearchManager.State == EnumSearchState.Finished)
            {
                await search();
            }
            else
            {
                SearchManager.Cancel();
            }
        }
        async Task search()
        {
            var key = hs.Text.Trim();
            switch(SearchType)
            {
                case EnumSearchType.key:
                    await SearchManager.Search(key, MusicType);
                    break;
                case EnumSearchType.url:
                    await SearchManager.SearchByUrl(key);
                    break;
                default:
                    break;
            }

        }
        public void Handle(MsgSearchStateChanged message)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                switch(message.State)
                {
                    case EnumSearchState.Started:
                        img_search.Visibility = Visibility.Collapsed;
                        img_stop.Visibility = Visibility.Visible;
                        break;
                    case EnumSearchState.Working:
                        break;
                    default:
                        img_stop.Visibility = Visibility.Collapsed;
                        img_search.Visibility = Visibility.Visible;
                        break;
                }
            }));
        }

        private void combo_xiami_type_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var x = (combo_xiami_type.SelectedItem as ComboBoxItem).Tag;
            if(x is EnumXiamiType)
                MusicType = (EnumXiamiType)x;
        }

        private void combo_search_type_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var x = (combo_search_type.SelectedItem as ComboBoxItem).Tag;
            if(x is EnumSearchType)
                SearchType = (EnumSearchType)x;
        }
    }
}