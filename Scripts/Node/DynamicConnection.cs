using Godot;
using System;
using System.Collections.Generic;

// Dynamic connection. Handles non-physical links between parts, such as fuel, power, logic linkages.
public class DynamicConnection : Connection
{
    public bool Picked = false; // Are we currently picked up by the mouse?
    public int PickedTime = 0;
    
    [Godot.Export]
    public Color DrawColor;

    
    public override void _Ready() {
        base._Ready();
    }

    public void _on_button_down() {
        Picked = true;
        GD.Print("Mouse down");
    }

    public override void _Draw() {
        DrawPolygon(new Vector2[] { new Vector2 (-10, 0), new Vector2 (0, 10), new Vector2 (10, 0), new Vector2 (0, -10)}, new Color[] { DrawColor });

        if (Picked) {
            DrawLine(new Vector2(0, 0),  GetLocalMousePosition(), DrawColor, 4);
        }

        foreach (Connection c in Connections) {
            DrawLine(new Vector2(0, 0), ToLocal(c.GlobalPosition), DrawColor, 4);
        }
    }

    public override void _Process(float delta) {
        Update();
    }
    
    public void _on_button_up() {
        Picked = false;
        GD.Print("Mouse up");

        DynamicConnection link = FindLink();
        if (link != null) {
            if (Connections.Contains(link)) {
                BreakLink(link);
            } else {
                MakeLink(link);
            }
        }
    }

    public DynamicConnection FindLink() {
        foreach(Connection c in GlobalConnections) {
            if (!(c is DynamicConnection)) {
                continue;
            }
            
            if (c == this) { // Checks if this is the same connection as us. No self-connecting :T
                continue;
            }

            if (c.GetParent() == GetParent()) { // Checks if the other connection is part of the same parent
                continue;
            }

             // No need to calculate the root of the square - since we can compare the squared numbers. X^2 = Y^2 is the same as sqrt(X^2) = Y
            if (GetGlobalMousePosition().DistanceSquaredTo(c.GlobalPosition) < 10*10) { // Checks if we're close enough to this connection
                return c as DynamicConnection;
            }

        }

        return null;
    }

    new public void MakeLink(Connection connection) { // At the moment, there isn't much else relevant to connections.
        Connections.Add(connection);
        connection.Connections.Add(this);
    }

    public void BreakLink(Connection connection) {
        Connections.Remove(connection);
        connection.Connections.Remove(this);
    }
}