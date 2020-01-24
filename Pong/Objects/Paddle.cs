using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Dynamics;

namespace Pong
{
    public class Paddle : PhysicsObject
    {
        private Keys _keyUp, _keyDown;
        private float _originX, _originRotation;
        private int _index;

        public Paddle(Vector2 position, Keys down, Keys up) : base(GameContent.Texture.Pad)
        {
            Position = position;
            _startPosition = position;

            _originX = ConvertUnits.ToSimUnits(position.X);
            _originRotation = Body.Rotation;
            _keyDown = down;
            _keyUp = up;
            _index = (down == Keys.Down) ? 1 : 0;
        }

        public override void Update(GameTime gameTime)
        {
            // Player controls
            // TODO: Optimize

            if (InputManager.IsKeyDown(_keyUp) || 
                InputManager.GStates[_index].DPad.Up == ButtonState.Pressed)
                Body.ApplyForce(new Vector2(0, -10f));

            if (InputManager.IsKeyDown(_keyDown) || 
                InputManager.GStates[_index].DPad.Down == ButtonState.Pressed)
                Body.ApplyForce(new Vector2(0, 10f));

            Body.ApplyForce(new Vector2(0, InputManager.GStates[_index].ThumbSticks.Left.Y * -10f));

            // Restore rotation and X position... slowly...

            if (Body.Rotation > _originRotation)
                Body.ApplyTorque(-0.3f);
            else if (Body.Rotation < _originRotation)
                Body.ApplyTorque(0.3f);

            if (Body.Position.X > _originX)
                Body.ApplyForce(new Vector2(-0.3f, 0));
            else if (Body.Position.X < _originX)
                Body.ApplyForce(new Vector2(0.3f, 0));
        }
    }
}
