using horse_racing.userControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace horse_racing.cs
{
    public class FinishLine
    {
        public void MakeFinishLine(Page1 page1)
        {
            page1.finishLine.Children.Clear();
            for(int i=0; i < page1.canvas.Children.Count; i++)
            {
                UserControlFinishLine userControlFinishLine = new UserControlFinishLine();
                page1.finishLine.Children.Add(userControlFinishLine);
            }
        }
    }
}
