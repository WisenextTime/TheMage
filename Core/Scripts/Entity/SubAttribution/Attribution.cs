using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using TheMage.Core.Extensions;

namespace TheMage.Core.Scripts.Entity.SubAttribution;

public record Attribution
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
	public Dictionary<string, ElementData> ElementDatas = [];

	public static TrueAttribution GetEntityTrueAttribution(Entity entity)
	{
		var sourceAttribution = entity.StatusCurve.Transform(entity.Attribution, entity.Level);
		var equipmentAttributions = entity.Equipments.Select(e => e.Value)
		                                  .Select(e => e.StatusCurves.Transform(e.Attributions, e.Level)).ToArray();
		var buffAttributions = entity.Buffs.Select(b => b.AttributeModifier).ToArray();

		var value = new Attribution()
		{
			MaxHp = GetProperty(a => a.MaxHp),
			MaxMp = GetProperty(a => a.MaxMp),
			MaxHpMul = GetProperty(a => a.MaxHpMul),
			MaxMpMul = GetProperty(a => a.MaxMpMul),
			HpRegSpd = GetProperty(a => a.HpRegSpd),
			MpRegSpd = GetProperty(a => a.MpRegSpd),
			AtkSpd = GetProperty(a => a.AtkSpd),
			MovSpd = GetProperty(a => a.MovSpd),
			Cri = GetProperty(a => a.Cri),
			CriDmg = GetProperty(a => a.CriDmg),

			ElementDatas = new Dictionary<string, ElementData>()
		};
		foreach (var element in Global.Elements.Select(e => e.Name))
		{
			value.ElementDatas.Add(element, new ElementData
			{
				Atk = GetProperty(a => a.ElementDatas.GetValueOrDefault(element).Atk),
				Def = GetProperty(a => a.ElementDatas.GetValueOrDefault(element).Def),
				AtkMul = GetProperty(a => a.ElementDatas.GetValueOrDefault(element).AtkMul),
				DefMul = GetProperty(a => a.ElementDatas.GetValueOrDefault(element).DefMul),
			});
		}
		var output = new TrueAttribution()
		{
			MaxHp = (int)(value.MaxHp * (1 + value.MaxHpMul)),
			MaxMp = (int)(value.MaxMp * (1 + value.MaxMpMul)),
			HpRegSpd = value.HpRegSpd,
			MpRegSpd = value.MpRegSpd,
			AtkSpd = value.AtkSpd,
			MovSpd = value.MovSpd,
			Cri = value.Cri,
			CriDmg = value.CriDmg,
			ElementDatas = new Dictionary<string, TrueElementData>()
		};
		foreach (var element in Global.Elements.Select(e => e.Name))
		{
			output.ElementDatas.Add(element, new TrueElementData()
			{
				Atk = (int)(value.ElementDatas.GetValueOrDefault(element).Atk *
					(1 + value.ElementDatas.GetValueOrDefault(element).AtkMul)),
				Def = (int)(value.ElementDatas.GetValueOrDefault(element).Def *
					(1 + value.ElementDatas.GetValueOrDefault(element).DefMul)),
			});
		}
		return output;

		T GetProperty<T>(Func<Attribution, T> selector) where T : IAdditionOperators<T, T, T> =>
			CalculateProperty(sourceAttribution, [equipmentAttributions, buffAttributions], selector);
	}

	private static T CalculateProperty<T>(
		Attribution source, IEnumerable<IEnumerable<Attribution>> modifiers, Func<Attribution, T> selector)
		where T : IAdditionOperators<T, T, T> =>
		selector(source) + modifiers.Select(attributes => attributes.Select(selector).Sum()).Sum();
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

public record ElementData
{
	public int Atk;
	public int Def;
	public float AtkMul;
	public float DefMul;
}

public record TrueElementData
{
	public int Atk;
	public int Def;
}