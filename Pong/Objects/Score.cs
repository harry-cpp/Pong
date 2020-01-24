using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Score : GameObject
    {
        private int _left, _right;
        private string _leftString, _rightString, _separatorString;
        private Vector2 _leftPosition, _rightPostion, _separatorPosition;

        public Score()
        {
            IsUILayer = true;
            Left = 0;
            Right = 0;
            Scored = true;

            _separatorString = ":";
            var separatorSize = GameContent.Font.Default.MeasureString(_separatorString);
            _separatorPosition = new Vector2((int)(GameSettings.Width / 2f - separatorSize.X / 2f), 10);

            RecalcPositions();
        }

        public int Left
        {
            get => _left;
            set
            {
                _left = value;
                _leftString = value.ToString();
            }
        }

        public int Right
        {
            get => _right;
            set
            {
                _right = value;
                _rightString = value.ToString();
            }
        }

        public bool Scored { get; set; }

        public void ScoreForLeft()
        {
            Left = Left + 1;
            RecalcPositions();
            Scored = true;
        }

        public void ScoreForRight()
        {
            Right = Right + 1;
            RecalcPositions();
            Scored = true;
        }

        public void RecalcPositions()
        {
            var separatorSize = GameContent.Font.Default.MeasureString(_separatorString);

            var leftSize = GameContent.Font.Default.MeasureString(_leftString);
            _leftPosition = new Vector2((int)(GameSettings.Width / 2f - leftSize.X / 2f - separatorSize.X * 2f), 10);

            var rightSize = GameContent.Font.Default.MeasureString(_rightString);
            _rightPostion = new Vector2((int)(GameSettings.Width / 2f - rightSize.X / 2f + separatorSize.X * 2f), 10);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameContent.Font.Default, _separatorString, _separatorPosition, Color.White);
            spriteBatch.DrawString(GameContent.Font.Default, _leftString, _leftPosition, Color.White);
            spriteBatch.DrawString(GameContent.Font.Default, _rightString, _rightPostion, Color.White);
        }
    }
}
