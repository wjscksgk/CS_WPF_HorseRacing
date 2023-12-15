using horse_racing.cs;
using horse_racing.userControl;
using horse_racing.view;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace horse_racing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<UserControlHorseRun> horses = new();
        Horse horse = new();
        Track track = new();
        Page1 page1;
        int count = 5;

        public MainWindow()
        {
            InitializeComponent();
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            page1 = new Page1(statBlock);
            page1.result += RankResult;
            frame.Content = page1;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (page1.horseList.Count <= 0)
            {
                MessageBox.Show("먼저 Add Horse버튼을 클릭해 말을 생성하세요.");
                return;
            }
            StartCount();
        }

        private void StartCount()
        {
            DispatcherTimer startTimer = new DispatcherTimer();
            startTimer.Interval = TimeSpan.FromSeconds(1);
            startTimer.Tick += new EventHandler(startTimer_Tick);
            startTimer.Start();
        }

        private void startTimer_Tick(object? sender, EventArgs e)
        {
            DispatcherTimer? startTimer = sender as DispatcherTimer;
            Opacity = 0.5;
            countBlock.Text = count.ToString();
            countBlock.Visibility = Visibility.Visible;
            page1.addButton.Visibility = Visibility.Hidden;
            page1.removeButton.Visibility = Visibility.Hidden;
            startButton.Click -= StartButton_Click;
            if (count < 1)
            {
                startTimer?.Stop();
                Opacity = 1;
                countBlock.Visibility = Visibility.Hidden;
                GameStart();
            }
            count--;
        }

        private void GameStart()
        {
            startButton.Content = "진행중";
            track.RenderTrack(page1, track);
            horse.StartRun(page1);
        }

        public void RankResult(object sender, EventArgs e)
        {
            ResultPage page2 = new();
            frame.Content = page2;
            sp.Children.Clear();
            UserControlPodium podium = new UserControlPodium();
            for (int i = 0; i < page1.horsess.Count; i++)
            {
                if(page1.horsess[i].hidden2.Text == "1")
                {
                    podium.first.Text = page1.horsess[i].hidden.Text;
                }
                else if (page1.horsess[i].hidden2.Text == "2")
                {
                    podium.second.Text = page1.horsess[i].hidden.Text;
                }
                else if (page1.horsess[i].hidden2.Text == "3")
                {
                    podium.third.Text = page1.horsess[i].hidden.Text;
                }
                else
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Margin = new Thickness(30, 0, 0, 10);
                    textBlock.Text = $"{i + 1}등: " + page1.horsess[i].hidden.Text;
                    page2.rankPanel.Children.Add(textBlock);
                }
            }
            Button restartButton = new Button();
            restartButton.Margin = new Thickness(550, 0, 0, 0);
            restartButton.Content = "다시하기";
            restartButton.Width = 100;
            restartButton.Height = 40;
            restartButton.Click += RestartButton_Click;
            sp.Children.Add(restartButton);
            page2.podiumPanel.Children.Add(podium);
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow(); 
            Application.Current.MainWindow = newWindow;
            newWindow.Show(); 
            this.Close();
        }
    }
}