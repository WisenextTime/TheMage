using System;
using System.Collections.Generic;

using TheMage.Core.Scripts.Elements;

namespace TheMage.Core.Scripts;

public class Global
{
	public static Global GameGlobal => field ??= new Global();

	public static readonly string[] Inputs =
	[
		//Movement
		"MoveLeft", "MoveRight", "MoveUp", "MoveDown",
		//Actions
		"ActionUse", "ActionAttack", "ActionRun",
		//Magics Joystick
		"MagicLeft", "MagicRight",
		"MagicJoy1", "MagicJoy2", "MagicJoy3", "MagicJoy4",
		//Magics Keyboard
		"Magic1", "Magic2", "Magic3", "Magic4", "Magic5", "Magic6", "Magic7", "Magic8",
	];

	public static readonly Element[] Elements = Enum.GetValues<Element>();

	public static readonly string[] EquipmentPosition =
	[
		"Hand", "Chest", "Leg", "Feet", "Hand1", "Hand2"
	];
}