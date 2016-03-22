﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KerbalGenerator {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		///
		[STAThread]
		static void Main( ) {
			string configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Badwater\\KerbalGen";

			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
			if ( firstRun( configPath ) ) {
				Application.Run( new Configurator( configPath ) );
			}
			Application.Run( new frm_Krb_Gen( ) );
			
		}

		private static bool firstRun( string configPath ) {
			return !File.Exists( configPath + "\\config.xml" );
		}
	}
}
