using horse_racing.userControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace horse_racing.cs
{
    public class Track
    {
        public int TrackWidth { get; set; }
        public int TrackHeight { get; set; }
        public int ItemPosition { get; set; }
        public int ObstaclePosition { get; set; }
        public string Fill { get; set; }

        Random random = new Random();
        public List<Track> trackList = new List<Track>();

        public void RenderTrack(Page1 page1, Track track)
        {
            page1.canvas.Children.Clear();
            for (int i = 0; i < page1.xListBox.Items.Count; i++)
                page1.canvas.Children.Add(track.MakeTrack(track.SetTrack()));
        }

        public List<Track> AddTrackList(Track track)
        {
            this.trackList.Add(track);
            return this.trackList;
        }

        public Track SetTrack()
        {
            Track track = new()
            {
                TrackWidth = 1000,
                TrackHeight = 50,
                ItemPosition = random.Next(300, 800),
                ObstaclePosition = random.Next(100, 250),
                Fill = RandomColor(),
            };
            AddTrackList(track);
            return track;
        }

        public UserControlTrack MakeTrack(Track track)
        {
            UserControlTrack userControlTrack = new() { DataContext = track };
            userControlTrack.hidden.Text = track.ItemPosition.ToString();
            userControlTrack.hidden2.Text = track.ObstaclePosition.ToString();
            return userControlTrack;
        }

        private string RandomColor()
        {
            int red = random.Next(0, 256);
            int green = random.Next(0, 256);
            int blue = random.Next(0, 256);
            string specifier = "X";
            string hex1 = red.ToString(specifier);
            string hex2 = green.ToString(specifier);
            string hex3 = blue.ToString(specifier);
            if (hex1.Length != 2)
                hex1 += hex1;
            if (hex2.Length != 2)
                hex2 += hex2;
            if (hex3.Length != 2)
                hex3 += hex3;
            string fill = "#" + hex1 + hex2 + hex3;
            return fill;
        }
    }
}
