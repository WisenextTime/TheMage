namespace TheMage.Core.Scripts.Items;

public class Item
{
	public string Id { get; set; }
	
	public string Name { get; set; }
	public bool CanBeUsed { get; set; } = false;
	public bool CanBeSold { get; set; } = true;
	public bool CanBeTrashed { get; set; } = true;
}