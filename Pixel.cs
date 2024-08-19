using System.Numerics;
using Raylib_cs;

public class Pixel
{
  public Vector2 position = new Vector2(0, 0);

  public Color color = Color.White;

  public Pixel()
  {

  }

  public Pixel(Vector2 position, Color color)
  {
    this.position = position;
    this.color = color;
  }
}