using Godot;
using TheMage.Core.Scripts.Entity.SubAttribution;

namespace TheMage.Core.Scripts.Entity;

public partial class Buff : Node
{
	public Attribution AttributeModifier  { get; set; }
}