﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KerbalGenerator {
	public partial class ConfiguratorForm : Form {
		Configurator cfgr;
		bool firstRun;

		public ConfiguratorForm( Configurator cfgr, bool firstRun ) {
			InitializeComponent( );
			this.firstRun = firstRun;
			this.cfgr = cfgr;
		}

		private void btn_Cancel_Click( object sender, EventArgs e ) {
			if ( firstRun ) {
				Environment.Exit( 1 );
			}
			else {
				this.Close( );
			}
		}

		private void btn_create_config_Click( object sender, EventArgs e ) {
			if ( cfgr.ValidatePath( fbd_SelectSavePath.SelectedPath ) ) {
				cfgr.CreateConfig( txt_ConfigName.Text, fbd_SelectSavePath.SelectedPath );
				this.Close( );
			}
			else {
				DialogResult result = MessageBox.Show("Error! Invalid Path!  Try Again?","Invalid Path", MessageBoxButtons.YesNo);
				if ( result == DialogResult.No ) {
					if ( firstRun ) {
						Application.Exit( );
					}
					else {
						this.Close( );
					}
				}
			}
		}




		private void ShowPathError( string s, bool success ) {
			lbl_PathErr.ForeColor = success ? Color.Green : Color.Red;
			lbl_PathErr.Text = s;
		}

		private void txt_kspPath_TextChanged( object sender, EventArgs e ) {
			bool validPath = cfgr.ValidatePath ( txt_kspPath.Text );
			ShowPathError( validPath ? "Valid Path Found" : "Sorry, That Path Does Not Exist", validPath );
			if ( validPath ) {
				validPath = cfgr.countSavesInPath( txt_kspPath.Text ) > 0;
				ShowPathError( validPath ? "Valid Saves Found.  Click Create Config to Continue" : "Sorry, no Valid Saves in that directory", validPath );
			}
		}

		private void folderBrowserDialog1_HelpRequest( object sender, EventArgs e ) {

		}

		private void btn_show_FolderDialog_Click( object sender, EventArgs e ) {
			fbd_SelectSavePath.ShowDialog( );
		}
	}
}
