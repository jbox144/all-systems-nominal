using Godot;
using System;


// Handles part spawning, build panel UI, and eventually symetry controls
public class BuildPanel : Panel
{
    public static Godot.Collections.Array<PackedScene> parts;
    public GridContainer partlist;
    public Panel statpanel;

    public override void _Ready()
    {
        parts = new Godot.Collections.Array<PackedScene> { // APPARENTLY I can't find a method to itterate over a folder's contents. So it's a code-side list. :^)
            (PackedScene)ResourceLoader.Load("res://Scenes/Parts/Part.tscn"),
            (PackedScene)ResourceLoader.Load("res://Scenes/Parts/Part1.tscn"),
            (PackedScene)ResourceLoader.Load("res://Scenes/Parts/Gyro.tscn"),
            (PackedScene)ResourceLoader.Load("res://Scenes/Parts/Rocket.tscn"),
            // (PackedScene)ResourceLoader.Load("res://Scenes/Parts/FuelTank.tscn") // Not ready yet. Talking to you, yay.
        };

        partlist = GetNode<GridContainer>("CategoryTabs/General/ItemList");
        statpanel = GetNode<Panel>("Stats");

        for(int i = 0; i < parts.Count; i++) {
            Button button = new Button();
            Part temp = (Part)parts[i].Instance(); // Maybe I should just make a dict for the basic stats of each part instead of pre-loading it...
            button.Connect("button_down", this, nameof(ButtonDown), new Godot.Collections.Array{i});
            button.Connect("mouse_entered", this, nameof(Hover), new Godot.Collections.Array{i});
            button.Text = temp.PartName;
            partlist.AddChild(button);
            temp.Free();
        }
    }

    // Triggers create part when the build panel is used
    public void ButtonDown(int index) {
        CreatePart(parts[index]);
    }

    // When hovering over a part, show it's name and desc in the stats panel
    public void Hover(int index) {
        Part temp = (Part)parts[index].Instance();
        statpanel.GetNode<RichTextLabel>("Name").BbcodeText = temp.PartName;
        statpanel.GetNode<RichTextLabel>("Desc").BbcodeText = temp.PartDesc;
        temp.Free();
    }

    // Creates a part and picks it with the mouse
    public void CreatePart(PackedScene part) {
        Part instance = (Part)part.Instance();
        instance.Picked = true;
        instance.CustomIntegrator = true;
        instance.CollisionLayer = 1;
        GetTree().GetRoot().AddChild(instance);
        instance.GlobalPosition = GetGlobalMousePosition();
    }
}
