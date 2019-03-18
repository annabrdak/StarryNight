using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StarryNight.Model
{
    class Star
    {
        public Point Location { get; set; }

        public Star(Point location,bool rotating)
        {
            Location = location;
            Rotating = rotating;
        }

        public bool Rotating { get; set; }

        //Once you get your program working,
        //try adding a Boolean Rotating
        //property to the Star class and use it
        //to make some of your stars slowly spin
        //around
    }
}
