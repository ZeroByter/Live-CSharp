using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Run_Live_CSharp
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            Properties.Settings.Default.Reset();

            assemblies.Text = Properties.Settings.Default.ReferenceAssemblies;
        }

        private void SettingsForm_Deactivate(object sender, EventArgs e)
        {
            Properties.Settings.Default.ReferenceAssemblies = assemblies.Text;
            Properties.Settings.Default.Save();
        }
    }
}
