using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using TheMage.Core.Scripts.Entity.SubAttribution;
using TheMage.Core.Scripts.Game;
using TheMage.Core.Scripts.Items;
using Tomlyn;

namespace TheMage.Core.Scripts.Entity;

[GlobalClass]
public partial class Entity : CharacterBody2D
{
	[Export] public string Id;
	public int PhysicsLayer { get; set; }
	[Export(PropertyHint.Range, "-2,9")] public int Team { get; set; } = -1;
	
	public string EntityName { get; set; } = "";
	
	protected Controller Controller;
	public float InitHp = 1;
	public float InitMp = 1;

	public int Hp;
	public int Mp;
	
	public StatusCurve StatusCurve = StatusCurve.Default;
	[Export] public int Level = 1;
	
	public Attribution Attribution = new();
	public Dictionary<string,Equipment> Equipments = [];
	public List<Buff> Buffs = [];
	public Weapon MainWeapon
	{
		get
		{
			if (Equipments["Hand"] is Weapon weapon) return weapon;
			else return null;
		}
	}

	public TrueAttribution TrueAttribution => Attribution.GetEntityTrueAttribution(this);

	public Entity()
	{
		
	}
	
	public virtual void TakeDamage(Damage damage, float multiplier = 1)
	{
		var damages = damage.GetDamage(this, multiplier);
		Hp -= damages.Sum(d => d.Value.damage);
	}
	
	public Dictionary<string,(int damage,bool crit)> GetDamages()
	{
		var trueAttribution = TrueAttribution;
		var output = new Dictionary<string, (int damage, bool crit)>();
		foreach (var element in Global.Elements.Select(e => e.Name))
		{
			var cri = trueAttribution.Cri >= Random.Shared.NextSingle();
			output.Add(element,
				((int)(trueAttribution.ElementDatas.GetValueOrDefault(element).Atk * (1 + (cri ? trueAttribution.CriDmg : 0))),
					cri));
		}
		return output;
	}

	public Dictionary<string, int> GetDefenses()
	{
		var trueAttribution = TrueAttribution;
		var output = new Dictionary<string, int>();
		foreach (var element in Global.Elements.Select(e => e.Name))
		{
			output.Add(element, trueAttribution.ElementDatas.GetValueOrDefault(element).Def);
		}
		return output;
	}
}