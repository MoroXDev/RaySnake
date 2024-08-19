public abstract class State
{
  public abstract void Update();

  public abstract void Draw();

  public virtual void Unload() {}
}