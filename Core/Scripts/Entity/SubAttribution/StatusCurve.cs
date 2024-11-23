using System;

namespace TheMage.Core.Scripts.Entity.SubAttribution;

public record StatusCurve
{

	public Func<(int value, int level), int> MaxHp { get; set; } = DefaultTransform.Linner(0.1f);
	public Func<(int value, int level), int> MaxMp { get; set; } = DefaultTransform.Linner(0.1f);

	public Func<(float value, int level), float> MaxHpMul { get; set; } = DefaultTransform.LinnerF(0.1f);

	public Func<(float value, int level), float> MaxMpMul { get; set; } = DefaultTransform.LinnerF(0.1f);
	public Func<(float value, int level), float> HpRegSpd { get; set; } = DefaultTransform.LinnerF(0.1f);
	public Func<(float value, int level), float> MpRegSpd { get; set; } = DefaultTransform.LinnerF(0.1f);
	public Func<(float value, int level), float> AtkSpd { get; set; } = DefaultTransform.ConstantF();
	public Func<(float value, int level), float> MovSpd { get; set; } = DefaultTransform.ConstantF();
	public Func<(float value, int level), float> Cri { get; set; } = DefaultTransform.LogF(0.1f);
	public Func<(float value, int level), float> CriDmg { get; set; } = DefaultTransform.LinnerF(0.1f);

	public Func<(int value, int level), int> Atk { get; set; } = DefaultTransform.Linner(0.1f);
	public Func<(float value, int level), float> AtkMul { get; set; } = DefaultTransform.LinnerF(0.1f);
	public Func<(int value, int level), int> Def { get; set; } = DefaultTransform.Linner(0.1f);
	public Func<(float value, int level), float> DefMul { get; set; } = DefaultTransform.LinnerF(0.1f);
	
	public static StatusCurve Default => field ??= new StatusCurve();

	public Attribution Transform(Attribution attribution, int level)
	{
		var output = new Attribution()
		{
			MaxHp = MaxHp((attribution.MaxHp, level)),
			MaxHpMul = MaxHpMul((attribution.MaxHpMul, level)),
			MaxMp = MaxMp((attribution.MaxMp, level)),
			MaxMpMul = MaxMpMul((attribution.MaxMpMul, level)),
			HpRegSpd = HpRegSpd((attribution.HpRegSpd, level)),
			MpRegSpd = MpRegSpd((attribution.MpRegSpd, level)),
			AtkSpd = AtkSpd((attribution.AtkSpd, level)),
			MovSpd = MovSpd((attribution.MovSpd, level)),
			Cri = Cri((attribution.Cri, level)),
			CriDmg = CriDmg((attribution.Cri, level)),
		};
		foreach (var elementData in attribution.ElementDataSet)
		{
			output.ElementDataSet.Add(elementData.Key,
				new ElementData()
				{
					Atk = Atk((elementData.Value.Atk, level)), AtkMul = AtkMul((elementData.Value.AtkMul, level)),
					Def = Def((elementData.Value.Def, level)), DefMul = DefMul((elementData.Value.DefMul, level)),
				});
		}
		return output;
	}


	public static class DefaultTransform
	{
		public static Func<(int value, int level), int> Linner(float slope = 1) => x =>
			x.value * (1 + (int)((x.level - 1) * slope));
		public static Func<(float value, int level), float> LinnerF(float slope = 1) => x => 
			x.value * (1 + (x.level - 1) * slope);
		
		public static Func<(int value, int level), int> Constant() => x => x.value;
		public static Func<(float value, int level), float> ConstantF() => x => x.value;
		
		public static Func<(int value, int level), int> Exp(float @base = 1) =>
			x => x.value * (int)MathF.Pow(@base, x.level - 1);
		public static Func<(float value, int level), float> ExpF(float @base = 1) =>
			x => x.value * MathF.Pow(@base, x.level - 1);

		public static Func<(int value, int level), int> Log(float @base = 1) =>
			x => x.value * (1 + (int)MathF.Log(@base, x.level));
		public static Func<(float value, int level), float> LogF(float @base = 1) =>
			x => x.value * (1 + MathF.Log(@base, x.level));
	}
}