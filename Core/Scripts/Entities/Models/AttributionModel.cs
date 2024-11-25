using System.Collections.Generic;

using TheMage.Core.Scripts.Entities.SubAttribution;

namespace TheMage.Core.Scripts.Entities.Models;

public record AttributionModel
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

	public Dictionary<string, ElementModel> Elements { get; set; } = new();
}

public record ElementModel
{
	public int Atk { get; set; }
	public int Def { get; set; }
	public int AtkMul { get; set; }
	public int DefMul { get; set; }
}