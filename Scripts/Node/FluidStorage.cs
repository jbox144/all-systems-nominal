using System.Collections.ObjectModel;
using System.Collections.Generic;
using Godot;
using System;

public class FluidStorage : Node
{
    [Godot.Export]
    public Godot.Collections.Dictionary<string, float> Contents;
    
    public float Temperature = 293.15f; // A nice 20 degrees. Centigrade, of course.
    
    public float moles {
        get {
            float sum = 0;
            foreach (KeyValuePair<string, float> kv in Contents) {
                sum += kv.Value;
            }
            return sum;
        }
    }

    public float Pressure {
        get {
            return moles * Temperature;
        }
    }

    public override void _Ready()
    {
        
    }

    // Physics time step heh
    // Eventually this will handle reactions and such
    public override void _PhysicsProcess(float delta)
    {
        
    }
}
