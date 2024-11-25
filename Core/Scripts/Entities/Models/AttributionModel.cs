using System;
using System.Collections.Generic;
using System.Linq;

using TheMage.Core.Scripts.Data;
using TheMage.Core.Scripts.Elements;
using TheMage.Core.Scripts.Entities.SubAttribution;

namespace TheMage.Core.Scripts.Entities.Models;

public record AttributionModel : IModel<Attribution>
{
	public int MaxHp { get; set; }
	public int MaxMp { get; set; }

	public float MaxHpMul { get; set; }

	public float MaxMpMul { get; set; }
	public float HpRegSpd { get; set; }
	public float MpRegSpd { get; set; }

	public float InitHp { get; set; } = 1;
	public float InitMp { get; set; } = 1;

	public float AtkSpd { get; set; }
	public float MovSpd { get; set; }

	public float Cri { get; set; }
	public float CriDmg { get; set; }

	public Dictionary<string, ElementModel> Elements { get; set; } = new();

	public Attribution Convert() => new()
	{
		MaxHp = MaxHp,
		MaxMp = MaxMp,
		MaxHpMul = MaxHpMul,
		MaxMpMul = MaxMpMul,
		HpRegSpd = HpRegSpd,
		MpRegSpd = MpRegSpd,
		InitHp = InitHp,
		InitMp = InitMp,
		AtkSpd = AtkSpd,
		MovSpd = MovSpd,
		Cri = Cri,
		CriDmg = CriDmg,
		ElementDataSet = ElementValues.Create(Elements.ToDictionary(pair => Enum.Parse<Element>(pair.Key),
		                                                            pair => pair.Value.Convert()))
	};
}

public record ElementModel : IModel<ElementData>
{
	public int Atk { get; set; }
	public int Def { get; set; }
	public int AtkMul { get; set; }
	public int DefMul { get; set; }

	public ElementData Convert() => new()
	{
		Atk = Atk,
		Def = Def,
		AtkMul = AtkMul,
		DefMul = DefMul
	};
}