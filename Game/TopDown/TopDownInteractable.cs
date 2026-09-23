using Godot;

public partial class TopDownInteractable : Node2D
{
    public enum InteractionKind { Message, Checkpoint, Exit }
    [Export] public InteractionKind Kind;
    [Export] public string Prompt = "Read sign";
    [Export(PropertyHint.MultilineText)] public string Message = "Welcome to the top-down starter.";
    [Export] public string CheckpointId = "";
    [Export] public Marker2D RespawnPoint;
    [Export] public Color Tint = new("#86cbbb");
    public override void _Draw()
    {
        DrawRect(new Rect2(-13, -3, 26, 6), new Color("#203b3d"));
        DrawRect(new Rect2(-10, -25, 20, 25), Tint);
        DrawRect(new Rect2(-6, -20, 12, 3), new Color("#203442"));
        DrawRect(new Rect2(-6, -14, 8, 3), new Color("#203442"));
    }
}
