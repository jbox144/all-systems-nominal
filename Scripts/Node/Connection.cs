using Godot;
using System;

public class Connection : Node2D
{
    public Connection other;
    public PinJoint2D joint;



    static Godot.Collections.Array<Connection> Connections = new Godot.Collections.Array<Connection>();

    public override void _Ready() {
        Connections.Add(this);
    }

    public Connection FindLink() {
        foreach(Connection connection in Connections) {
            if (connection == this) { // D
                continue;
            }

            if (connection.GetParent() == GetParent()) {
                continue;
            }
            if ((GlobalRotation - connection.GlobalRotation - Math.PI) % (Math.PI*2) > Math.PI * 0.25) {
                continue;
            }

            if (GlobalPosition.DistanceSquaredTo(connection.GlobalPosition) < 20*20) {
                return connection;
            }

        }

        return null;
    }

    public void MakeLink(Connection connection) {
        if (other != null) {
            BreakLink();
        }

        other = connection;
        other.other = this;
        
        joint = new PinJoint2D();
        other.joint = new PinJoint2D();
        
        AddChild(joint);
        joint.Position = new Vector2(10, 0);
        joint.Bias = 0.5f;

        joint.SetNodeA(this.GetParent<RigidBody2D>().GetPath());
        joint.SetNodeB(other.GetParent<RigidBody2D>().GetPath());

        joint.DisableCollision = false;

        other.AddChild(other.joint);
        other.joint.Position = new Vector2(10, 0);
        other.joint.Bias = 0.5f;
        
        other.joint.SetNodeA(other.GetParent<RigidBody2D>().GetPath());
        other.joint.SetNodeB(this.GetParent<RigidBody2D>().GetPath());

        other.joint.DisableCollision = false;
    }

    public void BreakLink() {
        if (other != null) {
            other.other = null;
            other.joint.QueueFree();
        }
        if (joint != null) {
            joint.QueueFree();
        }
        other = null;
    }
}
