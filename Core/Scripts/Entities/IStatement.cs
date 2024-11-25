namespace TheMage.Core.Scripts.Entities;

public interface IStatement
{
	public void AttachTo(Entity entity);
	public void RemoveFrom(Entity entity);

	//可以这样进行一个多态的扩展:
	// public void AttachTo(InheritedEntity entity) => AttachTo((Entity)entity);
	// public void RemoveFrom(InheritedEntity entity) => RemoveFrom((Entity)entity);
}