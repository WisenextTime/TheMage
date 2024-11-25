namespace TheMage.Core.Scripts.Data;

public interface IModel<out T>
{
	T Convert();
}