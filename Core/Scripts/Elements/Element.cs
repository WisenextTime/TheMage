using System;
using System.Linq;
using System.Numerics;

using Godot;

using Tomlyn.Parsing;

namespace TheMage.Core.Scripts.Elements;

public enum Element
{
	Physics,
	Zero,
	Aether,
	Fire,
	Air,
	Water,
	Earth,
}

public static class ElementExtensions
{
	public const int ElementCount = 7;

	public static Color GetColor(this Element element) => element switch
	{
		Element.Physics => new Color(0x363636_ff),
		Element.Zero    => new Color(0xFFFFFF_ff),
		Element.Aether  => new Color(0x3A003A_ff),
		Element.Fire    => new Color(0xFF0000_ff),
		Element.Air     => new Color(0xCCCCCC_ff),
		Element.Water   => new Color(0x0000FF_ff),
		Element.Earth   => new Color(0x663311_ff),
		_               => throw new ArgumentOutOfRangeException(nameof(element), element, null)
	};

	public static float[,] ModifierMatrix { get; } = new[,]
	{
		/* EFFECT                                                          */
		/* TO↓ BY-> | PHY | ZRO | ATH | FIR | AIR | WTR | ETH ||           */
		/*Physics*/ { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }, /*Physics*/
		/*          |_PHT_|_____|_____|_____|_____|_____|_____||           */
		/*   Zero*/ { 1.0f, 1.0f, 0.5f, 1.2f, 1.2f, 1.2f, 1.2f }, /*Zero   */
		/*          |_____|_ZRO_|_____|_____|_____|_____|_____||           */
		/* Aether*/ { 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }, /*Aether */
		/*          |_____|_____|_ATH_|_____|_____|_____|_____||           */
		/*   Fire*/ { 1.0f, 1.2f, 1.0f, 1.0f, 1.5f, 0.5f, 1.2f }, /*Fire   */
		/*          |_____|_____|_____|_FRE_|_____|_____|_____||           */
		/*    Air*/ { 1.0f, 1.2f, 1.0f, 1.5f, 1.0f, 1.2f, 0.5f }, /*Air    */
		/*          |_____|_____|_____|_____|_AIR_|_____|_____||           */
		/*  Water*/ { 1.0f, 1.2f, 1.0f, 0.5f, 1.2f, 1.0f, 1.5f }, /*Water  */
		/*          |_____|_____|_____|_____|_____|_WTR_|_____||           */
		/*  Earth*/ { 1.0f, 1.2f, 1.0f, 1.2f, 0.5f, 1.5f, 1.0f }, /*Earth  */
		/*          |_____|_____|_____|_____|_____|_____|_ETH_||           */
		/*          | PHY | ZRO | ATH | FIR | AIR | WTR | ETH ||           */
	};

	public static ElementValues<float> ToModifier<TValue>(this ElementValues<TValue> modifiedBy) where TValue : IEquatable<TValue> =>
		ToModifier(modifiedBy, static v => !v.Equals(default(TValue)));

	public static ElementValues<float> ToModifier<TValue>(this ElementValues<TValue> modifiedBy, Func<TValue, bool> predicate)
	{
		ArgumentNullException.ThrowIfNull(predicate);
		return Enum.GetValues<Element>()
		           .Where(byElement => predicate(modifiedBy[byElement]))
		           .Select(static byElement => ElementValues.Create(element => ModifierMatrix[(int)element, (int)byElement]))
		           .Aggregate(ElementValues.CreateFilled(1f), static (values, by) => values.Multiple(by));
	}

	public static ElementValues<TValue> Add<TValue>(this ElementValues<TValue> self, ElementValues<TValue> other)
		where TValue : IAdditionOperators<TValue, TValue, TValue> =>
		self.CalculateWith(other, static (x, y) => x + y);

	public static ElementValues<TValue> Multiple<TValue>(this ElementValues<TValue> self, ElementValues<TValue> other)
		where TValue : IMultiplyOperators<TValue, TValue, TValue> =>
		self.CalculateWith(other, static (x, y) => x * y);

	public static ElementValues<TValue> Negated<TValue>(this ElementValues<TValue> self)
		where TValue : IUnaryNegationOperators<TValue, TValue> => self.With(static x => -x);
}