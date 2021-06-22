using Godot;
using System;

// Temporary behaviour for making thrusters go up. Will be replaced with a complex interface when I have it ready.
public class ThrustBehaviour : Node
{
    public RigidBody2D parent;
    public Connection connector;

    public override void _Ready()
    {
        parent = (RigidBody2D)GetParent();
        connector = GetChild<Connection>(0);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsKeyPressed((int)KeyList.W)) {
            if (connector.Connections.Count == 0) {
                return;
            }
            FluidStorage storage = connector.Connections[0].GetParent<FluidStorage>();

            if (storage == null) {
                return;
            }

            if (storage.moles > 0.1f) {
                storage.moles -= 0.1f;
                parent.ApplyCentralImpulse(new Vector2(0, -32).Rotated(parent.Rotation)); // I know physics timesteps aren't fixed here! I'll replace it with force eventually.
            }
        }
    }
}
