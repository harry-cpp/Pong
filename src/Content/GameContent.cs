using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public static class GameContent
    {
        public static void Load(ContentManager content, GraphicsDevice graphics)
        {
            var colordata = new Color[1];
            colordata[0] = Color.White;
            Texture.Pixel = new Texture2D(graphics, 1, 1);
            Texture.Pixel.SetData<Color>(colordata);

            Texture.Ball = content.Load<Texture2D>("Textures/Ball");
            Texture.Pad = content.Load<Texture2D>("Textures/Paddle");

            Font.Default = content.Load<SpriteFont>("Fonts/Default");
        }

        public static class Texture
        {
            public static Texture2D Ball;
            public static Texture2D Pad;
            public static Texture2D Pixel;
        }

        public static class Font
        {
            public static SpriteFont Default;
        }
    }
}