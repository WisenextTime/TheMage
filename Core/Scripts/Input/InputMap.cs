using System;
using Godot;
using TheMage.Core.Scripts.IO;
using TheMage.Core.Scripts.IO.TomlModels;

namespace TheMage.Core.Scripts.Input;
public class InputMap
{
	public static InputMap GlobalInputMap => field ??= new InputMap();

	internal InputMap()
	{
		NowInputMapModel = TomlIO.GetDefaultInputMap();
	}
	
	public InputMapModel NowInputMapModel;
	public static void PreloadInputMap()
	{
		var defaultInputMap = TomlIO.GetDefaultInputMap();
		var customInputMap = TomlIO.GetCustomInputMap();
		GlobalInputMap.NowInputMapModel = customInputMap;
		foreach (var input in Global.Inputs)
		{
			var inputKeys = customInputMap.InputMap.TryGetValue(input, out var value)
				? value
				: defaultInputMap.InputMap[input];
			foreach (var key in inputKeys)
			{
				SetInputMap(input, key);
			}
		}
	}

	public static void SetInputMap(string input, string @event)
	{
		if (!Godot.InputMap.HasAction(input)) Godot.InputMap.AddAction(input);
		switch (@event.Split('.'))
		{
			case ["Key", { } keyCode]:
				Godot.InputMap.ActionAddEvent(input,
					new InputEventKey { PhysicalKeycode = (Key)long.Parse(keyCode), Pressed = true });
				break;
			case ["Joy", { } joyAxis and ("L" or "R"), { } joyValue ]:

				Godot.InputMap.ActionAddEvent(input, joyValue switch
				{
					"Left" => new InputEventJoypadMotion
						{ AxisValue = -1f, Axis = joyAxis == "L" ? JoyAxis.LeftX : JoyAxis.RightX },
					"Right" => new InputEventJoypadMotion
						{ AxisValue = 1f, Axis = joyAxis == "L" ? JoyAxis.LeftX : JoyAxis.RightX },
					"Down" => new InputEventJoypadMotion
						{ AxisValue = 1f, Axis = joyAxis == "L" ? JoyAxis.LeftY : JoyAxis.RightY },
					"Up" => new InputEventJoypadMotion
						{ AxisValue = -1f, Axis = joyAxis == "L" ? JoyAxis.LeftY : JoyAxis.RightY },
					_ => throw new ArgumentOutOfRangeException()
				});
				break;
			case ["Joy", { } joyAxis and ("LT" or "RT")]:
				Godot.InputMap.ActionAddEvent(input, new InputEventJoypadMotion
				{
					Axis = joyAxis == "LT" ? JoyAxis.TriggerLeft : JoyAxis.TriggerRight,
					AxisValue = 1f
				});
				break;
			case ["Joy", { } keyCode]:
				Godot.InputMap.ActionAddEvent(input,
					new InputEventJoypadButton
						{ ButtonIndex = (JoyButton)long.Parse(keyCode), Pressed = true });
				break;
			case ["Mouse", { } mouseButtonIndex]:
				Godot.InputMap.ActionAddEvent(input,
					new InputEventMouseButton
						{ ButtonIndex = (MouseButton)long.Parse(mouseButtonIndex), Pressed = true });
				break;
		}
	}

	public static void SaveInputMap()
	{
		TomlIO.SaveCustomInputMap(GlobalInputMap.NowInputMapModel);
	}
}