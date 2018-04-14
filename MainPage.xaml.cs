using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App13
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

       

        public async Task<StorageFile> Load()
        {
            try
            {
                var httpClient = new HttpClient();
                var buffer = await httpClient.GetBufferAsync(new Uri(mybox.Text));
                var file = await KnownFolders.MusicLibrary.CreateFileAsync("neusong.mp3", CreationCollisionOption.ReplaceExisting);
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await stream.WriteAsync(buffer);
                    await stream.FlushAsync();
                }
                Uri pathUri = new Uri(mybox.Text);
                media.Source = pathUri;
                media.Play();
                return file;
                
            }
            catch { }
            return null;
        }

        private async void search()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".mp4");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file 
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                media.SetSource(stream, file.ContentType);
                media.Play();
            }
            else
            {
                
            }
        }

        private void navview_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                switch (item.Tag)
                {
                    case "play":
                        media.Play();
                        break;

                    case "pause":
                        media.Pause();
                        break;

                    case "stop":
                        media.Stop();
                        break;

                    case "search":
                        search();
                        break;

                    case "cloud":
                        Uri pathUri = new Uri(mybox.Text);
                        media.Source = pathUri;
                        media.Play();
                        break;

                    case "save":
                        Load();
                        break;
                }
            }
        }
    }
}
