using Godot;
using CoreUI;
using Godot.Collections;

public partial class TopDownLevel : Node2D
{
    [Export] public TopDownPlayer Player;
    [Export] public Camera2D Camera;
    [Export] public Marker2D Spawn;
    [Export] public BaseMenu PauseMenu;
    [Export] public Label PromptLabel;
    [Export] public Label StatusLabel;
    [Export] public Area2D Hazard;
    [Export] public Array<TopDownInteractable> Interactables = new();
    [Export] public float InteractionRange = 42;
    [Export] public StringName InteractAction = "interact";
    [Export] public StringName PauseAction = "ui_cancel";
    [Export] public StringName SaveAction = "save_game";
    private Vector2 _checkpoint;
    private string _checkpointId = "";
    public override void _Ready()
    {
        _checkpoint = Spawn.GlobalPosition;
        Player.GlobalPosition = _checkpoint;
        var save = SaveManager.Instance;
        if (save.GetLastLevelPath() == SceneFilePath && save.GetFlag("topdown.saved"))
        {
            _checkpoint = new Vector2(save.GetStat("topdown.checkpoint_x"), save.GetStat("topdown.checkpoint_y"));
            _checkpointId = save.GetCheckpointId();
            Player.GlobalPosition = new Vector2(save.GetStat("topdown.player_x"), save.GetStat("topdown.player_y"));
            StatusLabel.Text = "Progress restored. Find the blue checkpoint, then the gold exit.";
        }
        Hazard.BodyEntered += OnHazard;
        Camera.ResetSmoothing();
        Camera.ForceUpdateScroll();
    }
    public override void _ExitTree() => Hazard.BodyEntered -= OnHazard;
    public override void _Process(double delta)
    {
        var target = FindInteractable();
        PromptLabel.Text = target == null ? "" : $"E / X: {target.Prompt}";
    }
    public TopDownInteractable FindInteractable()
    {
        if (Player.GameplayBlocked) return null;
        TopDownInteractable nearest = null;
        float distance = InteractionRange;
        foreach (var item in Interactables)
        {
            if (!IsInstanceValid(item)) continue;
            float next = Player.GlobalPosition.DistanceTo(item.GlobalPosition);
            if (next > distance) continue;
            var ray = PhysicsRayQueryParameters2D.Create(Player.GlobalPosition, item.GlobalPosition, 1);
            if (GetWorld2D().DirectSpaceState.IntersectRay(ray).Count != 0) continue;
            nearest = item;
            distance = next;
        }
        return nearest;
    }
    public override void _UnhandledInput(InputEvent input)
    {
        if (input.IsEcho() || Player.GameplayBlocked) return;
        if (input.IsActionPressed(PauseAction)) MenuManager.Instance.OpenMenu(PauseMenu);
        else if (input.IsActionPressed(SaveAction)) SaveProgress();
        else if (input.IsActionPressed(InteractAction)) TryInteract();
        else return;
        GetViewport().SetInputAsHandled();
    }
    public void TryInteract()
    {
        var target = FindInteractable();
        if (target == null) return;
        switch (target.Kind)
        {
            case TopDownInteractable.InteractionKind.Checkpoint:
                _checkpoint = target.RespawnPoint.GlobalPosition;
                _checkpointId = target.CheckpointId;
                if (SaveProgress()) StatusLabel.Text = "Checkpoint saved. Red water returns you here.";
                break;
            case TopDownInteractable.InteractionKind.Exit:
                if (SaveProgress()) SceneFlowManager.Instance.ReturnToMainMenu();
                break;
            default:
                StatusLabel.Text = target.Message;
                break;
        }
    }
    public bool SaveProgress()
    {
        if (Player.GameplayBlocked) return false;
        var save = SaveManager.Instance;
        save.CaptureCurrentScene();
        save.SetFlag("topdown.saved", true);
        save.SetStat("topdown.player_x", Player.GlobalPosition.X);
        save.SetStat("topdown.player_y", Player.GlobalPosition.Y);
        save.SetStat("topdown.checkpoint_x", _checkpoint.X);
        save.SetStat("topdown.checkpoint_y", _checkpoint.Y);
        save.SetCheckpointId(_checkpointId);
        bool ok = save.SaveGame();
        StatusLabel.Text = ok ? "Saved. Continue restores your position and checkpoint." : "Save failed.";
        return ok;
    }
    private void OnHazard(Node2D body) { if (body == Player) Respawn(); }
    public void Respawn()
    {
        Player.GlobalPosition = _checkpoint;
        Player.Velocity = Vector2.Zero;
        Player.ResetPhysicsInterpolation();
        Camera.ResetSmoothing();
        Camera.ForceUpdateScroll();
        StatusLabel.Text = "Back at the checkpoint. Your last disk save is unchanged.";
    }
}
