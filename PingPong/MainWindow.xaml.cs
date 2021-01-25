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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PingPong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thickness ballStartPosition, ballCurrentPosition, ballNextPosition;
        private Thickness padStartPosition, padCurrentPosition;
        private ThicknessAnimation ballAnimation, padAnimation;
        private DateTime startTime;
        private bool isStarted;
        private float speedFactor;
        private readonly Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            ballStartPosition = ellipseBall.Margin;
            ballNextPosition = new Thickness();
            padStartPosition = rectanglePad.Margin;
            padCurrentPosition = rectanglePad.Margin;
            isStarted = false;
            KeyDown += Navigate;
            ellipseBall.LayoutUpdated += BallLayoutUpdated;
        }
        private void NewGame(object sender, RoutedEventArgs e)
        {
            StartGame();
        }
        private void BallLayoutUpdated(object sender, EventArgs e)
        {
            if (isStarted)
            {
                if((int)ellipseBall.Margin.Left + ellipseBall.Width >= MWindow.Width - 10)
                {
                    ballNextPosition.Left = 0;
                    AnimateBall();
                }
                else if((int)ellipseBall.Margin.Left <= 10)
                {
                    ballNextPosition.Left = MWindow.Width;
                    AnimateBall();
                }
                else if((int)ellipseBall.Margin.Top <= 10)
                {
                    ballNextPosition.Top = MWindow.Height;
                    AnimateBall();
                }
                else if((int)ellipseBall.Margin.Top + ellipseBall.Height >= (int)rectanglePad.Margin.Top)
                {
                    if (ellipseBall.Margin.Left + ellipseBall.Width / 2 <= (int)rectanglePad.Margin.Left + rectanglePad.Width / 2 && ellipseBall.Margin.Left + ellipseBall.Width / 2 >= (int)rectanglePad.Margin.Left)
                    {
                        speedFactor *= (float)1.05;
                        ballNextPosition.Top = 0;
                        ballNextPosition.Left -= MWindow.Width / 3;
                        AnimateBall();
                    }
                    else if ((int)ellipseBall.Margin.Left + ellipseBall.Width / 2 >= (int)rectanglePad.Margin.Left + rectanglePad.Width / 2 && (int)ellipseBall.Margin.Left + ellipseBall.Width / 2 <= (int)rectanglePad.Margin.Left + rectanglePad.Width)
                    {
                        speedFactor *= (float)1.05;
                        ballNextPosition.Top = 0;
                        ballNextPosition.Left += MWindow.Width / 3;
                        AnimateBall();
                    }
                }
                if((int)ellipseBall.Margin.Top + ellipseBall.Height / 2 >= MWindow.Height - ellipseBall.Height)
                {
                    MessageBox.Show($"Game over. Game lasted {(DateTime.Now-startTime).TotalSeconds} seconds.");
                    StopGame();
                }
            }
        }
        private void Navigate(object sender, KeyEventArgs e)
        {
            padCurrentPosition = rectanglePad.Margin;
            if (e.Key == Key.Left && padCurrentPosition.Left > 0)
            {
                AnimatePad(-100);
            }
            if (e.Key == Key.Right && padCurrentPosition.Left < MWindow.Width - rectanglePad.Width)
            {
                AnimatePad(100);
            }
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void StartGame()
        {
            isStarted = true;
            startTime = DateTime.Now;
            ballCurrentPosition = ballStartPosition;
            int startPos = rand.Next(0, (int)(MWindow.Width-ellipseBall.Width));
            ballNextPosition = new Thickness(startPos - ellipseBall.Width / 2, MWindow.Height - ellipseBall.Height - ellipseBall.Height / 2, 0, 0);
            speedFactor = 1;
            AnimateBall();
        }
        private void StopGame()
        {
            isStarted = false;
        }
        private void AnimateBall()
        {
            ballAnimation = new ThicknessAnimation
            {
                From = ballCurrentPosition,
                To = ballNextPosition,
                Duration = new Duration(TimeSpan.FromMilliseconds(5)),
                SpeedRatio = speedFactor / BallTravelDistance()
            };
            ellipseBall.BeginAnimation(Ellipse.MarginProperty, ballAnimation, HandoffBehavior.SnapshotAndReplace);
        }
        private void AnimatePad(Int32 diff)
        {
            padAnimation = new ThicknessAnimation
            {
                From = padCurrentPosition,
                To = new Thickness(rectanglePad.Margin.Left + diff, rectanglePad.Margin.Top, 0, 0), 
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
            };
            rectanglePad.BeginAnimation(Rectangle.MarginProperty, padAnimation, HandoffBehavior.SnapshotAndReplace);
        }
        private double BallTravelDistance()
        {
            return Math.Sqrt(((ballCurrentPosition.Left - ballNextPosition.Left) * (ballNextPosition.Left - ballNextPosition.Left)) + ((ballCurrentPosition.Top - ballNextPosition.Top) * (ballCurrentPosition.Top - ballNextPosition.Top)));
        }
    }
}
