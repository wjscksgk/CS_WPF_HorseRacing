using horse_racing.userControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace horse_racing.cs
{
    public class Horse
    {
        public string? Name { get; set; }
        public int Speed { get; set; }
        public int MaxSpeed { get; set; }
        public int Jump { get; set; }
        public int Stamina { get; set; }

        public List<Horse> horseList = new List<Horse>();
        Track track = new();
        Random rand = new();

        public List<Horse> AddHorseList()
        {
            horseList.Add(MakeHorse());
            return horseList;
        }
        public List<Horse> SetHorseList(List<Horse> horseList)
        {
            this.horseList = horseList;
            return horseList;
        }
        private Horse MakeHorse()
        {
            Horse horse = new()
            {
                Name = SetHorseName(),
                Speed = SetHorseSpeed(),
                MaxSpeed = SetHorseMaxSpeed(),
                Jump = SetHorseJump(),
                Stamina = SetHorseStamina()
            };
            return horse;
        }

        private string SetHorseName()
        {
            string[] firstNameList = [ "Swift", "Fast", "Volant", "Endurance", "Strong", "Amazing", "Awesome", "Legendary", "Epic", "Hero" ];
            string[] secondNameList = [ "Horse", "Unicorn", "Zebra", "Foal", "Pony" ];
            string name = firstNameList[rand.Next(9)] + " " + secondNameList[rand.Next(4)];
            return name;
        }
        private int SetHorseSpeed()
        {
            int speed = rand.Next(5, 10);
            return speed;
        }
        private int SetHorseMaxSpeed()
        {
            int maxSpeed = rand.Next(10, 13);
            return maxSpeed;
        }
        private int SetHorseJump()
        {
            int jump = rand.Next(2, 8);
            return jump;
        }
        private int SetHorseStamina()
        {
            int stamina = rand.Next(100, 131);
            return stamina;
        }
        public void StartRun(Page1 page1)
        {
            FinishLine finishLine = new FinishLine();
            finishLine.MakeFinishLine(page1);
            page1.TrackOnHorse();
        }
    }
}
