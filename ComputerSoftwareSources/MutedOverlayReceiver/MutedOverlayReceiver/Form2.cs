using System;
using System.Windows.Forms;

namespace MutedOverlayReceiver {
	public partial class FormSettings : Form {
		public FormSettings() {
			InitializeComponent();
		}

		private void FormSettings_Load(object sender, EventArgs e) {
			this.textBox1.Text = MutedOverlayReceiver.Properties.Settings.Default.VoicemeeterIP;
			this.numericUpDown1.Value = MutedOverlayReceiver.Properties.Settings.Default.VoicemeeterPort;
			this.textBox2.Text = MutedOverlayReceiver.Properties.Settings.Default.VoicemeeterStream;
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void buttonSave_Click(object sender, EventArgs e) {

			Properties.Settings.Default.VoicemeeterIP = this.textBox1.Text;
			Properties.Settings.Default.VoicemeeterPort = (int)this.numericUpDown1.Value;
			Properties.Settings.Default.VoicemeeterStream = this.textBox2.Text;
			Properties.Settings.Default.Save();
			MessageBox.Show("Restart needed");
			this.Close();
		}

	}
}
