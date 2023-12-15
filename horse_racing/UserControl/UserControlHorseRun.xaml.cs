using horse_racing.cs;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace horse_racing.userControl
{
    /// <summary>
    /// UserControlHorseRun.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControlHorseRun : UserControl
    {
        public int ItemPosition { get; set; }
        public int ObstaclePosition { get; set; }
        public bool useItem = false;
        bool isJumping = false;
        int m = 0;
        int m2 = 0;
        int angle = 15;

        DispatcherTimer ReplaySoundTimer = new();
        MediaPlayer horseHoovesSound = new();
        DispatcherTimer timer = new();
        public Horse horse = new();
        Obstacle obstacle = new();
        Item item = new();

        public UserControlHorseRun()
        {
            InitializeComponent();
            horseHoovesSound.Open(new Uri(@"C:\Users\LT-0048\source\repos\horse_racing\horse_racing\Sound\말발굽소리.mp3", UriKind.Relative));
            horseHoovesSound.Play();
            ReplaySoundTimer.Interval = TimeSpan.FromSeconds(5);
            ReplaySoundTimer.Tick += new EventHandler(ReplaySound);
            ReplaySoundTimer.Start();
        }

        private void ReplaySound(object? sender, EventArgs e)
        {
            horseHoovesSound.Stop();
            horseHoovesSound.Play();
            horseHoovesSound.Position = new TimeSpan(0, 0, 0, 0, 2000);
        }

        private void SetTimer()
        {
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        public void SetHorse(Horse horse)
        {
            this.horse = horse;
            SetTimer();
        }

        private void GetItem()
        {
            DispatcherTimer itemTimer = new DispatcherTimer();
            itemTimer.Interval = TimeSpan.FromSeconds(3);
            itemTimer.Tick += new EventHandler(itemTimer_Tick);
            itemTimer.Start();

            if (item.BuffType == "sprint")
            {
                Run(horse.MaxSpeed);
                LegMovements();
            }
            /*else if(item.BuffType == "sturn")
            {
                useItem = true;
                Run(0);
            }*/
            else if(item.BuffType == "reverse")
            {
                useItem = true;
                Run(-horse.Speed);
            }
            else if (item.BuffType == "teleport" && useItem == false)
            {
                useItem = true;
                itemTimer.Interval = TimeSpan.FromMilliseconds(100);
                Random random = new Random();
                m = random.Next(900);
                Run(horse.Speed);
            }

            RemoveItem(this, new EventArgs());
        }

        private void itemTimer_Tick(object? sender, EventArgs e)
        {
            DispatcherTimer? itemTimer = sender as DispatcherTimer;
            itemTimer?.Stop();
            useItem = false;
            ItemPosition = 10000;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Check();
        }

        public event EventHandler<EventArgs> GoalIn;

        public event EventHandler<EventArgs> RemoveItem;

        public event EventHandler<EventArgs> RemoveObstacle;

        void Check()
        {
            if (m >= ItemPosition)
            {
                GetItem();
            }
            if(m >= ObstaclePosition - 10)
            {
                timer.Interval = TimeSpan.FromSeconds(100);
                DispatcherTimer ObstacleTimer = new DispatcherTimer();
                if (obstacle.Obstaclee(horse.Jump) == true)
                {
                    MediaPlayer horseSound = new MediaPlayer();
                    horseSound.Open(new Uri(@"C:\Users\LT-0048\source\repos\horse_racing\horse_racing\Sound\말울음소리.wav", UriKind.Relative));
                    horseSound.Play();
                    isJumping = true;
                    m2 -= 20;
                    rotate.Angle = -20;
                    foreLegs.Angle = -50;
                    hindLegs.Angle = 30;
                    timer.Interval = TimeSpan.FromMilliseconds(100);
                    ObstacleTimer.Interval = TimeSpan.FromSeconds(2);
                    ObstacleTimer.Tick += jumpTimer_Tick;
                    ObstacleTimer.Start();
                }
                else
                {
                    ObstacleTimer.Interval = TimeSpan.FromSeconds(3);
                    ObstacleTimer.Tick += sturnTimer_Tick;
                    ObstacleTimer.Start();
                }
                ReplaySoundTimer.Stop();
                horseHoovesSound.Stop();
                ObstaclePosition = 10000;
            }
            if(m <= ItemPosition && useItem == true && item.BuffType == "reverse")
            {
                Run(-horse.Speed-3);
            }
            if (m >= 1000)
            {
                timer?.Stop();
                hindLegs.Angle = 0;
                foreLegs.Angle = 0;
                ReplaySoundTimer.Stop();
                horseHoovesSound.Stop();
                GoalIn(this, new EventArgs());
                return;
            }
            else if(useItem == false)
            {
                Run(horse.Speed);
            }
        }

        private void jumpTimer_Tick(object? sender, EventArgs e)
        {
            DispatcherTimer? dispatcherTimer = sender as DispatcherTimer;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            isJumping = false;
            m2 = 0;
            rotate.Angle = 0;
            foreLegs.Angle = 0;
            hindLegs.Angle = 0;
            dispatcherTimer?.Stop();
            RemoveObstacle(this, new EventArgs());
            ReplaySoundTimer.Start();
            horseHoovesSound.Play();
        }

        private void sturnTimer_Tick(object? sender, EventArgs e)
        {
            DispatcherTimer? dispatcherTimer = sender as DispatcherTimer;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer?.Stop();
            RemoveObstacle(this, new EventArgs());
            ReplaySoundTimer.Start();
            horseHoovesSound.Play();
        }

        void Run(int speed)
        {
            if (horse.Stamina < 0)
            {
                m += (speed - 2);
            }
            else
            {
                m += speed;
                horse.Stamina -= 1;
            }
            
            LegMovements();
            this.Margin = new Thickness(m, m2, 0, 0);
        }

        void LegMovements()
        {
            if (angle > 0 && isJumping == false)
            {
                hindLegs.Angle = angle;
                angle *= -1;
                foreLegs.Angle = angle;
            }
            else if (angle < 0 && isJumping == false)
            {
                hindLegs.Angle = angle;
                angle = 15;
                foreLegs.Angle = angle;
            }
        }

        public void GetItem(Item item)
        {
            this.item = item;
            ItemPosition = item.Position;
        }

        public void GetObstacle(Obstacle obstacle)
        {
            this.obstacle = obstacle;
            ObstaclePosition = obstacle.Position;
        }
    }
}