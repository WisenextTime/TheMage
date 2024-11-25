using System;

namespace TheMage.Core.Scripts.Entities.SubAttribution;

public record StatusCurve
{
	public delegate T Curve<T>(T value, int level);

	public Curve<int> MaxHp { get; set; } = DefaultTransform.Linear(0.1f);
	public Curve<int> MaxMp { get; set; } = DefaultTransform.Linear(0.1f);

	public Curve<float> MaxHpMul { get; set; } = DefaultTransform.LinearF(0.1f);
	public Curve<float> MaxMpMul { get; set; } = DefaultTransform.LinearF(0.1f);
	public Curve<float> HpRegSpd { get; set; } = DefaultTransform.LinearF(0.1f);
	public Curve<float> MpRegSpd { get; set; } = DefaultTransform.LinearF(0.1f);
	public Curve<float> AtkSpd { get; set; } = DefaultTransform.ConstantF();
	public Curve<float> MovSpd { get; set; } = DefaultTransform.ConstantF();
	public Curve<float> Cri { get; set; } = DefaultTransform.LogF(0.1f);
	public Curve<float> CriDmg { get; set; } = DefaultTransform.LinearF(0.1f);

	public Curve<int> Atk { get; set; } = DefaultTransform.Linear(0.1f);
	public Curve<float> AtkMul { get; set; } = DefaultTransform.LinearF(0.1f);
	public Curve<int> Def { get; set; } = DefaultTransform.Linear(0.1f);
	public Curve<float> DefMul { get; set; } = DefaultTransform.LinearF(0.1f);

	public static StatusCurve Default => field ??= new StatusCurve();

	public Attribution Transform(Attribution attribution, int level) => new()
	{
		MaxHp = MaxHp(attribution.MaxHp, level),
		MaxHpMul = MaxHpMul(attribution.MaxHpMul, level),
		MaxMp = MaxMp(attribution.MaxMp, level),
		MaxMpMul = MaxMpMul(attribution.MaxMpMul, level),
		HpRegSpd = HpRegSpd(attribution.HpRegSpd, level),
		MpRegSpd = MpRegSpd(attribution.MpRegSpd, level),
		AtkSpd = AtkSpd(attribution.AtkSpd, level),
		MovSpd = MovSpd(attribution.MovSpd, level),
		Cri = Cri(attribution.Cri, level),
		CriDmg = CriDmg(attribution.Cri, level),
		ElementDataSet = attribution.ElementDataSet.With(data => new ElementData
		{
			Atk = Atk(data.Atk, level), AtkMul = AtkMul(data.AtkMul, level),
			Def = Def(data.Def, level), DefMul = DefMul(data.DefMul, level),
		})
	};


	public static class DefaultTransform
	{
		public static Curve<int> Linear(float slope = 1) =>
			(value, level) => value * (1 + (int)((level - 1) * slope));

		public static Curve<float> LinearF(float slope = 1) =>
			(value, level) => value * (1 + (level - 1) * slope);

		public static Curve<int> Constant() => (value, _) => value;

		public static Curve<float> ConstantF() => (value, _) => value;

		public static Curve<int> Exp(float @base = 1) =>
			(value, level) => value * (int)MathF.Pow(@base, level - 1);

		public static Curve<float> ExpF(float @base = 1) =>
			(value, level) => value * MathF.Pow(@base, level - 1);

		public static Curve<int> Log(float @base = 1) =>
			(value, level) => value * (1 + (int)MathF.Log(@base, level));

		public static Curve<float> LogF(float @base = 1) =>
			(value, level) => value * (1 + MathF.Log(@base, level));
	}
}