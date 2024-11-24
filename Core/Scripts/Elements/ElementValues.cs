using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using TheMage.Core.Extensions;

namespace TheMage.Core.Scripts.Elements;

public readonly struct ElementValues<TValue> : IReadOnlyDictionary<Element, TValue>
{
	private readonly TValue[] _values = new TValue[ElementExtensions.ElementCount];


	#region Creator

	public ElementValues(TValue[] values)
	{
		if (values.Length != ElementExtensions.ElementCount)
			throw new ArgumentOutOfRangeException("values should be of length " + ElementExtensions.ElementCount);

		_values = values;
	}

	public ElementValues(IEnumerable<KeyValuePair<Element, TValue>> source)
	{
		foreach (var (element, value) in source)
		{
			this[element] = value;
		}
	}

	public ElementValues<TValue> With(Dictionary<Element, TValue> change) => new(this.Concat(change));
	public ElementValues<TResult> With<TResult>(Func<TValue, TResult> selector) => new(Values.Select(selector).ToArray());

	#endregion


	#region Dictionary

	public IEnumerable<Element> Keys => Enum.GetValues<Element>();
	public IEnumerable<TValue> Values => _values.AsReadOnly();
	public TValue this[Element key]
	{
		get => _values[(int)key];
		init => _values[(int)key] = value;
	}

	public IEnumerator<KeyValuePair<Element, TValue>> GetEnumerator()
	{
		foreach (var element in Enum.GetValues<Element>())
		{
			yield return new KeyValuePair<Element, TValue>(element, _values[(int)element]);
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public int Count => ElementExtensions.ElementCount;

	public bool ContainsKey(Element key) => (int)key is >= 0 and < ElementExtensions.ElementCount;

	public bool TryGetValue(Element key, out TValue value)
	{
		if (!ContainsKey(key))
		{
			value = default;
			return false;
		}

		value = _values[(int)key];
		return true;
	}

	#endregion


	#region Value Properties

	public TValue Physics
	{
		get => this[Element.Physics];
		init => this[Element.Physics] = value;
	}
	public TValue Zero
	{
		get => this[Element.Zero];
		init => this[Element.Zero] = value;
	}
	public TValue Aether
	{
		get => this[Element.Aether];
		init => this[Element.Aether] = value;
	}
	public TValue Fire
	{
		get => this[Element.Fire];
		init => this[Element.Fire] = value;
	}
	public TValue Air
	{
		get => this[Element.Air];
		init => this[Element.Air] = value;
	}
	public TValue Water
	{
		get => this[Element.Water];
		init => this[Element.Water] = value;
	}
	public TValue Earth
	{
		get => this[Element.Earth];
		init => this[Element.Earth] = value;
	}

	#endregion


	#region Calculation

	public ElementValues<TResult> CalculateWith<TOther, TResult>(ElementValues<TOther> other, Func<TValue, TOther, TResult> calculation) =>
		new(_values.Zip(other._values, calculation).ToArray());

	#endregion
}

/// <summary>
/// Static factory methods for <see cref="ElementValues{TValue}"/>
/// </summary>
public static class ElementValues
{
	/// <summary>
	/// Creates a new <see cref="ElementValues{TValue}"/> with all elements filled with <paramref name="value"/>
	/// </summary>
	public static ElementValues<TValue> CreateFilled<TValue>(TValue value) => new(new TValue[ElementExtensions.ElementCount].Fill(value));

	/// <summary>
	/// Creates a new <see cref="ElementValues{TValue}"/> from an array
	/// </summary>
	/// <param name="values">
	/// Source array. Should be of length <see cref="ElementExtensions.ElementCount"/>.
	/// </param>
	public static ElementValues<TValue> Create<TValue>(TValue[] values) => new(values);

	/// <summary>
	/// Creates a new <see cref="ElementValues{TValue}"/> with a selector for each element
	/// </summary>
	public static ElementValues<TValue> Create<TValue>(Func<Element, TValue> selector) =>
		new(Enum.GetValues<Element>().Select(selector).ToArray());

	/// <summary>
	/// Creates a new <see cref="ElementValues{TValue}"/> from a <see cref="Element"/>-<see cref="TValue"/> dictionary.
	/// (or any collection of a KeyValuePair&lt;Element, TValue&gt;) 
	/// </summary>
	public static ElementValues<TValue> Create<TValue>(IEnumerable<KeyValuePair<Element, TValue>> source) => new(source);

	/// <summary>
	/// Creates a new <see cref="ElementValues{TValue}"/> from a collection of a KeyValuePair&lt;Element, TSource&gt; 
	/// and a selector from TSource to TValue
	/// </summary>
	public static ElementValues<TValue> Create<TSource, TValue>(IEnumerable<KeyValuePair<Element, TSource>> source,
	                                                            Func<TSource, TValue> selector) =>
		new(source.Select(x => new KeyValuePair<Element, TValue>(x.Key, selector(x.Value))));

	/// <summary>
	/// Creates a new <see cref="ElementValues{TValue}"/> from a collection of a KeyValuePair&lt;Element, TSource&gt;, 
	/// a extern data, and a selector from (TSource, TData) to TValue
	/// </summary>
	public static ElementValues<TValue> Create<TSource, TData, TValue>(IEnumerable<KeyValuePair<Element, TSource>> source,
	                                                                   TData data, Func<TSource, TData, TValue> selector) =>
		new(source.Select(x => new KeyValuePair<Element, TValue>(x.Key, selector(x.Value, data))));
}