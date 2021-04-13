using System.Net;
using Godot;
using System;

public class RigidConnection : Connection
{
    public PinJoint2D joint;

    public RigidConnection FindLink() {
        foreach(RigidConnection connection in GlobalConnections) {
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

    public void MakeLink(RigidConnection connection) {
        base.MakeLink(connection);
        
        joint = new PinJoint2D();
        connection.joint = new PinJoint2D();
        
        AddChild(joint);
        joint.Position = new Vector2(10, 0);
        joint.Bias = 0.5f;

        joint.SetNodeA(this.GetParent<RigidBody2D>().GetPath());
        joint.SetNodeB(connection.GetParent<RigidBody2D>().GetPath());

        joint.DisableCollision = false;

        connection.AddChild(connection.joint);
        connection.joint.Position = new Vector2(10, 0);
        connection.joint.Bias = 0.5f;
        
        connection.joint.SetNodeA(connection.GetParent<RigidBody2D>().GetPath());
        connection.joint.SetNodeB(this.GetParent<RigidBody2D>().GetPath());

        connection.joint.DisableCollision = false;
    }

    new public void BreakLink() {
        if (Connections[0] != null) {
            if (((RigidConnection)Connections[0]).joint != null) {
                ((RigidConnection)Connections[0]).joint.QueueFree();
            }
        }
        if (joint != null) {
            joint.QueueFree();
        }
        base.BreakLink();
    }
}