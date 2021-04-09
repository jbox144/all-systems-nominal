using System.ComponentModel.Design.Serialization;
using Godot;
using System;

public class BuildPanel : Panel
{
    public static PackedScene part1 = (PackedScene)ResourceLoader.Load("res://Scenes/Part.tscn");
    public static PackedScene part2 = (PackedScene)ResourceLoader.Load("res://Scenes/Part1.tscn");
    public static PackedScene part3 = (PackedScene)ResourceLoader.Load("res://Scenes/Rocket.tscn");
    public static PackedScene part4 = (PackedScene)ResourceLoader.Load("res://Scenes/Gyro.tscn");


    public override void _Ready()
    {
        
    }

    // Mmm yes, coding.
    // I need to replace this with an array of parts asap.
    public void _on_Button_button_down() {
        CreatePart(part1);
    }

    public void _on_Button3_button_down() {
        CreatePart(part2);
    }

    public void _on_Button4_button_down() {
        CreatePart(part3);
    }

    public void _on_Button5_button_down() {
        CreatePart(part4);
    }

    public void CreatePart(PackedScene part) {
        Part instance = (Part)part.Instance();
        instance.Picked = true;
        instance.CustomIntegrator = true;
        instance.CollisionLayer = 1;
        GetTree().GetRoot().AddChild(instance);
        instance.GlobalPosition = GetGlobalMousePosition();
    }
}
