using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace Pong
{
    public class Ball : PhysicsObject
    {
        public Ball() : base(GameContent.Texture.Ball)
        {
            Position = new Vector2(GameSettings.Width / 2f, GameSettings.Height / 2f);
            _startPosition = Position;
        }
    }
}
