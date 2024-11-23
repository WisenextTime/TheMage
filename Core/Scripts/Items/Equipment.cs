using TheMage.Core.Scripts.Entity.SubAttribution;

namespace TheMage.Core.Scripts.Items;

public class Equipment : Item
{
	public Attribution Attributions { get; set; } = new();
	public StatusCurve StatusCurves { get; set; } = StatusCurve.Default;
	
	public int Level { get; set; } = 1;
}