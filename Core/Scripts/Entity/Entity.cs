﻿using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

using TheMage.Core.Scripts.Elements;
using TheMage.Core.Scripts.Entity.SubAttribution;
using TheMage.Core.Scripts.Game;
using TheMage.Core.Scripts.Items;

namespace TheMage.Core.Scripts.Entity;

[GlobalClass]
public partial class Entity : CharacterBody2D
{
	[Export] public string Id;
	public int PhysicsLayer { get; set; }
	[Export(PropertyHint.Range, "-2,9")] public int Team { get; set; } = -1;
	
	protected Controller Controller;
	public float InitHp = 1;
	public float InitMp = 1;

	public int Hp;
	public int Mp;
	
	public StatusCurve StatusCurve = SubAttribution.StatusCurve.Default;
	[Export] public int Level = 1;
	
	public Attribution Attribution = new();
	public Dictionary<string,Equipment> Equipments = [];
	public List<Buff> Buffs = [];
	
	public TrueAttribution TrueAttribution => Attribution.GetEntityTrueAttribution(this);

	public virtual void TakeDamage(Damage damage, float multiplier = 1)
	{
		var damages = damage.GetDamage(this, multiplier);
		Hp -= damages.Sum(d => d.Value.damage);
	}
	
	public Dictionary<Element,(int damage,bool crit)> GetDamages()
	{
		var trueAttribution = TrueAttribution;
		var output = new Dictionary<Element, (int damage, bool crit)>();
		foreach (var element in Global.Elements)
		{
			var cri = trueAttribution.Cri >= Random.Shared.NextSingle();
			output.Add(element,
				((int)(trueAttribution.ElementDataSet.GetValueOrDefault(element).Atk * (1 + (cri ? trueAttribution.CriDmg : 0))),
					cri));
		}
		return output;
	}

	public Dictionary<Element, int> GetDefenses()
	{
		var trueAttribution = TrueAttribution;
		var output = new Dictionary<Element, int>();
		foreach (var element in Global.Elements)
		{
			output.Add(element, trueAttribution.ElementDataSet.GetValueOrDefault(element).Def);
		}
		return output;
	}
}