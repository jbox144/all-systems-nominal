using Godot;
using System;


// Handles part spawning, build panel UI, and eventually symetry controls
public class BuildPanel : Panel
{
    public static Godot.Collections.Array<PackedScene> parts = new Godot.Collections.Array<PackedScene>();

    [Godot.Export]
    public string[] scenePaths;

    public GridContainer partlist;
    public Panel statpanel;

    public override void _Ready()
    {
        foreach(string s in scenePaths) {
            if(ResourceLoader.Exists(s)) {
                parts.Add((PackedScene)ResourceLoader.Load(s));
            } else {
                GD.PrintErr("Part not found: ", s);
            }
        }

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
        Part instance = part.Instance() as Part;
        GD.Print(instance);
        
        BuildBehaviour behaviour = GetNode<BuildBehaviour>("/root/Root/BuildBehaviour");
        behaviour.PickPart(instance);
    }
}
