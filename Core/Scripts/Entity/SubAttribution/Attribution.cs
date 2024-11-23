using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using TheMage.Core.Extensions;

namespace TheMage.Core.Scripts.Entity.SubAttribution;

public record Attribution : IAdditionOperators<Attribution, Attribution, Attribution>
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
	public Dictionary<string, ElementData> ElementDataSet = [];

	public static TrueAttribution GetEntityTrueAttribution(Entity entity)
	{
		var sourceAttribution = entity.StatusCurve.Transform(entity.Attribution, entity.Level);
		var equipmentAttributions = entity.Equipments.Select(e => e.Value)
		                                  .Select(e => e.StatusCurves.Transform(e.Attributions, e.Level)).ToArray();
		var buffAttributions = entity.Buffs.Select(b => b.AttributeModifier).ToArray();

		Attribution[] allAttributions = [sourceAttribution, ..equipmentAttributions, ..buffAttributions];

		return allAttributions.Sum().ToTrueAttribution();
	}

	public TrueAttribution ToTrueAttribution() =>
		new()
		{
			MaxHp = (int)(MaxHp * (1 + MaxHpMul)),
			MaxMp = (int)(MaxMp * (1 + MaxMpMul)),
			HpRegSpd = HpRegSpd,
			MpRegSpd = MpRegSpd,
			AtkSpd = AtkSpd,
			MovSpd = MovSpd,
			Cri = Cri,
			CriDmg = CriDmg,
			ElementDatas = ElementDataSet.ToDictionary(pair => pair.Key, pair => pair.Value.ToTrueElementData())
		};

	public static Attribution operator +(Attribution left, Attribution right) =>
		new()
		{
			MaxHp = left.MaxHp + right.MaxHp,
			MaxMp = left.MaxMp + right.MaxMp,
			MaxHpMul = left.MaxHpMul + right.MaxHpMul,
			MaxMpMul = left.MaxMpMul + right.MaxMpMul,
			HpRegSpd = left.HpRegSpd + right.HpRegSpd,
			MpRegSpd = left.MpRegSpd + right.MpRegSpd,
			AtkSpd = left.AtkSpd + right.AtkSpd,
			MovSpd = left.MovSpd + right.MovSpd,
			Cri = left.Cri + right.Cri,
			CriDmg = left.CriDmg + right.CriDmg,
			ElementDataSet = Global.Elements.ToDictionary
				(e => e.Name,
				 e => left.ElementDataSet.GetValueOrDefault(e.Name) + right.ElementDataSet.GetValueOrDefault(e.Name))
		};
}

public record TrueAttribution
{
	public int MaxHp { get; set; }
	public int MaxMp { get; set; }
	public float HpRegSpd { get; set; }
	public float MpRegSpd { get; set; }

	public float AtkSpd { get; set; }
	public float MovSpd { get; set; }

	public float Cri { get; set; }
	public float CriDmg { get; set; }
	public Dictionary<string, TrueElementData> ElementDatas = [];
}

public record ElementData : IAdditionOperators<ElementData, ElementData, ElementData>
{
	public int Atk;
	public int Def;
	public float AtkMul;
	public float DefMul;

	public TrueElementData ToTrueElementData() => new()
	{
		Atk = (int)(Atk * (AtkMul + 1)),
		Def = (int)(Def * (DefMul + 1)),
	};

	public static ElementData operator +(ElementData left, ElementData right) => new()
	{
		Atk = left.Atk + right.Atk,
		Def = left.Def + right.Def,
		AtkMul = left.AtkMul + right.AtkMul,
		DefMul = left.DefMul + right.DefMul,
	};
}

public record TrueElementData
{
	public int Atk;
	public int Def;
}