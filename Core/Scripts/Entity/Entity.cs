using System;
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
	public Dictionary<EquipSlot, Equipment> Equipments = [];
	public List<Buff> Buffs = [];
	
	public ElementValues<bool> AttachedElements => throw new NotImplementedException();

	public TrueAttribution TrueAttribution => Attribution.GetEntityTrueAttribution(this);

	public virtual void TakeDamage(Damage damage, float multiplier = 1)
	{
		var damages = damage.GetDamage(this, multiplier);
		Hp -= damages.Sum(d => d.Value.damage);
	}

	public ElementValues<(int damage, bool crit)> GetAttackDamages()
	{
		return ElementValues.Create(TrueAttribution.ElementDataSet, TrueAttribution, static (data, attribution) =>
		{
			var cri = attribution.Cri >= Random.Shared.NextSingle();
			var damage = (int)(data.Atk * (1 + (cri ? attribution.CriDmg : 0)));
			return (damage, cri);
		});
	}

	public ElementValues<int> GetDefenses() => ElementValues.Create(TrueAttribution.ElementDataSet, static data => data.Def);
}