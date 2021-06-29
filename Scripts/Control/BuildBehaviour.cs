using Godot;
using System;

public class BuildBehaviour : Node2D
{
    public static BuildBehaviour mainBehaviour;
    static PackedScene inspector;
    static PackedScene vessel;
    int LastClick;

    [Signal]
    public delegate void PickedPart();

    [Signal]
    public delegate void DroppedPart();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mainBehaviour = this;
        inspector = (PackedScene)ResourceLoader.Load("res://Scenes/Inspector.tscn");
        vessel = (PackedScene)ResourceLoader.Load("res://Scenes/Vessel.tscn");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public void PickPart(Part part) {
        foreach(Node node in part.GetChildren()) { // Remove all connections
            Connection c = node as Connection;
            if (c == null) continue;
            c.BreakLink();
        }

        Node parent = part.GetParent(); // If there's a parent, orphan this child
        if (parent != null) {
            parent.RemoveChild(part);
            if (parent.GetChildCount() == 0) {
                parent.QueueFree();
            }
        }

        GD.Print(part);

        AddChild(part); // Our child now!
        part.GlobalPosition = GetGlobalMousePosition();
        LastClick = OS.GetTicksMsec();
        EmitSignal(nameof(PickedPart));
    }

    public void DropPart() {
        foreach(Part p in GetChildren()) {
            if (p == null) continue;
            Vessel v;
            foreach(Node node in p.GetChildren()) {
                RigidConnection c = node as RigidConnection; // Check if this is a rigid connection
                if (c == null) continue;
                RigidConnection l = c.FindLink(); // Try finding a link
                if (l == null) continue; 
                Part o = l.GetPart(); // Get the parent part
                if (o == null) continue;
                v = o.GetVessel(); // Get the parent vessel
                if (v == null) continue;
                RemoveChild(p); // Move the part to the orientation matching the node, and add it to this node's vessel
                v.AddChild(p);
                p.GlobalRotation = l.GlobalRotation - c.Rotation + (c.Opposites ? Mathf.Pi : 0);
                p.GlobalPosition = l.GlobalPosition - c.Position;
                break;
            }
            if (p.GetParent() != this) { // Set up all the links - legitimately.
                foreach (Node node in p.GetChildren()) {
                    RigidConnection c = node as RigidConnection;
                    if (c == null) continue;
                    RigidConnection l = c.FindLink();
                    if (l == null) continue;
                    c.MakeLink(l);
                }
                break;
            }
            v = vessel.Instance() as Vessel;
            v.GlobalPosition = p.GlobalPosition;
            p.Position = new Vector2();
            GetTree().GetRoot().GetChild(0).AddChild(v);
            RemoveChild(p);
            v.AddChild(p);
        }
        EmitSignal(nameof(DroppedPart));
    }

    public void ContextPart(Part part) {
        
    }

    public override void _UnhandledInput(InputEvent e)

    {
        InputEventMouseMotion motion = e as InputEventMouseMotion;
        InputEventMouseButton button = e as InputEventMouseButton;

        if (motion != null) {
            foreach(Part p in GetChildren()) {
                if (p == null) continue;
                p.SetGlobalPosition(motion.GlobalPosition);
            }
        }
        
        if (button != null && GetChildCount() > 0) {
            if (button.Pressed) { // If clicked again and we still have a part, drop it
                if ((ButtonList)button.ButtonIndex == ButtonList.Left) {
                    if (OS.GetTicksMsec() > LastClick + 1) {
                        DropPart();
                    }
                }
                if ((ButtonList)button.ButtonIndex == ButtonList.Right) {
                    foreach(Part p in GetChildren()) {
                        if (p == null) continue;
                        p.Rotation += Mathf.Pi * 0.25f; // 45 degrees. Imagine not knowing trig :3
                    }
                }
            } else {
                if ((ButtonList)button.ButtonIndex == ButtonList.Left && OS.GetTicksMsec() > LastClick + 100) {  // If released after we held it for more than 100ms, drop it
                    DropPart();
                }
            }
        }
    }
}
