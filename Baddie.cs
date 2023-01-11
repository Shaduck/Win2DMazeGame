using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;


namespace Game
{

    // This class contains the code that draws and moves the "badguy" image.

    public class Baddie
    {
        public int X;
        public int Y;
        private int _direction;

        // Call this to set starting position and the direction that the baddie will start going in.
        public Baddie(int startX, int startY, int startDirection)
        {
            X = startX;
            Y = startY;
            _direction = startDirection;
        }

        public void Move(int speed, Maze gamemaze)
        {
            // Update the position of the baddie depending on the direction.
            // We can change the speed to make them faster or slower, and so
            // make the game harder or easier.

            switch (_direction)
            {
                case 1: Y -= speed; break;
                case 2: X += speed; break;
                case 4: Y += speed; break;
                case 8: X -= speed; break;
            }

            // Test to see if the baddie is in the middle of a square in the maze, and then we can decide
            // to change direction or not.

            if ((X % 64 != 0) || (Y % 64 != 0)) return;

            // Map the screen co-ordinates to the map co-ordinates 
            // (the screen is 1024 by 1024, the maze data is 16 by 16)

            var mx = (X / 64);
            var my = (Y / 64);

            // Get the number that tells us the possible directions at this tile
            var possibleDirections = gamemaze.GetTile(my, mx);

            // Define the opposite value to the current direction,
            // because we don't want to double back

            var oppositeDirecton = 0;

            switch (_direction)
            {
                case 1:
                    oppositeDirecton = 4;
                    break;
                case 2:
                    oppositeDirecton = 8;
                    break;
                case 4:
                    oppositeDirecton = 1;
                    break;
                case 8:
                    oppositeDirecton = 2;
                    break;
            }

            int newDirection;
            //newDirection = 1;

            // Pick a new direction that is both possible AND not doubling back.
            do
            {
                do
                {
                    newDirection = gamemaze.NextDirection();
                }
                while ((newDirection & possibleDirections) == 0);
            } 
            while (newDirection == oppositeDirecton);

            _direction = newDirection;
        }

        // Draw the baddie. Needs a reference to the canvas
        public void Draw(CanvasDrawEventArgs args, CanvasBitmap dino)
        {
            args.DrawingSession.DrawImage(dino, X + 4, Y + 4);

        }
    }
}
