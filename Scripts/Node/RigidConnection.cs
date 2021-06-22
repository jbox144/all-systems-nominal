using Godot.Collections;
using System.Linq;
using Godot;
using System;

// Rigid connection. Handles any building link with a hard connection between two parts
public class RigidConnection : Connection
{
    public PinJoint2D joint;

    public RigidConnection FindLink() {
        foreach(Connection c in GlobalConnections) {
            if (!(c is RigidConnection)) {
                continue;
            }
            
            if (c == this) { // Checks if this is the same connection as us. No self-connecting :T
                continue;
            }

            if (c.GetParent() == GetParent()) { // Checks if the other connection is part of the same parent
                continue;
            }
            
            if ((GlobalRotation - c.GlobalRotation - Math.PI) % (Math.PI*2) > Math.PI * 0.25) { // Checks if we're facing the opposite angle of the connection. Don't ask me about the math, I can't remember how it works
                continue;
            }

             // No need to calculate the root of the square - since we can compare the squared numbers. X^2 = Y^2 is the same as sqrt(X^2) = Y
            if (GlobalPosition.DistanceSquaredTo(c.GlobalPosition) < 20*20) { // Checks if we're close enough to this connection
                return c as RigidConnection;
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
        if (Connections.Count > 0) {
            if ((Connections[0] as RigidConnection).joint != null) {
                (Connections[0] as RigidConnection).joint.QueueFree();
            }
        }
        if (joint != null) {
            joint.QueueFree();
        }

        base.BreakLink();
    }
}