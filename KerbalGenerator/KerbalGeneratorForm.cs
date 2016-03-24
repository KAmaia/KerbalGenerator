﻿using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KerbalGenerator {
	public partial class frm_Krb_Gen : Form {
		private readonly string configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Badwater\\KerbalGen";

		private Config config;

		private Configurator configurator;

		private Dictionary<string, Kerbal>roster;



		public frm_Krb_Gen( ) {
			InitializeComponent( );
			configurator = new Configurator( configPath );
			if ( FirstRun( ) ) {
				//lock all controls except configurator button.
				lockAllControls( );
			}
			else {
				configurator.LoadConfig( );
				config = configurator.Configuration;
			}
			lbl_kspInstalDir.Text = config.KSPPath;

			Debug.WriteLine( config.KSPPath );
		}

		private void lockAllControls( ) {
			foreach ( Control c in Controls ) {
				if ( c.Name.ToLower( ).Equals( "pnl_po_options" ) ) {
					continue;
				}
				else {
					c.Enabled = false;
				}
			}
		}

		private void parseKerbals( ) {
			cmb_kerb_list.Items.Clear( );
			KerbalParser parseTheKerbals = new KerbalParser();
			string persistent = cmb_AvailSaves.SelectedItem.ToString();
			parseTheKerbals.Parse( config.SavePaths[persistent] );
			roster = parseTheKerbals.Roster;
			cmb_kerb_list.Items.AddRange( roster.Keys.ToArray( ) );
			cmb_kerb_list.SelectedIndex = 0;

		}

		private bool FirstRun( ) {
			return !File.Exists( configPath + "\\config.xml" );
		}


		private void btn_gen_One_Kerb_Click( object sender, EventArgs e ) {

		}
		private void btn_gen_List_Kerb_Click( object sender, EventArgs e ) {

		}
		private void btn_po_exit_Click( object sender, EventArgs e ) {
			Application.Exit( );
		}
		private void btn_view_Kerbals_Click( object sender, EventArgs e ) {


		}
		private void frm_Krb_Gen_Load( object sender, EventArgs e ) {

			this.Text = "Kerbal Generator -- " + config.Name;
			cmb_AvailSaves.Items.AddRange( config.SavePaths.Keys.ToArray( ) );
			cmb_AvailSaves.SelectedIndex = 0;
			parseKerbals( );
		}

		private void cmb_AvailSaves_SelectedIndexChanged( object sender, EventArgs e ) {

			string path = cmb_AvailSaves.SelectedItem.ToString();
			Debug.WriteLine( path );
			string debug = config.SavePaths[path];
			Debug.WriteLine( "+++" + debug );
			Debug.WriteLine( "==" + config.SavePaths[path] );
			lbl_currentSaveLocation.Text = config.SavePaths[path];
			parseKerbals( );
			displaySaveStats( );
		}

		private void btn_gen_List_Kerb_Click_1( object sender, EventArgs e ) {

		}

		private void label3_Click( object sender, EventArgs e ) {

		}

		private void label19_Click( object sender, EventArgs e ) {

		}

		private void label22_Click( object sender, EventArgs e ) {

		}

		private void label23_Click( object sender, EventArgs e ) {

		}

		private void trackBar8_Scroll( object sender, EventArgs e ) {

		}


		private void groupBox6_Enter( object sender, EventArgs e ) {

		}

		private void lbl_si_kerbals_Click( object sender, EventArgs e ) {

		}

		private void label31_Click( object sender, EventArgs e ) {

		}

		private void groupBox1_Enter( object sender, EventArgs e ) {

		}

		private void btn_po_OpenCfgr_Click( object sender, EventArgs e ) {
			Form cfgrform = new ConfiguratorForm(ref configurator);
			cfgrform.Show( );
		}

		private void cmb_kerb_list_SelectedIndexChanged( object sender, EventArgs e ) {
			string selectedKerbal = cmb_kerb_list.SelectedItem.ToString();
			Kerbal k = roster[selectedKerbal];
			DisplayKerbalStats( k );
		}

		private void displaySaveStats( ) {
			Dictionary<string, int> tmp = countKerbals();
			lbl_si_badscountdisp.Text = tmp["badasskerbals"].ToString();
			lbl_si_tourcountdisp.Text = tmp["touristkerbals"].ToString( );
			lbl_si_kerbcountdisp.Text = tmp["totalkerbals"].ToString( );
			lbl_si_livingcountdisp.Text = tmp["livingkerbals"].ToString( );
			lbl_si_deadcountdisp.Text = tmp["deadkerbals"].ToString( );
			lbl_si_genderfcountdisp.Text = tmp["femalekerbals"].ToString( );
			lbl_si_gendermcountdisp.Text = tmp["malekerbals"].ToString( );
			lbl_si_engicountdisp.Text = tmp["engineerkerbals"].ToString( );
			lbl_si_scicountdisp.Text = tmp["scientistkerbals"].ToString( );
			lbl_si_pilotcountdisp.Text = tmp["pilotkerbals"].ToString( );


		}

		private void DisplayKerbalStats( Kerbal k ) {
			lbl_ki_genderdisp.Text = k.Gender;
			lbl_ki_roledisp.Text = k.Type;
			lbl_ki_badsdisp.Text = k.Bads;
			lbl_ki_tourdisp.Text = k.Tour;
			lbl_ki_bravedisp.Text = k.Brave;
			lbl_ki_stupiddisp.Text = k.Dumb;
			lbl_ki_statusdisp.Text = k.Trait;
			lbl_ki_statedisp.Text = k.State;
			lbl_ki_flightcountdisp.Text = k.Flights;
		}

		private Dictionary<string, int> countKerbals( ) {
			Dictionary<string, int>saveStatsDict = new Dictionary<string, int>();
			saveStatsDict.Add( "totalkerbals", 0 );
			saveStatsDict.Add( "malekerbals", 0 );
			saveStatsDict.Add( "femalekerbals", 0 );
			saveStatsDict.Add( "livingkerbals", 0 );
			saveStatsDict.Add( "deadkerbals", 0 );
			saveStatsDict.Add( "touristkerbals", 0 );
			saveStatsDict.Add( "engineerkerbals", 0 );
			saveStatsDict.Add( "scientistkerbals", 0 );
			saveStatsDict.Add( "pilotkerbals", 0 );
			saveStatsDict.Add( "badasskerbals", 0 );

			foreach ( string s in roster.Keys ) {
				saveStatsDict["totalkerbals"] += 1;
				if ( roster[s].Gender.ToLower( ).Equals( "male" ) ) {
					saveStatsDict["malekerbals"] += 1;
				}
				if ( roster[s].Gender.ToLower( ).Equals( "female" ) ) {
					saveStatsDict["femalekerbals"] += 1;
				}
				if ( ( roster[s].State.ToLower( ).Equals( "available" ) ) || roster[s].State.ToLower( ).Equals( "assigned" ) ) {
					saveStatsDict["livingkerbals"] += 1;
				}
				if ( roster[s].State.ToLower( ).Equals( "dead" ) ) {
					saveStatsDict["deadkerbals"] += 1;
				}
				if ( roster[s].Trait.ToLower( ).Equals( "engineer" ) ) {
					saveStatsDict["engineerkerbals"] += 1;
				}
				if ( roster[s].Trait.ToLower( ).Equals( "pilot" ) ) {
					saveStatsDict["pilotkerbals"] += 1;
				}
				if ( roster[s].Trait.ToLower( ).Equals( "scientist" ) ) {
					saveStatsDict["scientistkerbals"] += 1;
				}
				if ( roster[s].Tour.ToLower( ).Equals( "true" ) ) {
					saveStatsDict["touristkerbals"] += 1;
				}
				if ( roster[s].Bads.ToLower( ).Equals( "true" ) ) {
					saveStatsDict["badasskerbals"] += 1;
				}
			}
			return saveStatsDict;
		}

		private void grp_save_flag_Enter( object sender, EventArgs e ) {

		}
	}

}
