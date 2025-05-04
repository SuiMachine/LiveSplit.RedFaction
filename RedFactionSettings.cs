﻿using LiveSplit.Web.Share;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.RedFaction
{
	public partial class RedFactionSettings : UserControl
	{
		public bool AutoReset { get; set; }
		public bool AutoStart { get; set; }
		public int ModIndex { get; set; }

		private const bool DEFAULT_AUTORESET = false;
		private const bool DEFAULT_AUTOSTART = true;
		private const int DEFAULT_MODINDEX = 0;

		private static readonly Mod[] DEFAULT_MODS = new Mod[]
		{
			new Mod("Base Game", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Chapter 1 (Mines)", "l1s3.rfl", "l2s1.rfl"),
				new SplitLevelChange("Chapter 2 (Barracks)", "l2s3.rfl", "l3s1.rfl"),
				new SplitLevelChange("Chapter 3 (Reception && Docks)", "l3s4.rfl", "l4s1a.rfl"),
				new SplitLevelChange("Chapter 4 (Ventilation)", "l4s4.rfl", "l5s1.rfl"),
				new SplitLevelChange("Chapter 5 (Geothermal Plant)", "l5s4.rfl", "l6s1.rfl"),
				new SplitLevelChange("Chapter 6 (Administration)", "l6s3.rfl", "l7s1.rfl"),
				new SplitLevelChange("Chapter 7 (Backstage)", "l7s4.rfl", "l8s1.rfl"),
				new SplitLevelChange("Chapter 8 (Medical Labs)", "l8s4.rfl", "l9s1.rfl"),
				new SplitLevelChange("Chapter 9 (Caves)", "l9s4.rfl", "l10s1.rfl"),
				new SplitLevelChange("Chapter 10 (The \"Zoo\")", "l10s4.rfl", "l11s1.rfl"),
				new SplitLevelChange("Chapter 11 (Capek\\'s Secret Facility)", "l11s3.rfl", "l12s1.rfl"),
				new SplitLevelChange("Chapter 12 (Canion)", "l12s1.rfl", "l13s1.rfl"),
				new SplitLevelChange("Chapter 13 (Satelite Control)", "l13s3.rfl", "l14s1.rfl"),
				new SplitLevelChange("Chapter 14 (Missile Command Center)", "l14s3.rfl", "l15s1.rfl"),
				new SplitLevelChange("Chapter 15 (Catch a Shuttle)", "l15s4.rfl", "l17s1.rfl"), //Chapter 16 missing, don't panic
				new SplitLevelChange("Chapter 16 (Space Station)", "l17s4.rfl", "l18s1.rfl"),
				new SplitLevelChange("Chapter 17 (Back on Mars)", "l18s3.rfl", "l19s1.rfl"),
				new SplitLevelChange("Chapter 18 (Merc\\'s Base)", "l19s3.rfl", "l20s1.rfl"),
				new SplitLevelChange("Chapter 19 (Finale)", "l20s2.rfl", "l20s3.rfl"),
				new SplitVideoPlays("A Bomb!", "l20s3.rfl")
			}),
			// issue with sou currently, autostart works, splits do not
            new Mod("Soldier of Ultor", "souL1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Prison", "souL1.rfl", "souL2.rfl"),
				new SplitLevelChange("Inside", "souL2.rfl", "souL3.rfl"),
				new SplitLevelChange("Reactor", "souL3.rfl", "souL4.rfl"),
				new SplitLevelChange("Escape", "souL4.rfl", "souL5.rfl"),
				new SplitLevelChange("Tunnel", "souL5.rfl", "souL6.rfl"),
				new SplitLevelChange("Tunnel Out", "souL6.rfl", "souL7.rfl"),
				new SplitLevelChange("Pod", "souL7.rfl", "souL8.rfl"),
				new SplitLevelChange("Lab", "souL8.rfl", "souL9.rfl"),
				new SplitLevelChange("Corridor and Guns", "souL9.rfl", "souL10.rfl"),
				new SplitLevelChange("Power", "souL10.rfl", "souL11.rfl"),
				new SplitLevelChange("Air and Kill", "souL11.rfl", "souCS2.rfl")
			}),
			new Mod("Kava", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Tram Station", "l1s1.rfl", "rfrev_kva00b.rfl"),
				new SplitLevelChange("Surface of the Red Planet", "rfrev_kva00b.rfl", "rfrev_kva00c.rfl"),
				new SplitLevelChange("Generator Room", "rfrev_kva00c.rfl", "rfrev_kva00d.rfl"),
				new SplitLevelChange("Power Plant", "rfrev_kva00d.rfl", "rfrev_kva00e.rfl"),
				new SplitLevelChange("Research Facility", "rfrev_kva00e.rfl", "rfrev_kva00f.rfl"),
				new SplitLevelChange("Solar Array", "rfrev_kva00f.rfl", "rfrev_kva01.rfl"),
				new SplitLevelChange("Storage Depot", "rfrev_kva01.rfl", "rfrev_kva02.rfl"),
				new SplitLevelChange("Barracks Basement", "rfrev_kva02.rfl", "rfrev_kva03.rfl"),
				new SplitLevelChange("Sewer Flow Control", "rfrev_kva03.rfl", "rfrev_kva04.rfl"),
				new SplitLevelChange("Sewer Control Area", "rfrev_kva04.rfl", "rfrev_kva05.rfl"),
				new SplitLevelChange("Sewage Treatment Plant", "rfrev_kva05.rfl", "rfrev_kva06.rfl"),
				new SplitLevelChange("Storage Area", "rfrev_kva06.rfl", "rfrev_kva07.rfl"),
				new SplitLevelChange("Central Command - Level 1", "rfrev_kva07.rfl", "rfrev_kva08.rfl"),
				new SplitLevelChange("Central Command - Level 2", "rfrev_kva08.rfl", "rfrev_kva09.rfl"),
				new SplitLevelChange("Central Command - Level 3", "rfrev_kva09.rfl", "rfrev_kva10.rfl"),
				new SplitLevelChange("Docking Bay Finale", "rfrev_kva10.rfl", "rfrev_kvaend.rfl")
			}),
			new Mod("NGage SP Campaign Mod", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("The Mines (Pt. 1)", "l1s1.rfl", "ng1l2.rfl"),
				new SplitLevelChange("The Mines (Pt. 2)", "ng1l2.rfl", "ng1l3.rfl"),
				new SplitLevelChange("The Mines (Pt. 3)", "ng1l3.rfl", "ng2l1.rfl"),
				new SplitLevelChange("The Barracks (Pt. 1)", "ng2l1.rfl", "ng2l2.rfl"),
				new SplitLevelChange("The Barracks (Pt. 2)", "ng2l2.rfl", "ng3l1.rfl"),
				new SplitLevelChange("Docking Bay (Pt. 1)", "ng3l1.rfl", "ng3l2.rfl"), // docking bay 1
				new SplitLevelChange("Docking Bay (Pt. 2)", "ng3l2.rfl", "ng3l3.rfl"), // docking bay 2
				new SplitLevelChange("Docking Bay (Pt. 3)", "ng3l3.rfl", "ng4l1.rfl"), // docking bay 3
				new SplitLevelChange("Crevasse (Pt. 1)", "ng4l1.rfl", "ng4l2.rfl"), // crevasse 1
				new SplitLevelChange("Crevasse (Pt. 2)", "ng4l2.rfl", "ng5l1.rfl"), // crevasse 2
				new SplitLevelChange("Geothermal Power Plant", "ng5l1.rfl", "ng5l2.rfl"), // geothermal
				new SplitLevelChange("Underwater Tunnel", "ng5l2.rfl", "ng6l1.rfl"), // underwater tunnel
				new SplitLevelChange("Corporate HQ (Pt. 1)", "ng6l1.rfl", "ng6l2.rfl"), // corporate 1
				new SplitLevelChange("Corporate HQ (Pt. 2)", "ng6l2.rfl", "ng6l3.rfl"), // corporate 2
				new SplitLevelChange("Corporate HQ (Pt. 3)", "ng6l3.rfl", "ng7l1.rfl"), // corporate 3
				new SplitLevelChange("Medical Research (Pt. 1)", "ng7l1.rfl", "ng7l2.rfl"), // med lab 1
				new SplitLevelChange("Medical Research (Pt. 2)", "ng7l2.rfl", "ng8l1.rfl"), // med lab 2
				new SplitLevelChange("Capek\\'s Lair", "ng8l1.rfl", "ng8l2.rfl"), // capeks lair
				new SplitLevelChange("Inner Sanctum", "ng8l2.rfl", "ng9l1.rfl"), // inner sanctum
				new SplitLevelChange("Merc Command Centre", "ng9l1.rfl", "ng9l2.rfl"), // merc centre
				new SplitLevelChange("Missile Battery", "ng9l2.rfl", "ng10l1.rfl"), // missile battery
				new SplitLevelChange("Masako\\'s Lair (Pt. 1)", "ng10l1.rfl", "ng10l2.rfl"), // masako 1
				new SplitLevelChange("Masako\\'s Lair (Pt. 2)", "ng10l2.rfl", "glass_house.rfl") // masako 2
			}),
			new Mod("NGage SP Promo Campaign", "NGP1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Among the Mines", "NGP1.rfl", "NGP2.rfl"),
				new SplitLevelChange("Ultor\\'s Barracks", "NGP2.rfl", "NGP3.rfl"),
				new SplitLevelChange("Heavy Equipment", "NGP3.rfl", "NGP4.rfl"),
				new SplitLevelChange("Spooky Lair", "NGP4.rfl", "NGP5.rfl"),
				new SplitLevelChange("Icy Ride", "NGP5.rfl", "NGP6.rfl"),
				new SplitLevelChange("Power Station", "NGP6.rfl", "NGP7.rfl"),
				new SplitLevelChange("Elites\\' Division", "NGP7.rfl", "NGP8.rfl"),
				new SplitLevelChange("Sharp Greeting", "NGP8.rfl", "NGP9.rfl"),
				new SplitLevelChange("Open Area", "NGP9.rfl", "NGP10.rfl"),
				new SplitLevelChange("Boss", "NGP10.rfl", "glass_house.rfl")
			}),
			new Mod("Barracks Horror!", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Barracks Horror", "l1s1.rfl", "horrorend.rfl")
			}),
			new Mod("Nano-Theft", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Outside", "l1s1.rfl", "EDGarage.rfl"),
				new SplitLevelChange("Garage", "EDGarage.rfl", "NanoEnd.rfl")
			}),
			new Mod("LEGO Mod Campaign", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Sewer", "l1s1.rfl", "le2s1.rfl"),
				new SplitLevelChange("City", "le2s1.rfl", "le3s1.rfl"),
				new SplitLevelChange("Factory", "le3s1.rfl", "le4s1.rfl"),
				new SplitLevelChange("Tunnel", "le4s1.rfl", "le5s1.rfl"),
				new SplitLevelChange("Final Battle", "le5s1.rfl", "le6s1.rfl")
			}),
			new Mod("Red Doom", "RedDoom-00b.rfl", new List<SplitStructOverall>()
			{
				//technically starts when cutscene ends in RedDoom-00b.rfl, but no way to hook that currently
                new SplitLevelChange("Landing", "RedDoom-00b.rfl", "reddoom-01.rfl"),
				new SplitLevelChange("Lift Shaft", "reddoom-01.rfl", "reddoom-02.rfl"),
				new SplitLevelChange("Forcefield", "reddoom-02.rfl", "rd03a.rfl"),
				new SplitLevelChange("Laboratory", "rd03a.rfl", "rd-cellblock.rfl"),
				new SplitLevelChange("Facility", "rd-cellblock.rfl", "rd-final.rfl")
			}),
			new Mod("AQuest Speedrun Edition", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Cliffside 1", "l1s1.rfl", "aqsre1s2.rfl"),
				new SplitLevelChange("Cliffside 2", "aqsre1s2.rfl", "aqsre1s3.rfl"),
				new SplitLevelChange("Cliffside 3", "aqsre1s3.rfl", "aqsre1s4.rfl"),
				new SplitLevelChange("Cliffside 4", "aqsre1s4.rfl", "aqsre1s5.rfl"),
				new SplitLevelChange("Cliffside 5", "aqsre1s5.rfl", "aqsre1s6.rfl"),
				new SplitLevelChange("Cliffside 6", "aqsre1s6.rfl", "aqsre1s7.rfl"),
				new SplitLevelChange("Cliffside 7", "aqsre1s7.rfl", "aqsre1s8.rfl"),
				new SplitLevelChange("Cliffside 8", "aqsre1s8.rfl", "aqsre1s9.rfl"),
				new SplitLevelChange("Cliffside 9", "aqsre1s9.rfl", "aqsre1s10.rfl"),
				new SplitLevelChange("Cliffside 10", "aqsre1s10.rfl", "aqsre2s1.rfl"),
				new SplitLevelChange("Egyptian 1", "aqsre2s1.rfl", "aqsre2s2.rfl"),
				new SplitLevelChange("Egyptian 2", "aqsre2s2.rfl", "aqsre2s3.rfl"),
				new SplitLevelChange("Egyptian 3", "aqsre2s3.rfl", "aqsre2s4.rfl"),
				new SplitLevelChange("Egyptian 4", "aqsre2s4.rfl", "aqsre2s5.rfl"),
				new SplitLevelChange("Egyptian 5", "aqsre2s5.rfl", "aqsre2s6.rfl"),
				new SplitLevelChange("Egyptian 6", "aqsre2s6.rfl", "aqsre2s7.rfl"),
				new SplitLevelChange("Egyptian 7", "aqsre2s7.rfl", "aqsre2s8.rfl"),
				new SplitLevelChange("Egyptian 8", "aqsre2s8.rfl", "aqsre2s10.rfl"),
				new SplitLevelChange("Egyptian 10", "aqsre2s10.rfl", "aqsre3s1.rfl"),
				new SplitLevelChange("Factory 1", "aqsre3s1.rfl", "aqsre3s2.rfl"),
				new SplitLevelChange("Factory 2", "aqsre3s2.rfl", "noexist.rfl") // uses endgame event, currently no way to hook
            }),
			new Mod("CORNDOG", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Revival of Capek", "l1s1.rfl", "crash.rfl"),
				new SplitLevelChange("Crash", "crash.rfl", "cool.rfl"),
				new SplitLevelChange("Cool", "cool.rfl", "chimp.rfl"),
				new SplitLevelChange("Chimp", "chimp.rfl", "blue.rfl"),
				new SplitLevelChange("Blue", "blue.rfl", "cats.rfl"),
				new SplitLevelChange("Cats", "cats.rfl", "tram.rfl"),
				new SplitLevelChange("Tram", "tram.rfl", "fruit.rfl") // ends with a requested load to a map that doesn't exist
            }),
			new Mod("Capture the Flag!", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Entrance", "l1s1.rfl", "Admin01.rfl"),
				new SplitLevelChange("Administration Offices", "Admin01.rfl", "Security01.rfl"),
				new SplitLevelChange("Security", "Security01.rfl", "Security02.rfl"),
				new SplitLevelChange("Security Exit", "Security02.rfl", "Admin02.rfl"),
				new SplitLevelChange("Admin Retreat", "Admin02.rfl", "Floor01.rfl"),
				new SplitLevelChange("Escape", "Floor01.rfl", "CTFEND.rfl")
			}),
			new Mod("RF Fighter", "l1s1.rfl", new List<SplitStructOverall>()
			{
				new SplitLevelChange("Start", "RFB01b.rfl", "RFB02.rfl"),
				new SplitLevelChange("Elevator", "RFB02b.rfl", "RFB03.rfl"),
				new SplitLevelChange("Abandoned Area", "RFB04.rfl", "RFB05.rfl"),
				new SplitLevelChange("Into the Mines", "RFB05b.rfl", "noexist.rfl") // uses endgame event, currently no way to hook
            })
		};

		public Mod[] Mods;

		public List<SplitStructOverall> CurrentSplits => Mods[ModIndex].Splits;

		public RedFactionSettings()
		{
			InitializeComponent();

			this.chkAutoReset.DataBindings.Add("Checked", this, "AutoReset", false, DataSourceUpdateMode.OnPropertyChanged);
			this.chkAutoStart.DataBindings.Add("Checked", this, "AutoStart", false, DataSourceUpdateMode.OnPropertyChanged);
			this.Cbox_Mod.Items.Clear();
			foreach (var mod in DEFAULT_MODS)
			{
				this.Cbox_Mod.Items.Add(mod);
			}

			this.Cbox_Mod.DataBindings.Add("SelectedIndex", this, "ModIndex", false, DataSourceUpdateMode.OnPropertyChanged);

			// defaults
			this.AutoReset = DEFAULT_AUTORESET;
			this.AutoStart = DEFAULT_AUTOSTART;
			this.ModIndex = DEFAULT_MODINDEX;

			ValidateLevelNames();

			this.Mods = new Mod[DEFAULT_MODS.Length];
			for (int i = 0; i < Mods.Length; i++)
			{
				Mods[i] = (Mod)DEFAULT_MODS[i].Clone();
			}
		}

		private void ValidateLevelNames()
		{
			for (int modID = 0; modID < DEFAULT_MODS.Length; modID++)
			{
				var mod = DEFAULT_MODS[modID];
				for (int i = 0; i < mod.Splits.Count; i++)
				{
					if (mod.Splits[i].GetType() == typeof(SplitLevelChange))
					{
						var cast = (SplitLevelChange)mod.Splits[i];
						cast.PreviousLevelName = cast.PreviousLevelName.ToLower();
						cast.CurrentLevelName = cast.CurrentLevelName.ToLower();
					}
				}
			}
		}

		public XmlNode GetSettings(XmlDocument doc)
		{
			XmlElement settingsNode = doc.CreateElement("Settings");

			settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

			settingsNode.AppendChild(ToElement(doc, "AutoReset", this.AutoReset));
			settingsNode.AppendChild(ToElement(doc, "AutoStart", this.AutoStart));
			settingsNode.AppendChild(ToElement(doc, "ModIndex", this.ModIndex));
			settingsNode.AppendChild(ToElement(doc, "ModStates", this.Mods));

			return settingsNode;
		}

		public void SetSettings(XmlNode settings)
		{
			this.AutoReset = ParseBool(settings, "AutoReset", DEFAULT_AUTORESET);
			this.AutoStart = ParseBool(settings, "AutoStart", DEFAULT_AUTOSTART);
			this.ModIndex = ParseInt(settings, "ModIndex", DEFAULT_MODINDEX, 0, DEFAULT_MODS.Length - 1);
			Mods = ParseXML(settings, "ModStates", DEFAULT_MODS);
		}

		private Mod[] ParseXML(XmlNode settings, string setting, Mod[] default_)
		{
			var modParse = new Mod[default_.Length];
			for (int i = 0; i < modParse.Length; i++)
			{
				modParse[i] = (Mod)default_[i].Clone();
			}

			if (settings[setting] is null)
				return modParse;
			else
			{
				var node = settings[setting];
				foreach (XmlElement mod in node)
				{
					var modName = mod.Attributes["Name"];
					if (modName != null)
					{
						var foundMod = modParse.FirstOrDefault(x => x.ModName == modName.InnerText);
						if (foundMod != null)
						{
							if (mod["Splits"] != null)
							{
								var splitsNode = mod["Splits"];
								foreach (XmlElement split in splitsNode)
								{
									if (split.Attributes["Name"] != null)
									{
										var splitName = split.Attributes["Name"];
										var foundSplit = foundMod.Splits.FirstOrDefault(x => x.Name == splitName.InnerText);
										if (foundSplit != null)
										{
											foundSplit.Split = bool.TryParse(split.InnerText, out var parsedVal) ? parsedVal : false;
										}
									}
								}
							}
						}
					}
				}

				return modParse;
			}
		}

		static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
		{
			bool val;
			return settings[setting] != null ?
				(Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_)
				: default_;
		}

		static int ParseInt(XmlNode settings, string setting, int default_ = 0, int min = int.MinValue, int max = int.MaxValue)
		{
			int val;
			if (settings[setting] != null)
			{
				if (int.TryParse(settings[setting].InnerText, out val))
				{
					val = MathStuff.Clamp(val, min, max);
					return val;
				}
				else
					return default_;

			}
			return default_;
		}

		static XmlElement ToElement<T>(XmlDocument document, string name, T value)
		{
			XmlElement str = document.CreateElement(name);
			str.InnerText = value.ToString();
			return str;
		}

		static XmlElement ToElement(XmlDocument document, string name, Mod[] value)
		{
			//God help me
			XmlElement str = document.CreateElement(name);
			for (int i = 0; i < value.Length; i++)
			{
				var node = ToElement(document, value[i]);
				str.AppendChild(node);
			}

			return str;
		}

		static XmlElement ToElement(XmlDocument document, Mod value)
		{
			XmlElement str = document.CreateElement("Mod");
			var attr = document.CreateAttribute("Name");
			attr.InnerText = value.ModName;
			str.Attributes.Append(attr);
			var node = ToElement(document, value.Splits);
			str.AppendChild(node);
			return str;
		}

		static XmlElement ToElement(XmlDocument document, List<SplitStructOverall> value)
		{
			XmlElement str = document.CreateElement("Splits");
			for (int i = 0; i < value.Count; i++)
			{
				var node = ToElement(document, value[i]);
				str.AppendChild(node);
			}
			return str;
		}

		static XmlElement ToElement(XmlDocument document, SplitStructOverall value)
		{
			XmlElement str = document.CreateElement("Split");
			var attr = document.CreateAttribute("Name");
			attr.InnerText = value.Name;
			str.Attributes.Append(attr);
			str.InnerText = value.Split.ToString();
			return str;
		}

		private bool Initializing;

		private void Cbox_Mod_SelectedIndexChanged(object sender, EventArgs e)
		{
			CBList_Splits.DataSource = Mods[((ComboBox)sender).SelectedIndex].Splits;
			CBList_Splits.DisplayMember = "Name";
			CBList_Splits.ValueMember = "Split";

			Initializing = true;
			for (int i = 0; i < CBList_Splits.Items.Count; i++)
			{
				var obj = (SplitStructOverall)CBList_Splits.Items[i];
				CBList_Splits.SetItemChecked(i, obj.Split);
			}
			Initializing = false;
		}

		private void CBList_Splits_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (!Initializing)
			{
				CurrentSplits[e.Index].Split = e.NewValue == CheckState.Checked;
			}
		}
	}
}
