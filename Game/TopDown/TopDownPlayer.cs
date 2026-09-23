using Godot;
using CoreUI;

public partial class TopDownPlayer : CharacterBody2D
{
    [Export] public float Speed = 120;
    [Export] public StringName MoveLeft = "move_left";
    [Export] public StringName MoveRight = "move_right";
    [Export] public StringName MoveUp = "move_up";
    [Export] public StringName MoveDown = "move_down";
    [Signal] public delegate void MotionChangedEventHandler(Vector2 facing, bool moving);
    public Vector2 Facing { get; private set; } = Vector2.Down;
    public bool Moving { get; private set; }
    public bool GameplayBlocked => GetTree().Paused || MenuManager.Instance.ActiveMenu != null || SceneFlowManager.Instance.IsTransitioning;
    public override void _PhysicsProcess(double delta)
    {
        var direction = GameplayBlocked ? Vector2.Zero : Input.GetVector(MoveLeft, MoveRight, MoveUp, MoveDown);
        Velocity = direction * Speed;
        MoveAndSlide();
        var facing = direction.IsZeroApprox() ? Facing : direction.Normalized();
        bool moving = !Velocity.IsZeroApprox();
        if (facing != Facing || moving != Moving)
        {
            Facing = facing;
            Moving = moving;
            EmitSignal(SignalName.MotionChanged, Facing, Moving);
            QueueRedraw();
        }
    }
    public override void _Draw()
    {
        // Deliberately simple pixel placeholder. Replace with assigned visuals/animation.
        DrawRect(new Rect2(-8, -3, 16, 6), new Color("#182e30"));
        DrawRect(new Rect2(-6, -17, 12, 17), new Color("#f3cd79"));
        DrawRect(new Rect2(-6, -9, 12, 7), new Color("#52bdb1"));
        var look = new Vector2(Mathf.Round(Facing.X * 3), Mathf.Round(Facing.Y * 2));
        DrawRect(new Rect2(new Vector2(-3, -14) + look, new Vector2(2, 2)), new Color("#203442"));
        DrawRect(new Rect2(new Vector2(1, -14) + look, new Vector2(2, 2)), new Color("#203442"));
    }
}
