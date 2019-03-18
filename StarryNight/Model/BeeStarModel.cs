using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace StarryNight.Model
{
    class BeeStarModel
    {
        public static readonly Size StarSize = new Size(150, 100);

        private readonly Dictionary<Bee, Point> _bees = new Dictionary<Bee, Point>();

        private readonly Dictionary<Star, Point> _stars = new Dictionary<Star, Point>();

        private Random _random = new Random();

        public BeeStarModel()
        {
            _playAreaSize = Size.Empty;
        }

        public void Update()
        {
            MoveOneBee();
            AddOrRemoveAStar();
        }

        private static bool RectsOverlap(Rect r1, Rect r2)
        {
            r1.Intersect(r2);
            if (r1.Width > 0 || r1.Height > 0)
                return true;
            return false;
        }

        private Size _playAreaSize;

        public Size PlayAreaSize
        {
            get { return _playAreaSize;}
            // Add a backing field, and have the set accessor call CreateBees() and CreateStars()
            set
            {
                _playAreaSize = value;
                CreateBees();
                CreateStars();
            }
        }

        private void CreateBees()
        {
            // If the play area is empty, return. If there are already bees, move each of them.
            // Otherwise, create between 5 and 15 randomly sized bees (40 to 150 pixels), add
            // it to the _bees collection, and fire the BeeMoved event.

            if (PlayAreaSize.IsEmpty) return;
            if (_bees.Count > 0)
            {
                List<Bee> bees = _bees.Keys.ToList();

                foreach (Bee bee in bees)
                {
                    MoveOneBee(bee);
                }
            }
            else
            {
                int numberOfBees = _random.Next(5, 10);                

                for (int i = 0; i < numberOfBees; i++)
                {
                    int s = _random.Next(50, 100);
                    Size beeSize = new Size(s, s);
                    Point beeLocation = FindNonOverlappingPoint(beeSize);

                    Bee bee = new Bee(beeLocation,beeSize);
                    _bees.Add(bee,new Point(beeLocation.X, beeLocation.Y));
                    OnBeeMoved(bee, beeLocation.X, beeLocation.Y);
                }
            }
        }        

        private void CreateStars()
        {
            // If the play area is empty, return. If there are already stars,
            // set each star's location to a new point and fire the StarChanged
            // event, otherwise call CreateAStar() between 5 and 10 times.

            if (PlayAreaSize.IsEmpty) return;
            if (_stars.Count >0 )
            {                
                foreach (Star star in _stars.Keys)
                {
                    star.Location = FindNonOverlappingPoint(StarSize);
                    OnStarChanged(star, false);
                }
            }
            else
            {
                int starCount = _random.Next(5, 10);
                for (int i = 0; i < starCount; i++)
                {
                    CreateAStar();
                }
            }            
        }

        

        private void CreateAStar()
        {
            // Find a new non-overlapping point, add a new Star object to the
            // _stars collection, and fire the StarChanged event.

            bool isRotating = false;
            if (_random.Next(2) == 0)
                isRotating = true;

            Point starLocation = FindNonOverlappingPoint(StarSize);
            Star star = new Star(starLocation,isRotating);

            _stars[star] = starLocation;
            OnStarChanged(star, false);
        }
        private Point FindNonOverlappingPoint(Size size)
        {
            // Find the upper-left corner of a rectangle that doesn't overlap any bees or stars.
            // You'll need to try random Rects, then use LINQ queries to find any bees or stars
            // that overlap (the RectsOverlap() method will be useful).

            Rect randomRect = new Rect();
            bool noOverlap = false;
            int count = 0;

            while (!noOverlap)
            {
                randomRect = new Rect(_random.Next((int)_playAreaSize.Width - 150), _random.Next((int)_playAreaSize.Height - 150),size.Width,size.Height);

                var overlapingBees =
                    from bee in _bees.Keys
                    where RectsOverlap(bee.Position, randomRect)
                    select bee;

                var overlapingStars =
                    from star in _stars.Keys
                    where RectsOverlap(new Rect(star.Location.X, star.Location.Y, StarSize.Width, StarSize.Height), randomRect)
                    select star;

                if ((overlapingBees.Count() + overlapingStars.Count() == 0) || (count++ > 1000))
                {
                    noOverlap = true;
                }
            }            
            return new Point(randomRect.X, randomRect.Y);



        }
        private void MoveOneBee(Bee bee = null)
        {
            // If there are no bees, return. If the bee parameter is null, choose a random bee,
            // otherwise use the bee argument. Then find a new non-overlapping point, update the bee's
            // location, update the _bees collection, and then fire the OnBeeMoved event.

            if (_bees.Keys.Count() == 0) return;
            if (bee == null)
            {
                bee = _bees.Keys.ToList()[_random.Next(_bees.Keys.Count())];
            }
            bee.Location = FindNonOverlappingPoint(bee.Size);
            _bees[bee] = bee.Location;

            OnBeeMoved(bee, bee.Location.X, bee.Location.Y);

        }
        private void AddOrRemoveAStar()
        {
            // Flip a coin (_random.Next(2) == 0) and either create a star using CreateAStar() or
            // remove a star and fire OnStarChanged. Always create a star if there are <= 5, remove
            // one if >= 20. _stars.Keys.ToList()[_random.Next(_stars.Count)] will find a random star.


            //if (((_random.Next(2) == 0) || (_stars.Count <= 5)) && (_stars.Count < 20))
            //    CreateAStar();
            //else
            //{
            //    Star starToRemove = _stars.Keys.ToList()[_random.Next(_stars.Count)];
            //    _stars.Remove(starToRemove);
            //    OnStarChanged(starToRemove, true);
            //}



            if (_stars.Keys.Count() <= 5)
                CreateAStar();

            if (_stars.Keys.Count() >= 20)
            {
                Star starToRemove = _stars.Keys.ToList()[_random.Next(_stars.Count)];
                _stars.Remove(starToRemove);
                OnStarChanged(starToRemove, true);
            }

            else
            {
                int coinFlip = _random.Next(2);

                if (coinFlip == 0)
                    CreateAStar();
                if (coinFlip == 1)
                {
                    Star starToRemove = _stars.Keys.ToList()[_random.Next(_stars.Count)];
                    _stars.Remove(starToRemove);
                    OnStarChanged(starToRemove, true);
                }
            }

        }

        // You'll need to add the BeeMoved and StarChanged events and methods to call them.
        // They use the BeeMovedEventArgs and StarChangedEventArgs classes.

        public event EventHandler<BeeMovedEventArgs> BeeMoved;

        private void OnBeeMoved(Bee beeThatMoved, double x, double y)
        {
            EventHandler<BeeMovedEventArgs> beeMoved = BeeMoved;
            if (beeMoved != null)
            {
                beeMoved(this, new BeeMovedEventArgs(beeThatMoved, x, y));
            }
        }

        public event EventHandler<StarChangedEventArgs> StarChanged;

        private void OnStarChanged(Star starThatChanged, bool removed)
        {
            EventHandler<StarChangedEventArgs> starChanged = StarChanged;
            if (starChanged != null)
            {
                starChanged(this, new StarChangedEventArgs(starThatChanged, removed));
            }
        }
    }

}

