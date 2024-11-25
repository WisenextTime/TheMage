using System.Collections.Generic;

using TheMage.Core.Scripts.Data;

namespace TheMage.Core.Scripts.Entities.Models;

public record LoadedEntityModel:IModel<Entity>
{
	public string Source { get; init; }
	public int Level { get; init; }
	public int Team { get; init; }
	public int Hp { get; init; }
	public int Mp { get; init; }
	
	public Dictionary<string,string> Equipments { get; init; }
	public List<string> Buffs { get; init; }
	public Entity Convert()
	{
		throw new System.NotImplementedException();
	}
}