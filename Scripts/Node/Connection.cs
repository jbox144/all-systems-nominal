using Godot;
using System;

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
        if (Connections[0] != null) {
            BreakLink();
        }

        Connections[0] = connection;
        Connections[0].Connections[0] = this;
    }

    public void BreakLink() {
        if (Connections[0] != null) {
            Connections[0].Connections[0] = null;
        }
        Connections[0] = null;
    }
}
