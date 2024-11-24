using System;
using System.Collections.Generic;
using System.Linq;

using TheMage.Core.Scripts.Elements;

namespace TheMage.Core.Scripts.Entity;

public class Damage(Entity source)
{
	public Entity Source { get; init; } = source;
	public ElementValues<(int damage, bool cri)> Damages { get; init; } = new();

	public ElementValues<(int damage, bool cri)> GetDamage(Entity target, float multiplier) =>
		Source.GetDamages().CalculateWith(target.GetDefenses(), (atk, def) =>
		{
			var dmg = (int)(atk.damage * (1 - def / MathF.Abs(def + 600)) * multiplier);
			return (damage: dmg, cri: atk.crit);
		});
}