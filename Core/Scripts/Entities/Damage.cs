using System;

using TheMage.Core.Scripts.Elements;

namespace TheMage.Core.Scripts.Entities;

public class Damage(Entity source)
{
	public Entity Source { get; init; } = source;
	public ElementValues<(int damage, bool cri)> Damages { get; init; } = new();

	public ElementValues<(int damage, bool cri)> GetDamage(Entity target, float multiplier)
	{
		var defended = Source.GetAttackDamages().CalculateWith(target.GetDefenses(), (atk, def) =>
		{
			var dmg = (int)(atk.damage * (1 - def / MathF.Abs(def + 600)) * multiplier);
			return (damage: dmg, cri: atk.crit);
		});
		var modifierValues = defended.CombineWith(target.AttachedElements)
		                             .ToModifier(static b => b.Item2 || b.Item1.damage > 0);
		return defended.CalculateWith(modifierValues, (dmg, modifier) => dmg with
		{
			damage = (int)(dmg.damage * modifier)
		});
	}
}