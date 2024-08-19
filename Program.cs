using Raylib_cs;
using static Code;

public class Program
{
   public static List<State> states = new List<State>();
   public static void Main()
   {
      Raylib.SetConfigFlags(ConfigFlags.VSyncHint | ConfigFlags.ResizableWindow);
      Raylib.InitWindow(500, 500, "Snake");
      Raylib.SetWindowSize((int)(Raylib.GetMonitorWidth(0) * 0.75f), (int)(Raylib.GetMonitorHeight(0) * 0.75f));
      CenterWindow();
      Image icon = Raylib.LoadImage("Assets/SnakeIcon.png");
      Raylib.SetWindowIcon(icon);
      
      states.Add(new Game());
      
      while (Raylib.WindowShouldClose() == false && states.Count > 0)
      {
         states.Last().Update();

         states.Last().Draw();
      }
      for (int i = 0; i < states.Count(); i++)
      {
         states[i].Unload();
         states.RemoveAt(i);
      }
      Raylib.UnloadImage(icon);
      
      Raylib.CloseWindow();
   }
}
