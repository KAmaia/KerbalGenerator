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
using KerbalGenerator.Accumulators;

namespace KerbalGenerator {
	public partial class frm_Krb_Gen : Form {
		private KerbalGenerator generator;

		private SpecificAccumulator specAccum;
		private RandomAccumulator randAccum;

		private Dictionary<string, int> weights;

		public frm_Krb_Gen( ) {
			InitializeComponent( );
			generator = new KerbalGenerator( this );
			specAccum = new SpecificAccumulator( );
			randAccum = new RandomAccumulator( );
			weights = new Dictionary<string, int>( );
			weights.Add( "tbar_rnd_maxpilots", 60 );
			weights.Add( "tbar_rnd_maxengi", 30 );
			weights.Add( "tbar_rnd_maxsci", 10 );
		}

		private void LockOutRandom( ) {
			foreach ( Control c in pnl_rnd_gen.Controls ) {
				c.Enabled = false;
			}
		}

		/// <summary>
		/// Sets all controls except program options to active
		/// </summary>
		/// <param name="active"></param>
		internal void SetAllControls( bool active ) {
			foreach ( Control c in Controls ) {
				if ( c.Name.ToLower( ).Equals( "pnl_po_options" ) ) {
					continue;
				}
				else {
					c.Enabled = active;
				}
			}


			//LockOutRandom( );
		}

		private void frm_Krb_Gen_Load( object sender, EventArgs e ) {
			Text = "Kerbal Generator -- " + generator.Cfg.Name;
			cmb_AvailSaves.Items.AddRange( generator.GetSaves( ) );
			cmb_AvailSaves.SelectedIndex = 0;
			tbar_rnd_maxpilots.Value = weights["tbar_rnd_maxpilots"];
			tbar_rnd_maxengi.Value = weights["tbar_rnd_maxengi"];
			tbar_rnd_maxsci.Value = weights["tbar_rnd_maxsci"];
			btn_spe_generate.Enabled = false;
		}

		#region Stats Panel
		internal void UpdateSaveStats( string currentSavePath, Dictionary<string, int> countedKerbals ) {
			//Update Our Current Save Path
			lbl_currentSaveLocation.Text = currentSavePath;
			//Update Our Current Kerbal counts
			lbl_si_kerbcountdisp.Text = countedKerbals["Total"].ToString( );
			lbl_si_livingcountdisp.Text = countedKerbals["Living"].ToString( );
			lbl_si_deadcountdisp.Text = countedKerbals["Dead"].ToString( );
			lbl_si_pilotcountdisp.Text = countedKerbals["Pilot"].ToString( );
			lbl_si_engicountdisp.Text = countedKerbals["Engineer"].ToString( );
			lbl_si_scicountdisp.Text = countedKerbals["Scientist"].ToString( );
			lbl_si_gendermcountdisp.Text = countedKerbals["Male"].ToString( );
			lbl_si_genderfcountdisp.Text = countedKerbals["Female"].ToString( );
			lbl_si_badscountdisp.Text = countedKerbals["Badass"].ToString( );
			lbl_si_tourcountdisp.Text = countedKerbals["Tourist"].ToString( );
			lbl_si_hireddisp.Text = countedKerbals["Crew"].ToString( );
			lbl_si_applcntdisp.Text = countedKerbals["Applicant"].ToString( );
			lbl_si_assigneddisp.Text = countedKerbals["Assigned"].ToString( );
			lbl_si_availdisp.Text = countedKerbals["Available"].ToString( );
		}

		internal void UpdateKerbalList( ) {
			cmb_kerb_list.Items.Clear( );
			cmb_kerb_list.Items.AddRange( generator.GetRosterNames( ) );
			cmb_kerb_list.SelectedIndex = 0;
		}
		#endregion
		#region Available Saves Panel
		private void cmb_AvailSaves_SelectedIndexChanged( object sender, EventArgs e ) {
			string saveName = cmb_AvailSaves.SelectedItem.ToString();
			generator.SelectSave( saveName );
			generator.UpdateSaveStats( );
			cmb_kerb_list.Items.Clear( );
			cmb_kerb_list.Items.AddRange( generator.GetRosterNames( ) );
			cmb_kerb_list.SelectedIndex = 0;
			generator.SelectKerbal( cmb_kerb_list.SelectedItem.ToString( ) );
			generator.UpdateKerbalStats( );
		}
		#endregion
		#region program options panel
		private void btn_po_OpenCfgr_Click( object sender, EventArgs e ) {
			Form cfgrform = generator.GetCfgrForm();
			cfgrform.Show( );
		}

		private void btn_po_Save_Click( object sender, EventArgs e ) {
			generator.Save( cmb_AvailSaves.SelectedItem.ToString( ) );
		}

		private void btn_po_exit_Click( object sender, EventArgs e ) {
			Application.Exit( );
		}
		#endregion
		#region kerbalinfopanel
		internal void UpdateKerbalStats( Kerbal currentKerbal ) {

			lbl_ki_genderdisp.Text = currentKerbal.GetStat( "gender" );
			lbl_ki_roledisp.Text = currentKerbal.GetStat( "trait" );
			lbl_ki_statusdisp.Text = currentKerbal.GetStat( "type" );
			lbl_ki_badsdisp.Text = currentKerbal.GetStat( "badS" );
			lbl_ki_tourdisp.Text = currentKerbal.GetStat( "tour" );
			lbl_ki_bravedisp.Text = currentKerbal.GetStat( "brave" );
			lbl_ki_stupiddisp.Text = currentKerbal.GetStat( "dumb" );
			lbl_ki_statedisp.Text = currentKerbal.GetStat( "state" );

			foreach ( KeyValuePair<string, string> stat in currentKerbal.Stats ) {

				if ( stat.Key == "gender" ) {
					lbl_ki_genderdisp.Text = stat.Value;
				}
				if ( stat.Key == "trait" ) {
					lbl_ki_roledisp.Text = stat.Value;
				}
				if ( stat.Key == "type" ) {
					lbl_ki_statusdisp.Text = stat.Value;
				}
				if ( stat.Key == "badS" ) {
					lbl_ki_badsdisp.Text = stat.Value;
				}
				if ( stat.Key == "tour" ) {
					lbl_ki_tourdisp.Text = stat.Value;
				}
				if ( stat.Key == "brave" ) {
					lbl_ki_bravedisp.Text = stat.Value;
				}
				if ( stat.Key == "dumb" ) {
					lbl_ki_stupiddisp.Text = stat.Value;
				}
				if ( stat.Key == "state" ) {
					lbl_ki_statedisp.Text = stat.Value;
				}

			}
		}

		private void cmb_kerb_list_SelectedIndexChanged( object sender, EventArgs e ) {
			generator.SelectKerbal( cmb_kerb_list.SelectedItem.ToString( ) );
		}
		#endregion
		#region Random Kerbal Generation
		private void btn_gen_List_Kerb_Click( object sender, EventArgs e ) {


		}

		private void chk_rnd_allFemale_CheckedChanged( object sender, EventArgs e ) {

		}

		private void chk_rnd_allMale_CheckedChanged( object sender, EventArgs e ) {

		}

		#endregion
		#region Specific Kerbal Generation
		private void btn_spe_reset_Click( object sender, EventArgs e ) {
			txt_spe_kerbname.Text = "";

			rd_spe_genderfemale.Checked = true;
			rd_spe_pilot.Checked = true;

			chk_spe_badass.Checked = false;
			chk_spe_tourist.Checked = false;

			tbar_spe_brave.Value = 0;
			tbar_spe_stupid.Value = 0;

			btn_spe_generate.Enabled = false;

			lbl_spe_stupiddisp.Text = "0";
			lbl_spe_bravedisp.Text = "0";

			chk_spe_rndBrave.Checked = false;
			chk_spe_rndStupid.Checked = false;

			specAccum = specAccum.Reset( );
		}

		private void tbar_spe_stupid_Scroll( object sender, EventArgs e ) {
			lbl_spe_stupiddisp.Text = tbar_spe_stupid.Value.ToString( );
			specAccum.dumb = tbar_spe_stupid.Value;
		}

		private void tbar_spe_brave_Scroll( object sender, EventArgs e ) {
			lbl_spe_bravedisp.Text = tbar_spe_brave.Value.ToString( );
			specAccum.brave = tbar_spe_brave.Value;
		}

		private void chk_spe_lastNameKerman_CheckedChanged( object sender, EventArgs e ) {
			txt_spe_kerbname.MaxLength = chk_spe_lastNameKerman.Checked ? 11 : 18;
			specAccum.IsKerman = chk_spe_lastNameKerman.Checked;
		}

		private void txt_spe_kerbname_TextChanged( object sender, EventArgs e ) {
			//Disable the generate button if our name is blank.
			if ( !chk_spe_rndName.Checked ) {
				btn_spe_generate.Enabled = !( txt_spe_kerbname.Text == "" );
			}
			specAccum.Name = txt_spe_kerbname.Text;
		}

		private void chk_spe_rndName_CheckedChanged( object sender, EventArgs e ) {
			txt_spe_kerbname.Text = "";
			txt_spe_kerbname.Enabled = !chk_spe_rndName.Checked;
			specAccum.RndName = chk_spe_rndName.Checked;
			btn_spe_generate.Enabled = true;
		}

		private void btn_spe_generate_Click( object sender, EventArgs e ) {
			DetermineTrait( );
			generator.KreateKerbal( specAccum );
		}

		private void chk_spe_rndBrave_CheckedChanged( object sender, EventArgs e ) {
			if ( chk_spe_rndBrave.Checked ) {
				tbar_spe_brave.Value = 0;
				lbl_spe_bravedisp.Text = "";
				specAccum.RndBrave = chk_spe_rndBrave.Checked;
			}
			tbar_spe_brave.Enabled = !chk_spe_rndBrave.Checked;
		}

		private void chk_spe_rndStupid_CheckedChanged( object sender, EventArgs e ) {
			if ( chk_spe_rndStupid.Checked ) {
				tbar_spe_stupid.Value = 0;
				lbl_spe_stupiddisp.Text = "";
				specAccum.RndDumb = chk_spe_rndStupid.Checked;
			}
			tbar_spe_stupid.Enabled = !chk_spe_rndStupid.Checked;
		}

		private void DetermineTrait( ) {
			if ( rd_spe_pilot.Checked ) {
				specAccum.Trait = "Pilot";
			}
			if ( rd_spe_sci.Checked ) {
				specAccum.Trait = "Scientist";
			}
			if ( rd_spe_engi.Checked ) {
				specAccum.Trait = "Engineer";
			}
		}

		private void chk_spe_badass_CheckedChanged( object sender, EventArgs e ) {
			specAccum.Badass = chk_spe_badass.Checked;
		}

		private void chk_spe_tourist_CheckedChanged( object sender, EventArgs e ) {
			specAccum.Tourist = chk_spe_tourist.Checked;
		}

		private void rd_spe_gendermale_CheckedChanged( object sender, EventArgs e ) {
			if ( rd_spe_gendermale.Checked ) {
				specAccum.Female = false;
			}
		}

		private void rd_spe_genderfemale_CheckedChanged( object sender, EventArgs e ) {
			if ( rd_spe_genderfemale.Checked ) {
				specAccum.Female = true;
			}
		}


		#endregion

		private void groupBox4_Enter( object sender, EventArgs e ) {

		}

		private void tbar_rnd_FemaleToMale_Scroll( object sender, EventArgs e ) {
			decimal disp = 1 - Math.Abs((decimal) tbar_rnd_FemaleToMale.Value /(decimal) 100 );

			if ( tbar_rnd_FemaleToMale.Value == 0 ) {
				lbl_rnd_mfRatio.Text = "1:1";
				//if the value == 0, stop the slider to give it a snappy feeling.
				//it's hacky, but it works.
				//TODO: find a better way to do this.
				tbar_rnd_FemaleToMale.Enabled = false;
				tbar_rnd_FemaleToMale.Enabled = true;
				tbar_rnd_FemaleToMale.Select( );
			}
			else if ( disp.Equals( .50 ) || disp.Equals( .25 ) ) {
				tbar_rnd_FemaleToMale.Enabled = false;
				tbar_rnd_FemaleToMale.Enabled = true;
				tbar_rnd_FemaleToMale.Select( );
			}

			if ( tbar_rnd_FemaleToMale.Value > 0 ) {
				lbl_rnd_mfRatio.Text = disp + ":1";
			}
			else if ( tbar_rnd_FemaleToMale.Value < 0 ) {
				lbl_rnd_mfRatio.Text = "1:" + Math.Abs( disp );
			}
		}

		private void checkBox1_CheckedChanged( object sender, EventArgs e ) {
			tbar_rnd_FemaleToMale.Enabled = checkBox1.Checked ? false : true;
		}

		private void tbar_PES_scroll( object sender, EventArgs e ) {
			TrackBar tb = sender as TrackBar;
			UpdateWeights( tb );
		}

		private void UpdateWeights( TrackBar tb ) {
			decimal accum =  nud_rnd_howMany.Value;
			Dictionary<string, int> oldWeights = weights;
			tbar_rnd_maxpilots.Value = tb.Value;

			//figure out how much it moved
			int moved = 0;
			switch ( tb.Name ) {
				case "tbar_rnd_maxpilots":
					break;
				case "tbar_rnd_maxengi":
					break;
				case "tbar_rnd_maxsci":
					break;
				default:
					break;
			}
			
			weights[tb.Name] = tb.Value;
			
		}


		private void tbar_rnd_maxpilots_Scroll( object sender, EventArgs e ) {

		}
	}

}



