﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ConfigNodeParser;
using KerbalGenerator.Kerbals;
using KerbalGenerator.Accumulators;
using KerbalGenerator.Logging;

namespace KerbalGenerator {
	class KerbalGenerator {
		#region class variables

		private readonly string configPath = Path.Combine ( Path.Combine ( Path.Combine ( Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData ), "BadWater" ), "KerbalGen" ), "Config" );
		private readonly string logPath = Path.Combine ( Path.Combine ( Path.Combine ( Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData ), "BadWater" ), "KerbalGen" ), "Logs" );


		private Config config;
		private Kerbal currentKerbal;

		private Roster roster;
		private Configurator cfgr;
		private frm_Krb_Gen MainForm;

		private ConfigNode persistent;
		private ConfigNode currentGame;
		private string currentSavePath;

		public Roster Rstr { get { return roster; } }

		public Config Cfg { get { return config; } }

		public string CurrentSavePath { get { return currentSavePath; } }

		#endregion

		public KerbalGenerator ( frm_Krb_Gen mainForm ) {
			cfgr = new Configurator ( configPath );
			MainForm = mainForm;
			Initialize ( );
		}

		/// <summary>
		/// Checks to see if this is the first time the program has run.
		/// </summary>
		/// <returns><c>true</c>, If the program has never run before, <c>false</c> otherwise.</returns>
		private bool FirstRun ( ) {
			string path = Path.Combine ( configPath, "config.xml" );
			return !File.Exists ( path );
		}

		/// <summary>
		/// Initializes the program.
		/// </summary>
		private void Initialize ( ) {
			if ( FirstRun ( ) ) {
				Logger.LogEvent ( "First Run At: " + DateTime.Now.ToString ( ) );
				MainForm.SetAllControls ( false );
				cfgr.FirstRun ( );
			}
			cfgr.LoadConfig ( );
			config = cfgr.Configuration;
			SelectSave ( 0 );
			SelectKerbal ( 0 );
			MainForm.SetAllControls ( true );
		}

		/// <summary>
		/// Selects savepath by index.
		/// Should only ever be needed in initialize.
		/// </summary>
		/// <param name="index">Should Always be 0</param>
		private void SelectSave ( int index ) {
			currentSavePath = config.SavePaths.Values.ElementAt ( 0 );
			persistent = ConfigNode.Load ( currentSavePath );
			ParseRoster ( );
		}

		/// <summary>
		/// Select Save by Name.  
		/// </summary>
		/// <param name="name">The Name Of The Save To Select</param>
		internal void SelectSave ( string name ) {
			currentSavePath = Cfg.SavePaths [ name ];
			persistent = ConfigNode.Load ( currentSavePath );
			ParseRoster ( );
		}

		/// <summary>
		/// Selects Kerbal By Index.  Should only ever be used on initialization.
		/// </summary>
		/// <param name="index"></param>
		private void SelectKerbal ( int index ) {
			currentKerbal = roster.GetKerbal ( 0 );
			UpdateKerbalStats ( );
		}

		/// <summary>
		/// Selects Kerbal based on a String Value.
		/// </summary>
		/// <param name="kerbalName"></param>
		public void SelectKerbal ( string kerbalName ) {
			currentKerbal = roster.GetKerbal ( kerbalName );
			UpdateKerbalStats ( );
		}

		/// <summary>
		/// Return A String Array Of All Save Keys.
		/// </summary>
		/// <returns></returns>
		public String[ ] GetSaves ( ) {
			return config.SavePaths.Keys.ToArray ( );
		}

		/// <summary>
		/// Returns A String Array With The Name Of Each Kerbal In The Roster.
		/// </summary>
		/// <returns></returns>
		public string[ ] GetRosterNames ( ) {
			return roster.GetNames ( ).ToArray ( );
		}

		/// <summary>
		/// Send stats back to mainform to be displayed.
		/// </summary>
		internal void UpdateKerbalStats ( ) {
			MainForm.UpdateKerbalStats ( currentKerbal );
			
		}

		/// <summary>
		/// Updates The MainForm's Save Path Label and Kerbal Count.
		/// </summary>
		internal void UpdateSaveStats ( ) {
			MainForm.UpdateSaveStats ( currentSavePath, new KerbalKounter ( ).KountKerbals ( roster ) );
			MainForm.UpdateKerbalList ( );
		}

		/// <summary>
		/// Parses the Roster Node Into A Roster Object
		/// </summary>
		private void ParseRoster ( ) {
			currentGame = persistent.GetNode ( "GAME" );
			roster = RosterParser.GetRoster ( currentGame );
		}

		/// <summary>
		/// Save Our Updated Roster To File
		/// </summary>
		/// <param name="saveFile">No longer used.  There because I'm afraid Removing It Will break Something.</param>
		public void Save ( string saveFile ) {
			RosterParser.InsertRoster ( roster, currentGame );
			persistent.Save ( currentSavePath );
			
		}

		/// <summary>
		/// Create a New Kerbal, and Add to the Temporary Roster
		/// </summary>
		/// <param name="sa">The Specific Accumulator To Use.</param>
		public void KreateKerbal ( SpecificAccumulator sa ) {
			Kerbal k = new KerbalMaker ( ).KreateKerbal ( sa );
			switch ( PreviewKerbal ( k ) ) {
			case DialogResult.Yes:
				roster.AddKerbal ( k );
				break;
			default:
				break;
			}
			UpdateKerbalStats ( );
			UpdateSaveStats ( );
		}

		/// <summary>
		/// Kreates the roster.
		/// </summary>
		/// <param name="ra">Ra is a RandomAccumulator which holds the values for creating a roster</param>
		public void KreateRoster ( RandomAccumulator ra ) {
			Roster r = new KerbalMaker ( ).KreateRoster ( ra, roster );
			switch ( PreviewRoster ( r ) ) {
			case ( DialogResult.Yes ):
				roster.AddRoster ( r );
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// Return the Configurator Form/
		/// </summary>
		/// <returns></returns>
		public Form GetCfgrForm ( ) {
			return new ConfiguratorForm ( cfgr, false );
		}

		/// <summary>
		/// Preview the Just Created Kerbal
		/// </summary>
		/// <param name="k">The Kerbal We Just Created.</param>
		/// <returns></returns>
		private DialogResult PreviewKerbal ( Kerbal k ) {
			return new KerbalPreviewWindow ( k ).ShowDialog ( );
		}

		/// <summary>
		/// Previews the roster.
		/// </summary>
		/// <returns>The roster.</returns>
		/// <param name="r">The Roster To Preview</param>
		private DialogResult PreviewRoster ( Roster r ) {
			return new KerbalPreviewWindow ( r ).ShowDialog ( );
		}

		/// <summary>
		/// Validates the name of the kerbal.
		/// </summary>
		/// <returns><c>true</c>, if kerbal name was validated, <c>false</c> otherwise.</returns>
		/// <param name="name">Name.</param>
		public bool ValidateKerbalName ( string name ) {
			return ( roster.ValidateKerbal ( name ) );
		}

		
	}
}



