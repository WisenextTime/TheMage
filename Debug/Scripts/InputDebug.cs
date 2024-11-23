using Godot;
using InputMap = TheMage.Core.Scripts.Input.InputMap;

namespace TheMage.Debug.Scripts;

public partial class InputDebug : Control
{
	private Label Text => field??= GetNode<Label>("Text");
	public override void _Ready()
	{
		InputMap.PreloadInputMap();
	}

	public override void _PhysicsProcess(double delta)
	{

		Text.Position = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown").Normalized() * 100;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!@event.IsPressed()) return;
		foreach (var action in Godot.InputMap.GetActions())
		{
			if (@event.IsAction(action, true))
				Text.Text = $"{action}:\n{@event.AsText()}";
		}
	}
}