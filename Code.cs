using Raylib_cs;

public class Code
{
  public static void ChangeWindowMode()
  {
    if (Raylib.IsWindowFullscreen())
    {
      Raylib.ToggleFullscreen();
      Raylib.SetWindowSize((int)(Raylib.GetMonitorWidth(0) * 0.75f), (int)(Raylib.GetMonitorHeight(0) * 0.75f));
    }
    else
    {
      Raylib.SetWindowSize(Raylib.GetMonitorWidth(0), Raylib.GetMonitorHeight(0));
      Raylib.ToggleFullscreen();
    }
  }

  public static void CenterWindow()
  {
    Raylib.SetWindowPosition(Raylib.GetMonitorWidth(0) / 2 - Raylib.GetScreenWidth() / 2, Raylib.GetMonitorHeight(0) / 2 - Raylib.GetScreenHeight() / 2);
  }

  public static int FULLHD_WIDTH = 1920, FULLHDHEIGHT = 1080;
}