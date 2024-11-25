using TheMage.Core.Scripts.Entities.SubAttribution;

namespace TheMage.Core.Scripts.Entities.Buffs;

public class AttributionBuff(Attribution attributeModifier) : Buff
{
	public Attribution AttributeModifier { get; } = attributeModifier;

	protected override void OnAttached()
	{
		AttachedEntity.AttributionModifiers.Add(AttributeModifier);
	}

	protected override void OnRemoved()
	{
		AttachedEntity.AttributionModifiers.Remove(AttributeModifier);
	}
}