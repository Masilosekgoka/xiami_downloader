﻿using music_downloader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public static class XiamiUrl
{
    readonly static string url_action_fav = "http://www.xiami.com/app/android/fav?id={id}&type={type}";
    readonly static string url_action_unfav = "http://www.xiami.com/app/android/unfav?id={id}&type={type}";
    readonly static string url_album = "http://www.xiami.com/app/android/album?id={id}";
    public readonly static string url_artist = "http://www.xiami.com/app/android/artist?id={id}";
    public readonly static string url_artist_albums = "http://www.xiami.com/app/android/artist-albums?id={id}&page={page}";
    readonly static string url_artist_radio = "http://www.xiami.com/app/android/radio-artist?id={id}";
    readonly static string url_artist_top_song = "http://www.xiami.com/app/android/artist-topsongs?id={id}";
    readonly static string url_artsit_similars = "http://www.xiami.com/app/android/artist-similar?id={id}";
    public readonly static string url_collect = "http://www.xiami.com/app/android/collect?id={id}&uid={uid}";
    readonly static string url_grade = "http://www.xiami.com/app/android/grade?id={id}&grade={grade}";
    readonly static string url_lib_albums = "http://www.xiami.com/app/android/lib-albums?uid={uid}&page={page}";
    readonly static string url_lib_artists = "http://www.xiami.com/app/android/lib-artists?uid={uid}&page={page}";
    readonly static string url_lib_collects = "http://www.xiami.com/app/android/lib-collects?uid={uid}&page={page}";
    readonly static string url_lib_songs = "http://www.xiami.com/app/android/lib-songs?uid={uid}&page={page}";
    readonly static string url_myplaylist = "http://www.xiami.com/app/android/myplaylist?uid={uid}";
    readonly static string url_myradiosongs = "http://www.xiami.com/app/android/lib-rnd?uid={uid}";
    readonly static string url_playlog = "http://www.xiami.com/app/android/playlog?id={id}&uid={uid}";
    readonly static string url_radio = "http://www.xiami.com/app/android/radio?id={id}&uid={uid}";
    readonly static string url_radio_categories = "http://www.xiami.com/app/android/radio-category";
    readonly static string url_rndsongs = "http://www.xiami.com/app/android/rnd?uid={uid}";
    readonly static string url_search_all = "http://www.xiami.com/app/android/searchv1?key={key}";
    readonly static string url_search_parts = "http://www.xiami.com/app/android/search-part?key={key}&type={type}&page={page}";
    readonly static string fmtUrlPlaylistByIdAndType = "http://www.xiami.com/song/playlist/id/{0}/type/{1}";
    readonly static string fmtUrlSearchWithKeyAndPage = "http://www.xiami.com/app/nineteen/search/key/{0}/page/{1}";
    public readonly static string url_song = "http://www.xiami.com/app/iphone/song/id/{id}";
    readonly static string url_type_id = "http://www.xiami.com/app/android/{type}/id/{id}";
    public static string UrlPlaylistByIdAndType(string id, EnumMusicType type)
    {
        return url_type_id.WithParams(new Dictionary<string, string>{
                {"id",id},
                {"type",type.ToString()}
            });
    }
    public static string GoSong(string id)
    {
        return string.Format("http://www.xiami.com/song/{0}", id);
    }
    public static string GoArtist(string id)
    {
        return string.Format("http://www.xiami.com/artist/{0}", id);
    }
    public static string GoAlbum(string id)
    {
        return string.Format("http://www.xiami.com/album/{0}", id);
    }
    public static string GoCollect(string id)
    {
        return string.Format("http://www.xiami.com/song/showcollect/id/{0}", id);
    }
    public static string UrlSong(string id)
    {
        return url_song.WithParams(new Dictionary<string, string> { { "id", id } });
    }
    public static string UrlAlbum(string id)
    {
        return url_album.WithParams(new Dictionary<string, string> { { "id", id } });
    }
    public static string UrlArtistTopSong(string id)
    {
        return url_artist_top_song.WithParams(new Dictionary<string, string> { { "id", id } });
    }
    public static string UrlSearch(string key, int page, EnumMusicType type)
    {
        return url_search_parts.WithParams(new Dictionary<string, string>
            {
                {"key",key},
                {"type",type.ToString()+"s"},
                {"page",page.ToString()}
            });
    }
    public static string UrlSearchAll(string key)
    {
        return url_search_all.WithParams(new Dictionary<string, string> { { "key", key } });
    }
    public static string WithParams(this string format, Dictionary<string, string> dict)
    {
        if (string.IsNullOrWhiteSpace(format)) return format;
        foreach (var pair in dict)
        {
            var value = pair.Value ?? "";
            format = format.Replace('{' + pair.Key + '}', Uri.EscapeDataString(value));
        }
        return format;
    }

}