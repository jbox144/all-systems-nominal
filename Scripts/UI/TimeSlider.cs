using Godot;
using System;

// Does nothing. Seriously, I have no idea how to change the physics step.
public class TimeSlider : HSlider
{
	public Label label;
	
	public override void _Ready()
	{
	    label = GetParent().GetChild(1) as Label;
		Connect("value_changed", this, nameof(value_changed));
	}
	
	public void value_changed (float v){
		label.Text = v.ToString();
	}
}
