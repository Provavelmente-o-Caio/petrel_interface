using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Slb.Ocean.Petrel.Commands;
using Slb.Ocean.Petrel;

namespace dlseis_well_tie_petrel
{
    class Automatic_Seismic_to_Well_Tie : SimpleCommandHandler
    {
        public static string ID = "dlseis_well_tie_petrel.Automatic_Seismic_to_Well_Tie";

        #region SimpleCommandHandler Members

        public override bool CanExecute(Slb.Ocean.Petrel.Contexts.Context context)
        { 
            return true;
        }

        public override void Execute(Slb.Ocean.Petrel.Contexts.Context context)
        {
            //TODO: Add command execution logic here
            PetrelLogger.InfoOutputWindow(string.Format("{0} clicked", @"Automatic Seismic to Well Tie" ));
            var instanceWell_tie_window = new well_tie_window();
            instanceWell_tie_window.Show();
        }
    
        #endregion
    }
}
