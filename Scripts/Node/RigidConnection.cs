using Godot.Collections;
using System.Linq;
using Godot;
using System;

// Rigid connection. Handles any building link with a hard connection between two parts
public class RigidConnection : Connection
{
    public PinJoint2D joint;
    
    [Godot.Export]
    public bool Opposites = true; // Whether this connection will orient itself opposite the joinee

    public override void _Ready()
    {
        BuildBehaviour.mainBehaviour.Connect(nameof(BuildBehaviour.PickedPart), this, nameof(this.ShowSelf));
        BuildBehaviour.mainBehaviour.Connect(nameof(BuildBehaviour.DroppedPart), this, nameof(this.HideSelf));
    }

    // Why can't I connect signals directly?!
    public void HideSelf() {
        Hide(); 
    }
    
    public void ShowSelf() {
        if (Connections.Count() == 0) {
            Show();
        }
    }

    public override void _Draw()
    {
        if (Opposites)
            DrawColoredPolygon(new Vector2[] {new Vector2(-5, 0), new Vector2(0, 10), new Vector2(5, 0)}, color);
        else
            DrawColoredPolygon(new Vector2[] {new Vector2(-5, 0), new Vector2(0, 5), new Vector2(5, 0), new Vector2(0, -5)}, color);
    }

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

            if (c.Mask != Mask) {
                continue;
            }

            if (c.Connections.Count > 0) {
                continue;
            }

            // Stackoverflow is a life saver
            // https://stackoverflow.com/questions/1878907/how-can-i-find-the-difference-between-two-angles

            float a = c.GlobalRotation - GlobalRotation;
            a += (a > Mathf.Pi) ? -Mathf.Pi * 2 : (a<-Mathf.Pi) ? Mathf.Pi * 2 : 0;

            if (a < Mathf.Pi * 0.75f && !Opposites) { // Checks if we're facing the opposite angle of the connection. 
                continue;
            }

             // No need to calculate the root of the square - since we can compare the squared numbers. X^2 = Y^2 is the same as sqrt(X^2) = Y
            if (GlobalPosition.DistanceSquaredTo(c.GlobalPosition) < 20*20) { // Checks if we're close enough to this connection
                return c as RigidConnection;
            }
        }

        return null;
    }
}