using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics.Dynamics;

namespace Pong
{
    public class TheGame : Game
    {
        public static World MainWorld;

        private RenderTarget2D _renderTargetGame, _renderTargetUi;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<GameObject> _objects;
        private Ball _ball;
        private Score _score;
        private string _textStart, _textContinue;
        private Vector2 _positionStart, _positionContinue;
        private Mode _mode;

        public TheGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.HardwareModeSwitch = false;
            _graphics.PreferredBackBufferWidth = GameSettings.Width;
            _graphics.PreferredBackBufferHeight = GameSettings.Height;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            Window.Title = "Pong";

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _renderTargetUi = new RenderTarget2D(GraphicsDevice, GameSettings.Width, GameSettings.Height);
            _renderTargetGame = new RenderTarget2D(GraphicsDevice, GameSettings.Width, GameSettings.Height);

            LoadMe();
        }

        private async void LoadMe()
        {
            await GameContent.Load(Content, GraphicsDevice);

            _mode = Mode.Start;
            _objects = new List<GameObject>();
            MainWorld = new World(new Vector2(0, 0));

            _objects.Add(new Paddle(new Vector2(50, GameSettings.Height / 2f), Keys.S, Keys.W));
            _objects.Add(new Paddle(new Vector2(GameSettings.Width - 50, GameSettings.Height / 2f), Keys.Down, Keys.Up));

            _score = new Score();
            _objects.Add(_score);

            _ball = new Ball();
            _objects.Add(_ball);

            // top edge
            _objects.Add(new Edge(0, 0, GameSettings.Width, 0, null));

            // left edge
            _objects.Add(new Edge(0, 0, 0, GameSettings.Height, _ball));

            // right edge
            _objects.Add(new Edge(GameSettings.Width, 0, GameSettings.Width, GameSettings.Height, _ball));

            // bottom edge
            _objects.Add(new Edge(0, GameSettings.Height, GameSettings.Width, GameSettings.Height, null));

            _textStart = "Press Space to Start";
            var _textStartSize = GameContent.Font.Default.MeasureString(_textStart);
            _positionStart = new Vector2 (
                GameSettings.Width / 2f - _textStartSize.X / 2f,
                GameSettings.Height - _textStartSize.Y - 10f
            );

            _textContinue = "Press Space to Continue";
            var _textContinueSize = GameContent.Font.Default.MeasureString(_textContinue);
            _positionContinue = new Vector2 (
                GameSettings.Width / 2f - _textContinueSize.X / 2f,
                GameSettings.Height - _textContinueSize.Y - 10f
            );

            GameContent.IsLoaded = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (!GameContent.IsLoaded)
            {
                return;
            }

            InputManager.Update();

            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();

            MainWorld.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            foreach (var obj in _objects)
                obj.Update(gameTime);

            if (InputManager.IsKeyPressed(Keys.F))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();

                IsMouseVisible = !_graphics.IsFullScreen;
            }

            if (InputManager.IsKeyPressed(Keys.Space) || 
                InputManager.IsButtonPressed(0, Buttons.A) || 
                InputManager.IsButtonPressed(0, Buttons.Start) || 
                InputManager.IsButtonPressed(1, Buttons.A) || 
                InputManager.IsButtonPressed(1, Buttons.Start))
            {
                if (_mode == Mode.Start || _mode == Mode.NextStart)
                {
                    float forcex = (new Random()).Next(-20, 20);
                    float forcey = (new Random()).Next(-40, 40);

                    forcex = (forcex < 0) ? forcex - 20f : forcex + 20f;

                    _ball.Body.ApplyForce(new Vector2(forcex, forcey));

                    _score.Scored = false;
                    _mode = Mode.Game;
                }
                else if (_mode == Mode.Continue)
                {
                    foreach (var obj in _objects)
                        obj.ResetPosition();
                    
                    _mode = Mode.NextStart;
                }
            }

            if (!_score.Scored)
            {
                if (_ball.Position.X < 0)
                {
                    _score.ScoreForLeft();
                    _mode = Mode.Continue;
                }
                else if (_ball.Position.X > GameSettings.Width)
                {
                    _score.ScoreForRight();
                    _mode = Mode.Continue;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!GameContent.IsLoaded)
            {
                _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
                return;
            }

            _graphics.GraphicsDevice.SetRenderTarget(_renderTargetGame);
            _graphics.GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin();

            foreach (var obj in _objects)
                if (!obj.IsUILayer)
                    obj.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            _graphics.GraphicsDevice.SetRenderTarget(_renderTargetUi);
            _graphics.GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin();

            foreach (var obj in _objects)
                if (obj.IsUILayer)
                    obj.Draw(gameTime, _spriteBatch);
            
            if (_mode == Mode.Start)
                _spriteBatch.DrawString(GameContent.Font.Default, _textStart, _positionStart, Color.White);
            else if (_mode == Mode.Continue)
                _spriteBatch.DrawString(GameContent.Font.Default, _textContinue, _positionContinue, Color.White);

            _spriteBatch.End();

            _graphics.GraphicsDevice.SetRenderTarget(null);
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(_renderTargetGame, GraphicsDevice.PresentationParameters.Bounds, Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTargetUi, GraphicsDevice.PresentationParameters.Bounds, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
