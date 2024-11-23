using System.Collections.Generic;
using TheMage.Core.Scripts.Elements;
using TheMage.Core.Scripts.Entity;
using TheMage.Core.Scripts.Entity.Models;

namespace TheMage.Core.Scripts;

public class Global
{
	public static Global GameGlobal => field??= new Global();

	public Dictionary<string, EntityModel> Entities { get; set; } = new();

	public static readonly string[] Inputs = [
		//Movement
		"MoveLeft","MoveRight","MoveUp","MoveDown",
		//Actions
		"ActionUse","ActionAttack","ActionRun",
		//Magics Joystick
		"MagicLeft","MagicRight",
		"MagicJoy1","MagicJoy2","MagicJoy3","MagicJoy4",
		//Magics Keyboard
		"Magic1","Magic2","Magic3","Magic4","Magic5","Magic6","Magic7","Magic8",
	];

	public static readonly Element[] Elements = [
		new ("Physics", "#363636"),
		new ("Zero", "#FFFFFF"){Modifiers =
		{
			{"Aether",0.5f},
			{"Fire",1.2f},
			{"Air",1.2f},
			{"Earth",1.2f},
			{"Water",1.2f},
		}}, new ("Aether", "#3A003A"){Modifiers =
		{
			{"Zero",0.5f}
		}},
		new ("Fire", "#FF0000"){Modifiers =
		{
			{"Water",0.5f},
			{"Earth",1.2f},
			{"Air",1.5f},
		}}, new ("Air", "#CCCCCC"){Modifiers =
		{
			{"Earth",0.5f},
			{"Water",1.2f},
			{"Fire",1.5f},
		}}, new ("Water", "#0000FF"){Modifiers =
		{
			{"Fire",0.5f},
			{"Air",1.2f},
			{"Earth",1.5f},
		}}, new ("Earth", "#663311"){Modifiers =
		{
			{"Air",0.5f},
			{"Fire",1.2f},
			{"Water",1.5f},
		}},
	];

	public static readonly string[] EquipmentPosition =
	[
		"Hand", "Chest", "Leg", "Feet", "Hand1", "Hand2"
	];

	internal Global()
	{
		
	}
}