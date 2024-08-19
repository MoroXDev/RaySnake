using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Raylib_cs;
using static Code;

public class GameOver : State
{
  private RenderTexture2D screen = Raylib.LoadRenderTexture(FULLHD_WIDTH, FULLHDHEIGHT);
  private Texture2D gameOverTexture = Raylib.LoadTexture("Assets/GameOverScreen.png");
  private Font pixelFont = Raylib.LoadFont("Assets/PixelFont.ttf");
  private Vector2 yesTextPosition = new Vector2(FULLHD_WIDTH * 0.4f, FULLHDHEIGHT * 0.75f);
  private Vector2 noTextPosition = new Vector2(FULLHD_WIDTH * (1 - 0.4f), FULLHDHEIGHT * 0.75f);
  private Vector2 selectedPosition;
  private bool unloaded = false;

  public override void Draw()
  {
    Raylib.BeginTextureMode(screen);
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawTextureEx(gameOverTexture, new Vector2((int)(FULLHD_WIDTH * 0.25f), (int)(FULLHDHEIGHT * 0.10f)), 0, 0.5f, Color.White);

    Raylib.DrawTextPro(pixelFont, "Play Again?", new Vector2(FULLHD_WIDTH / 2 - 250, FULLHDHEIGHT / 1.6f), new Vector2(0, 0), 0, 50, 0, Color.White);

    Raylib.DrawTextPro(pixelFont, "YES", yesTextPosition, new Vector2(Raylib.MeasureText("YES", 50) / 2, 50 / 2), 0, 50, 0, Color.White);
    Raylib.DrawTextPro(pixelFont, "NO", noTextPosition, new Vector2(Raylib.MeasureText("NO", 50) / 2, 50 / 2), 0, 50, 0, Color.White);

    Raylib.DrawTextPro(pixelFont, "*", selectedPosition, new Vector2(Raylib.MeasureText("*", 50) / 2, 50 / 2), 0, 50, 0, Color.Yellow);
    Raylib.EndTextureMode();

    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.Black);
    Raylib.DrawTexturePro(screen.Texture, new Rectangle(0, 0, screen.Texture.Width, -screen.Texture.Height), new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()), new Vector2(0, 0), 0, Color.White);
    
    Raylib.EndDrawing();
  }

  public override void Update()
  {
    //CHANGING SELECTED POSITION
    if (Raylib.IsKeyPressed(KeyboardKey.D))
    {
      selectedPosition = new Vector2(noTextPosition.X + Raylib.MeasureText("NO", 50) + 10, noTextPosition.Y - 30);
    }
    else if (Raylib.IsKeyPressed(KeyboardKey.A))
    {
      selectedPosition = new Vector2(yesTextPosition.X + Raylib.MeasureText("YES", 50) + 10, yesTextPosition.Y - 30);
    }
    //CHANGING SELECTED POSITION

    //SELECTING YES OR NO
    if (Raylib.IsKeyPressed(KeyboardKey.Enter))
    {
      if (Vector2.Distance(selectedPosition, yesTextPosition) < 200)
      {
        Unload();
        Program.states.Add(new Game());
      }
      else
      {
        Unload();
        Program.states.Clear();
      }
    }
    //SELECTING YES OR NO
  }

  public override void Unload()
  {
    if (unloaded == false)
    {
    Raylib.UnloadRenderTexture(screen);
    Raylib.UnloadTexture(gameOverTexture);
    Raylib.UnloadFont(pixelFont);
    unloaded = true;
    }
  }

  public GameOver()
  {
    for (int i = Program.states.Count() - 2; i >= 0; i--)
    {
      Program.states[i].Unload();
      Program.states.RemoveAt(i);
    }
    selectedPosition = new Vector2(yesTextPosition.X + Raylib.MeasureText("YES", 50) + 10, yesTextPosition.Y - 30);
  }
}