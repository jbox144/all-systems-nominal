using Godot;
using System;

// Temporary behaviour for making gyros spin by keyboard input. Will be replaced with a complex interface when I have it ready.
public class GyroBehaviour : Node
{
    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsKeyPressed((int)KeyList.A)) {
            (GetParent() as Part).GetVessel().ApplyTorqueImpulse(-500);
        }
        
        if (Input.IsKeyPressed((int)KeyList.D)) {
            (GetParent() as Part).GetVessel().ApplyTorqueImpulse(500);
        }
    }
}