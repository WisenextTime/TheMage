using System.Collections.Generic;

namespace TheMage.Core.Scripts.Entity;

public class Damage(Entity source)
{
	public Entity Source { get; init; } = source;
	public Dictionary<string, (int damage, bool cri)> Damages { get; init; } = new();
}