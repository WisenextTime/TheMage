using TheMage.Core.Scripts.Entities;
using TheMage.Core.Scripts.Entities.SubAttribution;

namespace TheMage.Core.Scripts.Items;

public class Equipment : Item, IStatement
{
	public Attribution Attributions
	{
		get;
		set
		{
			field = value;
			UpdateFinalAttributions();
		}
	} = new();
	public StatusCurve StatusCurves
	{
		get;
		set
		{
			field = value;
			UpdateFinalAttributions();
		}
	} = StatusCurve.Default;

	public int Level
	{
		get;
		set
		{
			field = value;
			UpdateFinalAttributions();
		}
	} = 1;

	public Attribution FinalAttributions
	{
		get;
		private set
		{
			if (field == value) return;
			var before = field;
			field = value;
			AttributionUpdated(before, field);
		}
	}

	private void UpdateFinalAttributions() => FinalAttributions = StatusCurves.Transform(Attributions, Level);

	protected Entity EquippedEntity { get; private set; }

	void IStatement.AttachTo(Entity entity)
	{
		if (EquippedEntity is not null || entity is null) return;
		EquippedEntity = entity;
		EquippedEntity.AttributionModifiers.Add(FinalAttributions);
		OnEquipped();
	}

	void IStatement.RemoveFrom(Entity entity)
	{
		if (!ReferenceEquals(EquippedEntity, entity) || entity is null) return;
		OnUnequipped();
		EquippedEntity.AttributionModifiers.Remove(FinalAttributions);
		EquippedEntity = null;
	}

	protected virtual void OnEquipped() { }

	protected virtual void OnUnequipped() { }

	private void AttributionUpdated(Attribution beforeFinal, Attribution afterFinal)
	{
		if (EquippedEntity is null) return;
		EquippedEntity.AttributionModifiers.Remove(beforeFinal);
		EquippedEntity.AttributionModifiers.Add(afterFinal);
	}
}