using System.Collections.Generic;

using TheMage.Core.Scripts.Data;

namespace TheMage.Core.Scripts.Entities.Models;

public record EntityModel : IModel<LoadedEntityModel>
{
	public string Name { get; init; }

	public int Level { get; set; }
	public int InitTeam { get; set; } //todo: 喵？

	public Dictionary<string, string> InitEquipments { get; set; }
	public List<string> InitBuffs { get; set; }

	public AttributionModel Attribution { get; init; }
	public StatusCurveModel StatusCurve { get; init; }
	
	public LoadedEntityModel Convert()
	{
		throw new System.NotImplementedException();
	}
}