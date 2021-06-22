using System.Collections.ObjectModel;
using System.Collections.Generic;
using Godot;
using System;

// Fluid storage. Handles the storage of fluids.
public class FluidStorage : Node2D
{
    public Godot.Collections.Dictionary<string, float> Contents = new Godot.Collections.Dictionary<string, float>();

    [Godot.Export]
    public Rect2 Display;
    
    [Godot.Export]
    public Color DrawColor;

    public Connection connector;
    
    [Godot.Export]
    public float Temperature = 293.15f; // A nice 20 degrees. Centigrade, of course.
    
    [Godot.Export]
    public float moles {
        get {
            float sum = 0;
            foreach (KeyValuePair<string, float> kv in Contents) {
                sum += kv.Value;
            }
            return sum;
        }
        set {
            float difference = value / moles;
            foreach (string s in Contents.Keys) {
                Contents[s] *= difference;
            }
        }
    }

    [Godot.Export]
    public float Pressure {
        get {
            return moles * Temperature;
        }
        set {
            float difference = value / Pressure;
            foreach (string s in Contents.Keys) {
                Contents[s] *= difference;
            }
        }
    }

    public override void _Ready()
    {
        connector = GetChild<Connection>(0);
        Contents["plasma"] = 32;
    }

    public override void _Draw() {

        DrawRect(new Rect2(0, 16, 16, -moles), DrawColor);
    }

    public override void _Process(float delta) {
        Update();
    }


    // Physics time step heh
    // Eventually this will handle reactions and such
    public override void _PhysicsProcess(float delta)
    {
        foreach(Connection c in connector.Connections) {       
            FluidStorage other = c.GetParent() as FluidStorage;
            if (other == null) continue;

            float pressuredif = (Pressure - other.Pressure) * 0.001f;

            foreach(string content in Contents.Keys) {
                Contents[content] -= pressuredif;
                other.Contents[content] += pressuredif;
            }
        }
    }
}
