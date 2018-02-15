namespace Cogs.Designer
{
	public class Guideline
	{
		public double Position { get; set; }
		public GuideLineOrientation Orientation { get; set; }
		public GuidelineType Type { get; set; }
		public bool IsActive { get; set; }
	}

	public enum GuideLineOrientation
	{
		Vertical,
		Horizontal
	}

	public enum GuidelineType
	{
		Edge,
		Center
	}
}
