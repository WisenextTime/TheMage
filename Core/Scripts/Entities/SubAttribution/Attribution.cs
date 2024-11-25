using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using TheMage.Core.Scripts.Elements;

namespace TheMage.Core.Scripts.Entities.SubAttribution;

public record struct Attribution() : IAdditionOperators<Attribution, Attribution, Attribution>
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
	public ElementValues<ElementData> ElementDataSet { get; set; } = [];

	public FinalAttribution ToFinal() =>
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
			ElementDataSet = ElementDataSet.With(static data => data.ToFinal())
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
			ElementDataSet = left.ElementDataSet.Add(right.ElementDataSet)
		};
}

public record struct FinalAttribution()
{
	public int MaxHp { get; init; }
	public int MaxMp { get; init; }
	public float HpRegSpd { get; init; }
	public float MpRegSpd { get; init; }

	public float AtkSpd { get; init; }
	public float MovSpd { get; init; }

	public float Cri { get; init; }
	public float CriDmg { get; init; }
	public ElementValues<FinalElementData> ElementDataSet = [];
}

public record struct ElementData : IAdditionOperators<ElementData, ElementData, ElementData>
{
	public int Atk { get; set; }
	public int Def { get; set; }
	public float AtkMul { get; set; }
	public float DefMul { get; set; }

	public FinalElementData ToFinal() => new()
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

public record struct FinalElementData
{
	public int Atk { get; init; }
	public int Def { get; init; }
}