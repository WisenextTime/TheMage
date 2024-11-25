using System;
using System.Collections.Generic;

using TheMage.Core.Scripts.Elements;
using TheMage.Core.Scripts.Entities.Models;
using TheMage.Core.Scripts.Items;

namespace TheMage.Core.Scripts;

public class Global
{
	public static Global GameGlobal => field ??= new Global();

	public Dictionary<string, EntityModel> Entities { get; set; } = new();

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

	public static readonly EquipSlot[] EquipSlots = Enum.GetValues<EquipSlot>();
}