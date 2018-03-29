using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Pong
{
    public class PhysicsObject : GameObject
    {
        protected Body _body;
        protected Texture2D _sprite;
        protected Vector2 _origin;
        protected Vector2? _startPosition;

        public PhysicsObject(Body body)
        {
            _body = body;
            _body.BodyType = BodyType.Dynamic;
            _body.Restitution = 1f;
            _body.Friction = 0f;
        }

        public PhysicsObject(Texture2D sprite)
        {
            _body = BodyFactory.CreateRectangle(
                TheGame.MainWorld,
                ConvertUnits.ToSimUnits(sprite.Width),
                ConvertUnits.ToSimUnits(sprite.Height),
                1f
            );
            _body.BodyType = BodyType.Dynamic;
            _body.Restitution = 1f;
            _body.Friction = 0f;

            _origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
            _sprite = sprite;
        }

        public Body Body => _body;

        public Vector2 Origin
        {
            get => _origin;
            set => _origin = value;
        }
        
        public Vector2 Position
        {
            get => ConvertUnits.ToDisplayUnits(Body.Position);
            set => Body.Position = ConvertUnits.ToSimUnits(value);
        }

        public override void ResetPosition()
        {
            if (_startPosition.HasValue)
                Position = _startPosition.Value;
            
            Body.Rotation = 0f;
            Body.ResetDynamics();
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_sprite == null)
                return;

            spriteBatch.Draw(
                _sprite,
                Position,
                null,
                Color.White,
                Body.Rotation,
                _origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}