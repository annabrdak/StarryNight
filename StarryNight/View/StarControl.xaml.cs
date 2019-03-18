using System;
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
using System.Windows.Media.Animation;
using System.IO.Packaging;
using System.Reflection;
using System.Windows.Markup;

namespace StarryNight.View
{
    /// <summary>
    /// Interaction logic for StarControl.xaml
    /// </summary>
    public partial class StarControl : UserControl
    {
        Storyboard fadeInStoryboard;
        Storyboard fadeOutStoryboard;
        Storyboard rotateStoryboard;

        public StarControl()
        {
            InitializeComponent();
            
            

            fadeInStoryboard = FindResource("fadeInStoryboard") as Storyboard;
            fadeOutStoryboard = FindResource("fadeOutStoryboard") as Storyboard;
            rotateStoryboard = FindResource("rotateStoryboard") as Storyboard;


        }

        public void FadeIn()
        {
            fadeInStoryboard.Begin();
            
        }

        public void FadeOut()
        {
            fadeOutStoryboard.Begin();
        }

        public void Rotate(bool rotating)
        {
            if(rotating)
                rotateStoryboard.Begin();
        }

        
    }
}
