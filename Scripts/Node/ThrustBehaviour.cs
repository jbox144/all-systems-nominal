using Godot;
using System;

public class ThrustBehaviour : Node
{
    public RigidBody2D parent;

    public override void _Ready()
    {
        parent = (RigidBody2D)GetParent();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsKeyPressed((int)KeyList.W)) {
            parent.ApplyCentralImpulse(new Vector2(0, -32).Rotated(parent.Rotation));
        }
    }
}
