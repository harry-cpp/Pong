using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Pong
{
    public class Edge : PhysicsObject
    {
        private bool _drawEdge;
        private Rectangle _drawRectangle;

        public Edge(float x1, float y1, float x2, float y2, PhysicsObject ballbody)
                : base(CreateEdge(x1, y1, x2, y2, ballbody))
        {
            Body.BodyType = BodyType.Static;

            _drawEdge = (ballbody != null);
            if (_drawEdge)
            {
                _drawRectangle = new Rectangle(
                    (int)(x1 - 2),
                    (int)(y1 - 2),
                    (int)(x2 - x1 + 4),
                    (int)(y2 - y1 + 4)
                );
            }
        }

        private static Body CreateEdge(float x1, float y1, float x2, float y2, PhysicsObject ballbody = null)
        {
            var ret = BodyFactory.CreateEdge(
                TheGame.MainWorld,
                new Vector2(ConvertUnits.ToSimUnits(x1), ConvertUnits.ToSimUnits(y1)),
                new Vector2(ConvertUnits.ToSimUnits(x2), ConvertUnits.ToSimUnits(y2))
            );
            ret.Restitution = 1f;
            ret.Friction = 0f;

            if (ballbody != null)
            {
                ret.OnCollision += (fixtureA, fixtureB, contact) =>
                {
                    if (fixtureB.FixtureId == ballbody.Body.BodyId)
                        return false;

                    return true;
                };
            }

            return ret;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!_drawEdge)
                return;

            spriteBatch.Draw(GameContent.Texture.Pixel, _drawRectangle, null, Color.Red);
        }
    }
}
