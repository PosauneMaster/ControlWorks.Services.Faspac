using ControlWorks.Services.Configuration;
using ControlWorks.Services.Pvi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestHarness
{
    public partial class frmVariables : Form
    {
        public frmVariables()
        {
            InitializeComponent();
        }

        private void btnAddVariables_Click(object sender, EventArgs e)
        {

            var filepath = ConfigurationProvider.VariableSettingsFile;
            
            var collection = new VariableInfoCollection();
            collection.Open(filepath);
            var cpuName = txtCpuName.Text;
            var names = txtVariables.Lines;

            collection.AddRange(cpuName, names);
            collection.Save(filepath);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var filepath = ConfigurationProvider.VariableSettingsFile;

            var collection = new VariableInfoCollection();
            collection.Open(filepath);
            var cpuName = txtCpuName.Text;
            var names = txtVariables.Lines;

            collection.RemoveRange(cpuName, names);
            collection.Save(filepath);


        }
    }
}
