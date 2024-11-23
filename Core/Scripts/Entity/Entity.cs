using System;
using System.Collections.Generic;
using Godot;
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
	
	public StatusCurve StatusCurve = SubAttribution.StatusCurve.Default;
	[Export] public int Level = 1;
	
	public Attribution Attribution = new();
	public Dictionary<string,Equipment> Equipments = [];
	public List<Buff> Buffs = [];

	public virtual void TakeDamage(Damage damage) { }
	
	public Dictionary<string,(int damage,bool crit)> GetDamages()
	{
		throw new NotImplementedException();
	}

	public Dictionary<string, int> GetDefenses()
	{
		throw new NotImplementedException();
	}
}