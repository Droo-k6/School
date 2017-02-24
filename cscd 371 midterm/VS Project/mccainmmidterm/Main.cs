// Mathew McCain
// cscd371 midterm project
// contains main entry point for program

using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace mccainmmidterm {
	static class MainClass {
		// Main method
		[STAThread]
		static void Main() {
			// Run form
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormMonitor());
			// Form closed, nothing else
		}

		// static method to create List<String> from comma seperated extension names
		// removes '.' from entries as wlel
		// used by FormMonitor and FormQuery
		public static List<string> ExtStrToList(string extStr) {
			// Strip whitespace
			extStr = extStr.Trim();

			List<string> extList = new List<string>();

			// if empty, skip
			if (extStr.Length > 0) {
				// tokenize by commas
				string[] tokens = extStr.Split(',');
				foreach (string tok in tokens) {
					// strip whitespace and trim '.' from beginning
					// convert to lowercase
					string ntok = tok.Trim().TrimStart('.').ToLower();
					// if not empty, add
					if (ntok.Length > 0) {
						// add if not contained
						if (!extList.Contains(ntok))
							extList.Add(ntok);
					}
				}
			}

			return extList;
		}
	}
}
