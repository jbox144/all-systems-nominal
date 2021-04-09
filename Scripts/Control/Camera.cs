using Godot;
using System;

public class Camera : Camera2D
{
    public bool dragging = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton) {
            if (mouseButton.ButtonIndex == (int)ButtonList.Right) {
                dragging = mouseButton.Pressed;
                
            }
        }

        if (dragging) {
            if (@event is InputEventMouseMotion mouseMotion) {
                Translate(-mouseMotion.Relative);
            }
        }
    }
}
