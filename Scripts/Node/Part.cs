using System.ComponentModel;
using Godot;
using System;


// Base part class.
public class Part : CollisionShape2D
{
    [Godot.Export]
    public string PartName = "Part"; // Part name, because you can't have nodes with the same name, but we want our ships having more than one part.
    [Godot.Export]
    public string PartDesc = "Desc"; // Part description
    
    public Inspector inspector;

    public override void _UnhandledInput(InputEvent e)
    {
        if (GetParent() == BuildBehaviour.mainBehaviour || GetParentOrNull<Node>() == null) {
            return;
        }

        InputEventMouseButton button = e as InputEventMouseButton;
        
        if (button != null && button.Pressed && Shape.Collide( GlobalTransform, new SegmentShape2D(), new Transform2D(0, GetGlobalMousePosition()) )) {
            if ((ButtonList)button.ButtonIndex == ButtonList.Left) {
                if (BuildBehaviour.mainBehaviour.GetChildCount() == 0)
                    BuildBehaviour.mainBehaviour.PickPart(this);
                    GetTree().SetInputAsHandled();
            }

            if ((ButtonList)button.ButtonIndex == ButtonList.Right) {
                BuildBehaviour.mainBehaviour.ContextPart(this);
                GetTree().SetInputAsHandled();
            }
        }
    }

    public Vessel GetVessel() {
        Node n = this;
        while(n != null && !(n is Vessel)) {
            n = n.GetParentOrNull<Node>();
        }
        return n as Vessel;
    }
}
