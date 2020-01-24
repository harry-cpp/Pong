using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public static class GameContent
    {
        public static bool IsLoaded;

        public static async Task Load(ContentManager content, GraphicsDevice graphics)
        {
#if WEB
            Texture.Pixel = await content.LoadAsync<Texture2D>("Textures/Ball");
            Texture.Ball = await content.LoadAsync<Texture2D>("Textures/Ball");
            Texture.Pad = await content.LoadAsync<Texture2D>("Textures/Paddle");
            Font.Default = await content.LoadAsync<SpriteFont>("Fonts/Default");
#else
            await Task.Run(() => {
                Texture.Pixel = content.Load<Texture2D>("Textures/Ball");
                Texture.Ball = content.Load<Texture2D>("Textures/Ball");
                Texture.Pad = content.Load<Texture2D>("Textures/Paddle");
                Font.Default = content.Load<SpriteFont>("Fonts/Default");
            });
#endif
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
