using System.Collections.Generic;
using System.Linq;

namespace TheMage.Core.Scripts.Entity.SubAttribution;

public record Attribution
{
	public int MaxHp { get; set; }
	public int MaxMp { get; set; }

	public float MaxHpMul { get; set; }
	
	public float MaxMpMul { get; set; }
	public float HpRegSpd  { get; set; }
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
		var souceAttribution = entity.StatusCurve.Transform(entity.Attribution, entity.Level);
		var equipmentAttribution = entity.Equipments.Select(e => e.Value)
			.Select(e => e.StatusCurves.Transform(e.Attributions, e.Level)).ToArray();
		var buffAttribution = entity.Buffs.Select(b => b.AttributeModifier).ToArray();
		var value = new Attribution()
		{
			MaxHp = souceAttribution.MaxHp + equipmentAttribution.Select(e => e.MaxHp).Sum() +
			        buffAttribution.Select(b => b.MaxHp).Sum(),
			MaxMp = souceAttribution.MaxMp + equipmentAttribution.Select(e => e.MaxMp).Sum() +
			        buffAttribution.Select(b => b.MaxMp).Sum(),
			MaxHpMul = souceAttribution.MaxHpMul + equipmentAttribution.Select(e => e.MaxHpMul).Sum() +
			           buffAttribution.Select(b => b.MaxHpMul).Sum(),
			MaxMpMul = souceAttribution.MaxMpMul + equipmentAttribution.Select(e => e.MaxMpMul).Sum() +
			           buffAttribution.Select(b => b.MaxMpMul).Sum(),
			HpRegSpd = souceAttribution.HpRegSpd + equipmentAttribution.Select(e => e.HpRegSpd).Sum() +
			           buffAttribution.Select(b => b.HpRegSpd).Sum(),
			MpRegSpd = souceAttribution.MpRegSpd + equipmentAttribution.Select(e => e.MpRegSpd).Sum() +
			           buffAttribution.Select(b => b.MpRegSpd).Sum(),
			AtkSpd = souceAttribution.AtkSpd + equipmentAttribution.Select(e => e.AtkSpd).Sum() +
			         buffAttribution.Select(b => b.AtkSpd).Sum(),
			MovSpd = souceAttribution.MovSpd + equipmentAttribution.Select(e => e.MovSpd).Sum() +
			         buffAttribution.Select(b => b.MovSpd).Sum(),
			Cri = souceAttribution.Cri + equipmentAttribution.Select(e => e.Cri).Sum() +
			      buffAttribution.Select(b => b.Cri).Sum(),
			CriDmg = souceAttribution.CriDmg + equipmentAttribution.Select(e => e.CriDmg).Sum() +
			         buffAttribution.Select(b => b.CriDmg).Sum(),
			ElementDatas = new Dictionary<string, ElementData>()
		};
		foreach (var element in Global.Elements.Select(e => e.Name))
		{
			value.ElementDatas.Add(element, new ElementData
			{
				Atk = souceAttribution.ElementDatas.GetValueOrDefault(element).Atk +
				      equipmentAttribution.Select(e => e.ElementDatas.GetValueOrDefault(element).Atk).Sum() +
				      buffAttribution.Select(e => e.ElementDatas.GetValueOrDefault(element).Atk).Sum(),
				Def = souceAttribution.ElementDatas.GetValueOrDefault(element).Def +
				      equipmentAttribution.Select(e => e.ElementDatas.GetValueOrDefault(element).Def).Sum() +
				      buffAttribution.Select(e => e.ElementDatas.GetValueOrDefault(element).Def).Sum(),
				AtkMul = souceAttribution.ElementDatas.GetValueOrDefault(element).AtkMul +
				         equipmentAttribution.Select(e => e.ElementDatas.GetValueOrDefault(element).AtkMul).Sum() +
				         buffAttribution.Select(e => e.ElementDatas.GetValueOrDefault(element).AtkMul).Sum(),
				DefMul = souceAttribution.ElementDatas.GetValueOrDefault(element).DefMul +
				         equipmentAttribution.Select(e => e.ElementDatas.GetValueOrDefault(element).DefMul).Sum() +
				         buffAttribution.Select(e => e.ElementDatas.GetValueOrDefault(element).DefMul).Sum(),
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
	}
}

public record TrueAttribution
{
	public int MaxHp { get; set; }
	public int MaxMp { get; set; }
	public float HpRegSpd  { get; set; }
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