using Raylib_cs;
using System.Diagnostics;
using System.Numerics;

public class Game : State
{
   private Stopwatch movementTimer = new Stopwatch();
   private RenderTexture2D screen = Raylib.LoadRenderTexture(32, 18);
   private List<Pixel> snake = new List<Pixel>();
   private Direction snakeDirection = Direction.Right;
   private Pixel apple = new Pixel();
   private bool isSnakeMoved = false;
   private bool gameOver = false;
   private bool unloaded = false;
   private int score = 0;
   private Font pixelFont = Raylib.LoadFont("Assets/PixelFont.ttf");
   private Stopwatch startAndGameOverTimer = new Stopwatch();
   private int startCounter = 5;
   private bool gameOverTimerRestarted = false;

   public override void Update()
   {
      if (startCounter < 0)
      {

         if (gameOver != true)
         {
            //COLLISION APPLE AND SNAKE
            if ((int)snake[0].position.X == (int)apple.position.X && (int)snake[0].position.Y == (int)apple.position.Y)
            {
               Pixel snakeElement = new Pixel();
               snakeElement.color = Color.Violet;
               snakeElement.position = snake[0].position;

               snake.Add(snakeElement);

               apple.position = GenerateApplePos(snake, screen.Texture.Width, screen.Texture.Height);
               score++;

            }
            //COLLISION APPLE AND SNAKE

            //MOVING SNAKE
            if (movementTimer.ElapsedMilliseconds > 200)
            {
               for (int i = snake.Count() - 1; i >= 0; i--)
               {
                  if (i == 0)
                  {
                     switch (snakeDirection)
                     {
                        case Direction.Up: { snake[i].position = snake[i].position + new Vector2(0, -1); break; }
                        case Direction.Down: { snake[i].position = snake[i].position + new Vector2(0, 1); break; }
                        case Direction.Right: { snake[i].position = snake[i].position + new Vector2(1, 0); break; }
                        case Direction.Left: { snake[i].position = snake[i].position + new Vector2(-1, 0); break; }
                        default: { break; }
                     }
                  }
                  else
                  {
                     snake[i].position = snake[i - 1].position;
                  }
               }
               movementTimer.Restart();
               isSnakeMoved = true;
            }
            //MOVING SNAKE

            //CHANGING SNAKE DIRECTION
            if (Raylib.IsKeyPressed(KeyboardKey.W))
            {
               snakeDirection = Direction.Up;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.S))
            {
               snakeDirection = Direction.Down;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.A))
            {
               snakeDirection = Direction.Left;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.D))
            {
               snakeDirection = Direction.Right;
            }
            //CHANGING SNAKE DIRECTION

            //COLLISION HEAD AND BODY
            if (isSnakeMoved)
            {
               for (int i = 1; i < snake.Count; i++)
               {
                  if (Equals((int)snake[0].position.X, (int)snake[i].position.X) && Equals((int)snake[0].position.Y, (int)snake[i].position.Y))
                  {
                     gameOver = true;
                  }
               }
            }
            //COLLISION HEAD AND BODY

            //HEAD POS OUT OF BOUNDS
            if ((int)snake[0].position.X >= screen.Texture.Width || (int)snake[0].position.X < 0 || (int)snake[0].position.Y >= screen.Texture.Height || (int)snake[0].position.Y < 0)
            {
               gameOver = true;
            }
            //HEAD POS OUT OF BOUNDS

            //RESETING VALUES
            isSnakeMoved = false;
            //RESETING VALUES
         }
         else 
         {
            //CHECK GAMEOVER
            if (gameOverTimerRestarted == false)
            {
               startAndGameOverTimer.Restart();
               gameOverTimerRestarted = true;
            }
            else if (startAndGameOverTimer.Elapsed.Seconds > 2)
            {
             Unload();
             Program.states.Add(new GameOver());
            }
            //CHECK GAMEOVER
         }
      }
      else
      {
         if (startAndGameOverTimer.Elapsed.Seconds > 0.5)
         {
            startCounter--;
            startAndGameOverTimer.Restart();
         }
      }
   }

   public override void Draw()
   {
      Raylib.BeginTextureMode(screen);
      Raylib.ClearBackground(Color.Lime);
      //DRAW SCREEN 

      //DRAWING APPLE
      Raylib.DrawPixel((int)apple.position.X, (int)apple.position.Y, apple.color);

      //DRAWING SNAKE
      for (int i = snake.Count() - 1; i >= 0; i--)
      {
         Raylib.DrawPixel((int)snake[i].position.X, (int)snake[i].position.Y, snake[i].color);
      }

      //DRAW SCREEN
      Raylib.EndTextureMode();

      Raylib.BeginDrawing();
      Raylib.ClearBackground(Color.Black);

      //DRAWING CREATED SCREEN
      Raylib.DrawTexturePro(screen.Texture, new Rectangle(0, 0, screen.Texture.Width, -screen.Texture.Height), new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()), new Vector2(0, 0), 0, Color.White);

      Raylib.DrawTextEx(pixelFont, "SCORE:" + score, new Vector2(20, 20), Raylib.GetScreenHeight() / 40, 0, Color.White);

      if (startCounter >= 0)
      {
         Raylib.DrawTextEx(pixelFont, startCounter.ToString(), new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2), Raylib.GetScreenHeight() / 20, 0, Color.Black);

      }

      Raylib.EndDrawing();
   }

   public Vector2 GenerateApplePos(List<Pixel> snake, int widthMaxValue, int heightMaxValue)
   {
      bool found = false;
      Random random = new Random();
      Vector2 applePosition = new Vector2(0, 0);

      while (found == false)
      {
         applePosition = new Vector2(random.Next(0, widthMaxValue), random.Next(0, heightMaxValue));
         for (int i = 0; i < snake.Count; i++)
         {
            if (snake[i].position == applePosition)
            {
               break;
            }
            else if (i == snake.Count - 1 && snake[i].position != applePosition)
            {
               found = true;
            }
         }
      }

      return applePosition;
   }

   enum Direction
   {
      Up,
      Down,
      Left,
      Right
   }


   public Game()
   {
      for (int i = Program.states.Count() - 2; i >= 0; i--)
      {
         Program.states[i].Unload();
         Program.states.RemoveAt(i);
      }
      movementTimer.Start();
      startAndGameOverTimer.Start();
      snake.Add(new Pixel(new Vector2(screen.Texture.Width / 2, screen.Texture.Height / 2), Color.DarkPurple));
      apple.color = Color.Red;
      apple.position = GenerateApplePos(snake, screen.Texture.Width, screen.Texture.Height);
   }

   public override void Unload()
   {
      if (unloaded == false)
      {
         Raylib.UnloadRenderTexture(screen);
         Raylib.UnloadFont(pixelFont);
         unloaded = true;
      }
   }
}