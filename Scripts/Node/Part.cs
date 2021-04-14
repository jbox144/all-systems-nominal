using System.ComponentModel;
using Godot;
using System;


// Base part class. Handles building logic too, because I can't translate parts without directly interfacing it's physics process. Price you pay I guess.
public class Part : RigidBody2D
{
    [Godot.Export]
    public string PartName = "Part"; // Part name, because you can't have nodes with the same name, but we want our ships having more than one part.
    [Godot.Export]
    public string PartDesc = "Desc"; // Part description


    public bool Picked = false; // Are we currently picked up by the mouse?
    public int PickedTime = 0;
    public bool QueRot = false; // Used by the input handler to tell the physics handler to rotate the next tick
    public bool QueConnect = false; // Used by the input handler to tell the physics handler to check for connections and connect the next tick

    public static PackedScene inspectorResource = (PackedScene)ResourceLoader.Load("res://Scenes/Inspector.tscn"); // Inspector resource
    public Inspector inspector;

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {

    }

    /* // Meant to deal with the mouse getting stuck, but doesn, so eh...
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent) {
            if(mouseEvent.ButtonIndex == (int)ButtonList.Left && !mouseEvent.IsPressed() && PickedTime < OS.GetTicksMsec())
            {
                Picked = false;
                CustomIntegrator = true;
                QueConnect = true;
            }
        }
    } 
    */

    public void Pick(bool picking) {

    }

    public override void _InputEvent (Godot.Object viewport, InputEvent inputEvent, int shapeidx)
    {
        if (inputEvent is InputEventMouseButton mouseEvent) {
            if(mouseEvent.ButtonIndex == (int)ButtonList.Left && (mouseEvent.IsPressed() || PickedTime < OS.GetTicksMsec()))
            {
                Picked = !Picked;
                CustomIntegrator = Picked;
                CollisionLayer = Picked ? 1 : 3;
                PickedTime = OS.GetTicksMsec() + 200;

                if (Picked) {
                    foreach(Node child in GetChildren()) {
                        if (child is RigidConnection connection) {
                            connection.BreakLink();
                        }
                    }
                } else {
                    CustomIntegrator = true;
                    QueConnect = true;
                }
            }

            if(mouseEvent.ButtonIndex == (int)ButtonList.Right && mouseEvent.Pressed)
            {
                if (Picked) {
                    QueRot = true;
                } else {
                    if (inspector != null && !inspector.IsQueuedForDeletion()) {
                        inspector.QueueFree();
                    }
                    inspector = (Inspector)inspectorResource.Instance();
                    inspector.part = this;
                    GetNode<Control>("/root/Root/Camera2D/UI").AddChild(inspector);
                }
            }
        }
    }

    // Because of the way GODOT works, I have to litterally override the physics function just to move my parts around. :)
    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {


        if(Picked)
        {
            state.SetLinearVelocity((GetGlobalMousePosition() - Position) * 20);
            state.SetAngularVelocity(0);
            state.SetTransform(new Transform2D(QueRot ? Rotation + (Mathf.Pi * 0.25f) : Rotation, GetGlobalMousePosition()));
            
            QueRot = false;
        }
        else 
        {
            if(QueConnect) {
                foreach(Node child in GetChildren()) {
                    if (child is RigidConnection connection) {
                        RigidConnection c = connection.FindLink();
                        if (c != null) {                        
                            Transform2D transform = new Transform2D(c.GlobalRotation - connection.Rotation + Mathf.Pi, GlobalPosition);
                            Transform = transform;
                            state.Transform = new Transform2D(Rotation, c.GlobalPosition + (GlobalPosition - connection.GlobalPosition));
                            connection.MakeLink(c);
                        }
                    }
                }
                CustomIntegrator = false;
                QueConnect = false;
            }
            base._IntegrateForces(state);
        }
    }
}
