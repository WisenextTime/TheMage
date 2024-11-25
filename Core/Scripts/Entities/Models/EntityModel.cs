using System.Collections.Generic;

namespace TheMage.Core.Scripts.Entities.Models;

public record EntityModel
{
	public string Name { get; init; }
	
	public int Level { get; set; }
	public int InitTeam { get; set; }
	
	public Dictionary<string,string> InitEquipments { get; set; }
	public List<string> InitBuffs { get; set; }
	
	public AttributionModel Attribution { get; init; }
	public StatusCurveModel StatusCurve { get; init; }
}