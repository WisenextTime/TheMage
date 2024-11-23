using System;
using System.Collections.Generic;
using System.Linq;

namespace TheMage.Core.Scripts.Entity;

public class Damage(Entity source)
{
	public Entity Source { get; init; } = source;
	public Dictionary<string, (int damage, bool cri)> Damages { get; init; } = new();

	public Dictionary<string, (int damage, bool cri)> GetDamage(Entity target, float multiplier)
	{
		var atk = target.GetDamages();
		var def = target.GetDefenses();
		var output = new Dictionary<string, (int damage, bool cri)>();
		foreach (var element in Global.Elements.Select(e => e.Name))
		{
			output.Add(element,
				((int)(atk[element].damage * (1 - def[element] / MathF.Abs(def[element] + 600)) * multiplier),
					atk[element].crit));
		}
		return output;
	}
}