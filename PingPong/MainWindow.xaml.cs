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
        public MainWindow()
        {
            InitializeComponent();
            ballStartPosition = ellipseBall.Margin;
            padStartPosition = rectanglePad.Margin;
            padCurrentPosition = rectanglePad.Margin;
            ballNextPosition = new Thickness();
            isStarted = false;
            KeyDown += Navigate;
            ellipseBall.LayoutUpdated += BallLayoutUpdated;
        }
        private void NewGame(object sender, RoutedEventArgs e)
        {
            StartGame();
        }
        /// <summary>
        /// Szeretnénk kezelni minden esetet, amikor a labda falnak vagy az ütönek ütközik, ezt a labda és az üto Margin attribútumának elemei (Top, Left) és az ablak méretei alapján állapíthatjuk meg. 
        /// A vesztes eset kivételével minden eset végén hívjuk meg az AnimateBall metódust. 
        /// A következo eseteket kezeljük: 
            /// – A labda az ütonek ütközik balról: növeljük meg a labda sebességét 5%-kal! 
                /// Állítsuk be a ballNextPosition Top propertyjét 0-ra (felfelé akarunk mozogni), a Left propertyjét pedig az eddigi pozícióhoz képest toljuk el pl. -200-zal(ablak szélessége / 3), mert bal felé is szeretnénk haladni. 
            /// – A labda az ütonek ütközik jobbról: növeljük meg a labda sebességét 5%-kal! 
                /// Állítsuk be a ballNextPosition Top propertyjét 0-ra (felfelé akarunk mozogni), a Left propertyjét pedig az eddigi pozícióhoz képest toljuk el pl. 200-zal, mert jobb felé is szeretnénk haladni.
            /// – A labda a tetonek ütközik: a ballNextPosition Top propertyje kapja értékül az ablak magasságát (az ablak alja felé kezdjen mozogni).
            /// – A labda az ablak bal oldalának ütközik: a ballNextPosition Left propertyje kapja értékül az ablak szélességét(jobbra kezdjen mozogni). 
            /// – A labda az ablak jobb oldalának ütközik: a ballNextPosition Left legyen 0 (balra kezdjen mozogni). 
            /// – A labda túlmegy az üton: dobjunk fel egy MessageBox-ot(Show) a “Játék vége” üzenettel, és írjuk ki, hogy hány másodpercig tartott a játék, majd állítsuk meg a játékot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BallLayoutUpdated(object sender, EventArgs e)
        {
            if (isStarted)
            {

            }
        }
        private void Navigate(object sender, KeyEventArgs e)
        {
            padCurrentPosition = rectanglePad.Margin;
            // **TODO** mit jelent, hogy az üto az ablak bal szélétol távolabb van
            if (e.Key == Key.Left && padCurrentPosition.Left >= 10)
            {
                AnimatePad(-100);
            }
            // **TODO** mit jelent, hogy az ütő nincs a jobb szélen
            if (e.Key == Key.Right && padCurrentPosition.Right >= 10)
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
            startTime = DateTime.Now;
            ballCurrentPosition = ballStartPosition;
            // **TODO** mit jelent, hogy a kezdoiránynak pedig adjunk meg véletlenszeru értékeket? Mi a kezdőirány? 
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
                To = new Thickness(rectanglePad.Margin.Left + diff, rectanglePad.Margin.Right + diff, 0, 0),
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
