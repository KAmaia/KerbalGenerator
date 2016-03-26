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
		private KerbalGenerator generator;

		public frm_Krb_Gen( ) {
			InitializeComponent( );
			generator = new KerbalGenerator( this );
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
		}


		private void frm_Krb_Gen_Load( object sender, EventArgs e ) {
			Text = "Kerbal Generator -- " + generator.Cfg.Name;
			cmb_AvailSaves.Items.AddRange( generator.GetSaves( ) );
			cmb_AvailSaves.SelectedIndex = 0;
		}

		#region Stats Panel

		internal void UpdateSaveStats( string currentSavePath, Dictionary<string, int> countedKerbals ) {
			//Update Our Current Save Path
			lbl_currentSaveLocation.Text = currentSavePath;
			//Update Our Current Kerbal counts
			lbl_si_kerbcountdisp.Text = countedKerbals["Total"].ToString();
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

		

		#endregion

		#region Available Saves Panel
		private void cmb_AvailSaves_SelectedIndexChanged( object sender, EventArgs e ) {
			string saveName = cmb_AvailSaves.SelectedItem.ToString();
			generator.SelectSave( saveName );
			generator.UpdateSaveStats( );
			cmb_kerb_list.Items.Clear( );
			cmb_kerb_list.Items.AddRange( generator.GetRosterNames() );
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

			foreach (KeyValuePair<string, string> stat in currentKerbal.Stats ) {
				
				if ( stat.Key == "gender" ) {
					lbl_ki_genderdisp.Text = stat.Value;
				}
				if ( stat.Key == "trait" ) {
					lbl_ki_roledisp.Text = stat.Value;
				}
				if(stat.Key == "type" ) {
					lbl_ki_statusdisp.Text = stat.Value;
				}
				if ( stat.Key == "badS" ) {
					lbl_ki_badsdisp.Text = stat.Value;
				}
				if ( stat.Key == "tour" ) {
					lbl_ki_tourdisp.Text = stat.Value;
				}
				if(stat.Key == "brave" ) {
					lbl_ki_bravedisp.Text = stat.Value;
				}
				if(stat.Key == "dumb" ) {
					lbl_ki_stupiddisp.Text = stat.Value;
				}
				if(stat.Key == "state" ) {
					lbl_ki_statedisp.Text = stat.Value;
				}

			}
		}
		private void cmb_kerb_list_SelectedIndexChanged( object sender, EventArgs e ) {
			generator.SelectKerbal( cmb_kerb_list.SelectedItem.ToString());

		}

		#endregion

		#region Random Kerbal Generation
		private void btn_gen_List_Kerb_Click( object sender, EventArgs e ) {

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

		}

		private void tbar_spe_stupid_Scroll( object sender, EventArgs e ) {
			lbl_spe_stupiddisp.Text = tbar_spe_stupid.Value.ToString( );
		}

		private void tbar_spe_brave_Scroll( object sender, EventArgs e ) {
			lbl_spe_bravedisp.Text = tbar_spe_brave.Value.ToString( );
		}

		private void chk_spe_lastNameKerman_CheckedChanged( object sender, EventArgs e ) {
			txt_spe_kerbname.MaxLength = chk_spe_lastNameKerman.Checked ? 11 : 18;
		}

		private void txt_spe_kerbname_TextChanged( object sender, EventArgs e ) {
			//Disable the generate button if our name is blank.
			if ( !chk_spe_rndName.Checked ) {
				btn_spe_generate.Enabled = !( txt_spe_kerbname.Text == "" );
			}
		}
		private void chk_spe_rndName_CheckedChanged( object sender, EventArgs e ) {
			txt_spe_kerbname.Text = "";
			txt_spe_kerbname.Enabled = !chk_spe_rndName.Checked;
			btn_spe_generate.Enabled = true;
		}

		private void btn_spe_generate_Click( object sender, EventArgs e ) {

		}

		private string DetermineSpecificTrait( ) {
			
			if ( rd_spe_engi.Checked ) {
				return "engineer";
			}
			if ( rd_spe_sci.Checked ) {
				return "scientist";
			}
			else {
				return "pilot";

			}
		}

		#endregion

		private void chk_spe_rndBrave_CheckedChanged( object sender, EventArgs e ) {
			if ( chk_spe_rndBrave.Checked ) {
				tbar_spe_brave.Value = 0;
				lbl_spe_bravedisp.Text = "";
			}
			tbar_spe_brave.Enabled = !chk_spe_rndBrave.Checked;
		}

		private void chk_spe_rndStupid_CheckedChanged( object sender, EventArgs e ) {
			if ( chk_spe_rndStupid.Checked ) {
				tbar_spe_stupid.Value = 0;
				lbl_spe_stupiddisp.Text = "";
			}
			tbar_spe_stupid.Enabled = !chk_spe_rndStupid.Checked;
		}

		private void menuStrip1_ItemClicked( object sender, ToolStripItemClickedEventArgs e ) {

		}

		private void fileToolStripMenuItem_Click( object sender, EventArgs e ) {

		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e ) {
			Application.Exit( );
		}

		private void btn_po_Save_Click( object sender, EventArgs e ) {
			generator.Save( cmb_AvailSaves.SelectedItem.ToString( ) );
		}
	}

}



