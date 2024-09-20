using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LiveSplit.RedFaction
{
	public class Mod : ICloneable
	{
		[XmlAttribute] public string ModName;
        [XmlAttribute] public string FirstLevel;
        [XmlArrayItem] public List<SplitStructOverall> Splits;

		public Mod()
		{
			ModName = "";
			FirstLevel = "";
			Splits = null;
		}

		public Mod(string ModName, string FirstLevel, List<SplitStructOverall> Splits)
		{
			this.ModName = ModName;
			this.FirstLevel = FirstLevel;
			this.Splits = Splits;
		}

		public object Clone()
		{
			var newSplits = new List<SplitStructOverall>();
			for(int i=0; i<this.Splits.Count; i++)
			{
				newSplits.Add(this.Splits[i].Clone());
			}
			
			return new Mod(this.ModName, this.FirstLevel, newSplits);
		}

		public override string ToString() => ModName;
	}

	[Serializable]
	public abstract class SplitStructOverall
	{
		[XmlAttribute] public bool Split { get; set; }
		[XmlAttribute] public string Name { get; set; }

		public SplitStructOverall()
		{
			Split = false;
			Name = "";
		}

		public abstract SplitStructOverall Clone();

		internal abstract bool Check(in string levelName, in string prevLevelName, in bool isMoviePlaying);
	}

	[Serializable]
	public class SplitLevelChange : SplitStructOverall
	{
		[XmlIgnore] public string PreviousLevelName;
		[XmlIgnore] public string CurrentLevelName;

		public SplitLevelChange()
		{
			Split = false;
			Name = "";
			PreviousLevelName = "";
			CurrentLevelName = "";
		}

		public SplitLevelChange(string Name, string PreviousLevelName, string CurrentLevelName)
		{
			this.Split = true;
			this.Name = Name;
			this.PreviousLevelName = PreviousLevelName;
			this.CurrentLevelName = CurrentLevelName;
		}

		public override SplitStructOverall Clone()
		{
			return new SplitLevelChange() { Split = this.Split, Name = this.Name, PreviousLevelName = this.PreviousLevelName, CurrentLevelName = this.CurrentLevelName };
		}

		internal override bool Check(in string levelName, in string prevLevelName, in bool isMoviePlaying)
		{
			if (this.Split && this.CurrentLevelName == levelName && this.PreviousLevelName == prevLevelName)
				return true;
			else
				return false;
		}
	}

	[Serializable]
	public class SplitVideoPlays : SplitStructOverall
	{
		[XmlIgnore] public string CurrentLevelName;

		public SplitVideoPlays()
		{
			Split = false;
			Name = "";
			CurrentLevelName = "";
		}

		public SplitVideoPlays(string Name, string CurrentLevelName)
		{
			this.Split = true;
			this.Name = Name;
			this.CurrentLevelName = CurrentLevelName;
		}

		public override SplitStructOverall Clone()
		{
			return new SplitVideoPlays() { Split = this.Split, Name = this.Name, CurrentLevelName = this.CurrentLevelName };
		}

		internal override bool Check(in string levelName, in string prevLevelName, in bool isMoviePlaying)
		{
			if (this.Split && isMoviePlaying && this.CurrentLevelName == levelName)
				return true;
			else
				return false;
		}
	}
}
