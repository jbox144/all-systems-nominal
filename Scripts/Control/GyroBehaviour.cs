using Godot;
using System;

public class GyroBehaviour : Node
{
    public RigidBody2D parent;

    public override void _Ready()
    {
        parent = (RigidBody2D)GetParent();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsKeyPressed((int)KeyList.A)) {
            parent.ApplyTorqueImpulse(-500);
        }
        
        if (Input.IsKeyPressed((int)KeyList.D)) {
            parent.ApplyTorqueImpulse(500);
        }
    }
}