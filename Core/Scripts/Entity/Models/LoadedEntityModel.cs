using System.Collections.Generic;

namespace TheMage.Core.Scripts.Entity.Models;

public record LoadedEntityModel
{
	public string Source { get; set; }
	public int Level { get; set; }
	public int Team { get; set; }
	public int Hp { get; set; }
	public int Mp { get; set; }
	
	public Dictionary<string,string> Equipments { get; set; }
	public List<string> Buffs { get; set; }
}