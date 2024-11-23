using Godot;

namespace TheMage.Core.Scripts.Items;

public partial class Item : Node
{
	public string Id { get; set; }
	
	public string ItemName { get; set; }
	public bool CanBeUsed { get; set; } = false;
	public bool CanBeSold { get; set; } = true;
	public bool CanBeTrashed { get; set; } = true;
}