using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LiveSplit.RedFaction
{
	public class Mod
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
	}

	[Serializable]
	public class SplitStructOverall
	{
		[XmlAttribute] public bool Split;
		[XmlAttribute] public string Name;

		public SplitStructOverall()
		{
			throw new Exception("Don't use that!");
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
	}
}
