using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;


namespace Game
{

    // This class controls and draws the player object.
    public class Player
    {
        private int _x, _y, _direction;

        public int Score { get; set; }
        public int NewDirection { get; set; }

        // Call this method to set starting position and the direction that the baddie will start going in.
        public Player(int startX, int startY, int startDirection)
        {
            _x = startX;
            _y = startY;
            _direction = startDirection;
        }


        public void Move(Maze gamemaze)
        {
            switch (_direction)
            {
                case 1: _y -= 8; break;
                case 2: _x += 8; break;
                case 4: _y += 8; break;
                case 8: _x -= 8; break;
            }

            // Test to see if the player is in the middle of a square in the maze, and then we can decide
            // to change direction or not depending on whether a key is pressed or if they've run into a wall.
            // It turns out this is a little trickier than you might expect - the player might be trying to 
            // turn in a direction that isn't good just yet, but will be soon - so we need to keep trying without
            // stopping them until it's good to go.

            if ((_x % 64 == 0) && (_y % 64 == 0))
            {
                // Map the screen co-ordinates to the map co-ordinates 
                // (the screen is 1024 by 1024, the maze data is 16 by 16)

                var mx = (_x / 64);
                var my = (_y / 64);

                // Check for a dot being eaten
                if ((gamemaze.GetTile(my, mx) & 16) != 0)
                {
                    gamemaze.SetTile(my, mx, gamemaze.GetTile(my, mx) - 16);
                    Score += 1;
                }


                // Get the number that tells us the possible directions at this tile
                var possibleDirections = gamemaze.GetTile(my, mx);

                var currentDirection = _direction;


                // If the player has pressed a key, let's consider that a request to change direction.
                // We can check if it's possible because we know for any tile what directions are available.
                // If the player cannot change in that direction, they'll just stop.

                if (NewDirection == 0)
                {
                    // The player hasn't requested a change in direction
                    // So let's see if they can move in the current direction

                    if ((_direction & possibleDirections) == 0)
                    {
                        // No, the player would hit a wall. Better stop!
                        _direction = 0;
                    }
                }
                else // The player has requested a change in direction
                {
                    // Is it ok?
                    if ((NewDirection & possibleDirections) != 0)
                    {
                        // yes, it's allowed
                        _direction = NewDirection;
                    }
                    else
                    {
                        // No, player can't change direction in that way, but we can't just stop
                        // because the current direction might be good.  

                        if ((currentDirection & possibleDirections) == 0)
                        {
                            // No, carrying on isn't allowed! Better stop!
                            _direction = 0;
                        }
                    }
                }
            }
        }

        // Draw the player. Needs a reference to the canvas
        public void Draw(CanvasDrawEventArgs args, CanvasBitmap ninjacat)
        {
            args.DrawingSession.DrawImage(ninjacat, _x, _y);
        }

        public bool Check(Baddie badguy)
        {
            // Check the distance between the player and a Baddie
            if ((((_x - badguy.X) * (_x - badguy.X)) < 256) && (((_y - badguy.Y) * (_y - badguy.Y)) < 256))
            {
                return true;
            }

            return false;
        }

    }
}
