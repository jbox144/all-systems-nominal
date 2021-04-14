using System.Collections.Generic;
using Godot;
using System;

// Base connection class, used by the dynamic connection and rigid connection.
// Using a base class so fluid transfer and power transfer can use either connection type
public class Connection : Node2D
{
    public Godot.Collections.Array<Connection> Connections = new Godot.Collections.Array<Connection>();

    public static Godot.Collections.Array<Connection> GlobalConnections = new Godot.Collections.Array<Connection>();

    public override void _Ready() {
        GlobalConnections.Add(this);
    }

    public override void _ExitTree()
    {
        GlobalConnections.Remove(this);
    }

    public void MakeLink(Connection connection) { // Base connections are intended for singular connections. Some require plural connections.
        if (Connections.Count > 0) {
            BreakLink();
        }

        Connections.Add(connection);
        Connections[0].Connections.Add(this);
    }

    public void BreakLink() {
        if (Connections.Count > 0) {
            if (Connections[0].Connections.Count > 0) {
                Connections[0].Connections.RemoveAt(0);
            }
            Connections.RemoveAt(0);
        }
    }
}
