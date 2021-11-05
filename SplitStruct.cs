using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LiveSplit.RedFaction
{
	public class Mod : ICloneable
	{
		[XmlAttribute] public string ModName;
		[XmlArrayItem] public List<SplitStructOverall> Splits;

		public Mod()
		{
			ModName = "";
			Splits = null;
		}

		public Mod(string ModName, List<SplitStructOverall> Splits)
		{
			this.ModName = ModName;
			this.Splits = Splits;
		}

		public object Clone()
		{
			var newSplits = new List<SplitStructOverall>();
			for(int i=0; i<this.Splits.Count; i++)
			{
				newSplits.Add((SplitStructOverall)this.Splits[i].Clone());
			}
			
			return new Mod(this.ModName, newSplits);
		}

		public override string ToString() => ModName;
	}

	[Serializable]
	public class SplitStructOverall : ICloneable
	{
		[XmlAttribute] public bool Split { get; set; }
		[XmlAttribute] public string Name { get; set; }

		public SplitStructOverall()
		{
			Split = false;
			Name = "";
		}

		public object Clone()
		{
			return new SplitStructOverall() { Split = this.Split, Name = this.Name };
		}

		internal virtual bool Check(in string levelName, in string prevLevelName, in bool isMoviePlaying)
		{
			return false;
		}
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

		internal override bool Check(in string levelName, in string prevLevelName, in bool isMoviePlaying)
		{
			if (this.CurrentLevelName == levelName && this.PreviousLevelName == prevLevelName)
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

		internal override bool Check(in string levelName, in string prevLevelName, in bool isMoviePlaying)
		{
			if (isMoviePlaying && this.CurrentLevelName == levelName)
				return true;
			else
				return false;
		}
	}
}
