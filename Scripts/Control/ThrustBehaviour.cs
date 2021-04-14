using Godot;
using System;

// Temporary behaviour for making thrusters go up. Will be replaced with a complex interface when I have it ready.
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
            parent.ApplyCentralImpulse(new Vector2(0, -32).Rotated(parent.Rotation)); // I know physics timesteps aren't fixed here! I'll replace it with force eventually.
        }
    }
}
