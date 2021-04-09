using Godot;
using System;

public class Inspector : Panel
{
    public Line2D pointerLine;
    public Part part;
    public Container header;
    public bool picked;

    public override void _Ready()
    {
        pointerLine = (Line2D)GetNode("Header/PointerLine");
    }

    public override void _Process(float delta)
    {
        if (part == null) {
            QueueFree();
            return;
        }

        pointerLine.SetPointPosition(0, RectPosition);
        pointerLine.SetPointPosition(0, part.GlobalPosition - RectGlobalPosition);
    }


    public void _on_Header_gui_input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton) {
            if (mouseButton.ButtonIndex == (int)ButtonList.Left) {
                if (mouseButton.IsPressed()) {
                    picked = true;
                } else {
                    picked = false;
                }
            }
        }

        if (picked) {
            if (@event is InputEventMouseMotion mouseMotion) {
                RectGlobalPosition += mouseMotion.Relative;
            }
        }
    }

    public void _on_Close_pressed() {
        QueueFree();
    }
}
