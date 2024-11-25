namespace TheMage.Core.Scripts.Entities.Buffs;

public abstract class Buff : IStatement
{
	protected Entity AttachedEntity { get; private set; }
	public double TimeRemain { get; set; }

	void IStatement.AttachTo(Entity entity)
	{
		if (AttachedEntity is not null || entity is null) return;

		AttachedEntity = entity;
		entity.ProcessUpdate += ProcessUpdate;
		OnAttached();
	}

	void IStatement.RemoveFrom(Entity entity)
	{
		if (!ReferenceEquals(AttachedEntity, entity) || entity is null) return;

		OnRemoved();
		entity.ProcessUpdate -= ProcessUpdate;
		AttachedEntity = null;
	}

	private void ProcessUpdate(double delta)
	{
		OnProcess();

		if (TimeRemain < 0) return; //时间为负则无限持续

		TimeRemain -= delta;
		if (TimeRemain <= 0) AttachedEntity.RemoveBuff(this);
	}

	protected virtual void OnAttached() { }
	protected virtual void OnProcess() { }
	protected virtual void OnRemoved() { }
}