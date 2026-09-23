using Godot;

public partial class PixelRoom : Node2D
{
    public override void _Draw()
    {
        DrawRect(new Rect2(0, 0, 960, 640), new Color("#243d40"));
        for (int y = 32; y < 608; y += 32)
            for (int x = 32; x < 928; x += 32)
            {
                var color = (x / 32 + y / 32) % 2 == 0 ? new Color("#304b4a") : new Color("#334f4d");
                DrawRect(new Rect2(x, y, 31, 31), color);
                if ((x * 3 + y) % 160 == 0) DrawRect(new Rect2(x + 8, y + 17, 3, 2), new Color("#49665a"));
            }
        DrawRect(new Rect2(96, 280, 768, 48), new Color("#526859"));
        DrawRect(new Rect2(760, 280, 64, 232), new Color("#526859"));
    }
}
