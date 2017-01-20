using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Demo.Scenes;

namespace Demo
{
    public class DialogBox  : SceneManager
    {
        public string Text { get; set; }
        public bool Active { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }
        public Color DialogColor { get; set; }
        public int BorderWidth { get; set; }
        private readonly Texture2D _fillTexture;
 //       private readonly Texture2D _borderTexture;
        private List<string> _pages;
        private const float DialogBoxMargin = 10f;
        private Vector2 _characterSize = Game1.font.MeasureString(new StringBuilder("q", 5));
        //        private int MaxCharsPerLine => (int)Math.Floor((Size.X - DialogBoxMargin) / _characterSize.X);
        //        private int MaxLines => (int)Math.Floor((Size.Y - DialogBoxMargin) / _characterSize.Y) - 1;
        private int MaxCharsPerLine => 40;
        private int MaxLines => 4;
        private int _currentPage;
        private int _interval;
        private Rectangle TextRectangle => new Rectangle(Position.ToPoint(), Size.ToPoint());

        private List<Rectangle> BorderRectangles => new List<Rectangle>
        {
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y - BorderWidth,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            new Rectangle(TextRectangle.X + TextRectangle.Size.X, TextRectangle.Y, BorderWidth, TextRectangle.Height),

            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y + TextRectangle.Size.Y,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y, BorderWidth, TextRectangle.Height)
        };

        private Vector2 TextPosition => new Vector2(Position.X + DialogBoxMargin / 2, Position.Y + DialogBoxMargin / 2);

        private Stopwatch _stopwatch;

        public DialogBox(Game game) : base(game)
        {
            BorderWidth = 1;
            DialogColor = Color.White;

            FillColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

            BorderColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

            //  _fillTexture = new Texture2D(GraphicsDevice, 1, 1);
            //   _fillTexture.SetData(new[] { FillColor });
            _fillTexture = Content.Load<Texture2D>(@"interface\menu");

 //           _borderTexture = new Texture2D(GraphicsDevice, 1, 1);
 //           _borderTexture.SetData(new[] { BorderColor });

            _pages = new List<string>();
            _currentPage = -1;

            var sizeX = (int)(200);
            var sizeY = (int)(50);

            Size = new Vector2(sizeX, sizeY);

            var posX = Village.player.Position.X - 100;
            var posY = Village.player.Position.Y;

            Position = new Vector2(posX, posY);
        }

        public void Initialize(string text = null)
        {
            Text = text ?? Text;

            _currentPage = -1;

            Show();
        }

        public override void Show()
        {
            Active = true;

            _stopwatch = new Stopwatch();

            _stopwatch.Start();

            _pages = WordWrap(Text);
        }

        public override void Hide()
        {
            Active = false;

            _stopwatch.Stop();

            _stopwatch = null;
        }

        public void Update()
        {
            if (Active)
            {
                if ((Village.newState.IsKeyDown(Keys.Enter) && Village.oldState.IsKeyUp(Keys.Enter)))
                {
                    if (_currentPage >= _pages.Count - 1)
                    {
                        Hide();
                        Village.inDialog = false;
                    }
                    else
                    {
                        _currentPage++;
                        _stopwatch.Restart();
                    }
                }
                if ((Village.newState.IsKeyDown(Keys.X) && Village.oldState.IsKeyUp(Keys.X)))
                {
                    Hide();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                foreach (var side in BorderRectangles)
                {
               // spriteBatch.Draw(_borderTexture, null, side);
                }
                spriteBatch.Draw(_fillTexture, null, TextRectangle);

                spriteBatch.DrawString(Game1.font, _pages[_currentPage], TextPosition, DialogColor);

                if (BlinkIndicator() || _currentPage == _pages.Count - 1)
                {
                    var indicatorPosition = new Vector2(TextRectangle.X + TextRectangle.Width - (_characterSize.X) - 4,
                        TextRectangle.Y + TextRectangle.Height - (_characterSize.Y));

                    spriteBatch.DrawString(Game1.font, "", indicatorPosition, Color.Red);
                }
            }
        }

        private bool BlinkIndicator()
        {
            _interval = (int)Math.Floor((double)(_stopwatch.ElapsedMilliseconds % 1000));

            return _interval < 500;
        }

        private List<string> WordWrap(string text)
        {
            var pages = new List<string>();

            var capacity = MaxCharsPerLine * MaxLines > text.Length ? text.Length : MaxCharsPerLine * MaxLines;

            var result = new StringBuilder(capacity);
            var resultLines = 0;

            var currentWord = new StringBuilder();
            var currentLine = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var isNewLine = text[i] == '\n';
                var isLastChar = i == text.Length - 1;


                    currentWord.Append(currentChar);

                if (char.IsWhiteSpace(currentChar) || isLastChar)
                {
                    var potentialLength = currentLine.Length + currentWord.Length;

                    if (potentialLength > MaxCharsPerLine)
                    {
                        result.AppendLine(currentLine.ToString());

                        currentLine.Clear();

                        resultLines++;
                    }

                    currentLine.Append(currentWord);

                    currentWord.Clear();

                    if (isLastChar || isNewLine)
                    {
                        result.AppendLine(currentLine.ToString());
                    }

                    if (resultLines > MaxLines || isLastChar || isNewLine)
                    {
                        pages.Add(result.ToString());

                        result.Clear();

                        resultLines = 0;

                        if (isNewLine)
                        {
                            currentLine.Clear();
                        }
                    }
                }
            }

            return pages;
        }
    }
}
