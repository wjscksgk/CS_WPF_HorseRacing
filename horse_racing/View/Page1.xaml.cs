using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using horse_racing.cs;
using horse_racing.userControl;

namespace horse_racing
{
    /// <summary>
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Page1 : Page
    {
        public List<Horse> horseList = new List<Horse>();
        public List<UserControlHorseRun> horsess = new List<UserControlHorseRun>();
        Horse horse = new();
        Horse? selectedHorse;
        StackPanel statBlock;
        string? changeName;
        int selectedIndex = 0;
        bool isStarted = false;

        public Page1(StackPanel e)
        {
            InitializeComponent();
            xListBox.SelectionChanged += ListBox_SelectionChanged;
            statBlock = e;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if(listBox.SelectedIndex >= 0)
            {
                UserControlHorse? userControlHorse = listBox.SelectedItem as UserControlHorse;
                selectedHorse = userControlHorse?.DataContext as Horse;
                RenderHorseStat(selectedHorse);
            }
        }

        public void RenderHorseStat(Horse selectedHorse)
        {
            UserControlStat userControlStat = new() { DataContext = selectedHorse };
            userControlStat.textBox.TextChanged += TextBox_TextChanged;
            userControlStat.textBox.KeyUp += TextBox_KeyUp;
            userControlStat.changeButton.Click += ChangeButton_Click;
            statBlock.Children.Clear();
            statBlock.Children.Add(userControlStat);
            if (isStarted == true)
            {
                userControlStat.textBox.Visibility = Visibility.Hidden;
                userControlStat.changeButton.Visibility = Visibility.Hidden;
            }
            if (selectedHorse == null)
            {
                statBlock.Children.Clear();
                statBlock.Children.Add(Textt());
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
                ChangeHorseName();  
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeHorseName();
        }

        void ChangeHorseName()
        {
            if (xListBox.SelectedIndex != -1)
                selectedIndex = xListBox.SelectedIndex;
            else
                xListBox.SelectedItem = xListBox.Items[selectedIndex];
            horseList[selectedIndex].Name = changeName;

            EasterEgg();
            RenderHorseStat(selectedHorse);
            RenderHorseList();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            changeName = "";
            changeName = textBox?.Text;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            xListBox.Visibility = Visibility.Visible;
            addButton.Margin = new Thickness(0,10,0,10);
            horseList = horse.AddHorseList();
            RenderHorseList();
        }

        void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (xListBox.SelectedIndex == -1)
            {
                MessageBox.Show("먼저 삭제하고 싶은 말을 선택하세요.");
                return;
            }

            horseList.RemoveAt(xListBox.SelectedIndex);

            if (horseList.Count == 0)
                addButton.Margin = new Thickness(20, 10, 20, 10);

            statBlock.Children.Clear();
            statBlock.Children.Add(Textt());
            selectedHorse = null;
            RenderHorseList();
        }

        TextBlock Textt()
        {
            TextBlock textBlock = new();
            textBlock.Text = "말을 선택하면 선택한 말의 정보를 볼 수 있습니다.";
            textBlock.FontSize = 20;
            textBlock.Margin = new Thickness(20, 30, 0, 0);
            return textBlock;
        }

        private void RenderHorseList()
        {
            xListBox.Items.Clear();
            foreach (Horse horse in horseList)
            {
                UserControlHorse userControlHorse = new() { DataContext = horse };
                xListBox.Items.Add(userControlHorse);
            }
        }
        public void TrackOnHorse()
        {
            int i = 0;
            isStarted = true;
            RenderHorseStat(selectedHorse);
            horse.SetHorseList(horseList);
            foreach (UserControlTrack userControltrack in canvas.Children)
            {
                UserControlHorseRun horsee = new()
                {
                    Margin = new Thickness(0, 0, 0, 0),
                    DataContext = horseList[i]
                };
                horsee.GoalIn += Horsee_GoalIn;
                horsee.RemoveItem += Horsee_RemoveItem;
                horsee.RemoveObstacle += Horsee_RemoveObstacle;
                horsee.SetHorse(horseList[i]);
                horsee.GetItem(TrackOnItem(userControltrack));
                horsee.GetObstacle(TrackOnObstacle(userControltrack));
                userControltrack.sp2.Children.Add(horsee);
                i++;
            }
        }

        private void Horsee_RemoveItem(object? sender, EventArgs e)
        {
            UserControlHorseRun? userControlHorseRun = sender as UserControlHorseRun;
            PanelChildrenClear(userControlHorseRun, 0);
        }

        private void Horsee_RemoveObstacle(object? sender, EventArgs e)
        {
            UserControlHorseRun? userControlHorseRun = sender as UserControlHorseRun;
            PanelChildrenClear(userControlHorseRun, 1);
        }

        void PanelChildrenClear(UserControlHorseRun userControlHorseRun, int i)
        {
            foreach (UserControlTrack userControltrack in canvas.Children)
            {
                foreach (UserControlHorseRun horse in userControltrack.sp2.Children)
                {
                    if (userControlHorseRun == horse && i == 0)
                        userControltrack.itemPanel.Children.Clear();
                    else if (userControlHorseRun == horse && i == 1)
                        userControltrack.obstaclePanel.Children.Clear();
                }
            }
        }

        private void Horsee_GoalIn(object? sender, EventArgs e)
        {
            UserControlHorseRun? userControlHorseRun = sender as UserControlHorseRun;
            horsess.Add(userControlHorseRun);
            if (horsess.Count == 1)
                userControlHorseRun.hidden2.Text = "1";
            if(horsess.Count == 2)
                userControlHorseRun.hidden2.Text = "2";
            if (horsess.Count == 3)
                userControlHorseRun.hidden2.Text = "3";
            if (horsess.Count == horseList.Count)
                result(this, new EventArgs());
        }

        public event EventHandler<EventArgs> result;

        public Item TrackOnItem(UserControlTrack userControltrack)
        {
            Item item = new();
            UserControlItem userControlItem = new UserControlItem();
            userControlItem.DataContext = item.Itemm();
            userControltrack.itemPanel.Children.Add(userControlItem);
            item.Position = int.Parse(userControltrack.hidden.Text);
            return item;
        }
        public Obstacle TrackOnObstacle(UserControlTrack userControlTrack)
        {
            Obstacle obstacle = new();
            UserControlObstacle userControlObstacle = new UserControlObstacle();
            userControlTrack.obstaclePanel.Children.Add(userControlObstacle);
            obstacle.Position = int.Parse(userControlTrack.hidden2.Text);
            return obstacle;
        }

        void EasterEgg()
        {
            if (horseList[selectedIndex].Name == "Secretariat")
            {
                horseList[selectedIndex].Speed = 20;
                horseList[selectedIndex].MaxSpeed = 30;
                horseList[selectedIndex].Jump = 10;
            }
        }
    }
}
