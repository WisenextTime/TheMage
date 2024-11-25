using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Godot;

using TheMage.Core.Extensions;
using TheMage.Core.Scripts.Elements;
using TheMage.Core.Scripts.Entities.Buffs;
using TheMage.Core.Scripts.Entities.SubAttribution;
using TheMage.Core.Scripts.Game;
using TheMage.Core.Scripts.Items;

namespace TheMage.Core.Scripts.Entities;

[GlobalClass]
public partial class Entity : CharacterBody2D
{
	[Export] public string Id { get; set; }
	public int PhysicsLayer { get; set; }

	[Export(PropertyHint.Range, "-2,9")]
	public int Team { get; set; } = -1;

	protected Controller Controller;
	public float InitHp { get; set; } = 1;
	public float InitMp { get; set; } = 1;

	public int Hp { get; set; }
	public int Mp { get; set; }

	public StatusCurve StatusCurve { get; protected set; } = StatusCurve.Default;
	[Export] public int Level { get; set; } = 1;

	public Attribution Attribution { get; protected set; } = new();
	public List<Attribution> AttributionModifiers { get; } = [];

	private readonly Dictionary<EquipSlot, Equipment> _equipments = [];
	public IReadOnlyDictionary<EquipSlot, Equipment> Equipments => _equipments.AsReadOnly();

	private readonly List<Buff> _buffs = [];
	public IReadOnlyList<Buff> Buffs => _buffs.AsReadOnly();

	public ElementValues<bool> AttachedElements => throw new NotImplementedException(); //你自己实现吧，嘿嘿

	public FinalAttribution FinalAttribution => (Attribution + AttributionModifiers.Sum()).ToFinal();


	#region Damage

	public virtual void TakeDamage(Damage damage, float multiplier = 1)
	{
		var damages = damage.GetDamage(this, multiplier);
		Hp -= damages.Sum(d => d.Value.damage);
	}

	public ElementValues<(int damage, bool crit)> GetAttackDamages()
	{
		return ElementValues.Create(FinalAttribution.ElementDataSet, FinalAttribution, static (data, attribution) =>
		{
			var cri = attribution.Cri >= Random.Shared.NextSingle();
			var damage = (int)(data.Atk * (1 + (cri ? attribution.CriDmg : 0)));
			return (damage, cri);
		});
	}

	public ElementValues<int> GetDefenses() => ElementValues.Create(FinalAttribution.ElementDataSet, static data => data.Def);

	#endregion


	#region Statement (buffs, equipments...)

	//用来实现双分派，需要多态时候override两个一样的
	//（感觉不一定用得到，不过也不碍事）
	protected virtual void Acquire(IStatement statement) => statement.AttachTo(this);
	protected virtual void Lose(IStatement statement) => statement.RemoveFrom(this);

	public void ReplaceEquipment(EquipSlot slot, [AllowNull] Equipment equipment) //感觉我还是不太习惯不开nullable
	{
		if (_equipments.TryGetValue(slot, out var old)) Lose(old);
		_equipments[slot] = equipment;
		if (equipment is not null) Acquire(equipment);
	}

	public void AddBuff(Buff buff, double time)
	{
		_buffs.Add(buff);
		Acquire(buff);
		buff.TimeRemain = time;
	}

	public void RemoveBuff(Buff buff)
	{
		if (_buffs.Remove(buff))
			Lose(buff);
	}

	#endregion


	#region Events

	public event Action<double> ProcessUpdate;

	#endregion
}