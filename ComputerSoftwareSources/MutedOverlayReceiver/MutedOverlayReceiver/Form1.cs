using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MutedOverlayReceiver {
	public partial class Form1 : Form {
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		const int MUTE_HOTKEY_ID = 1;
		const int UNMUTE_HOTKEY_ID = 2;


		public int color = 25;
		public int colorDir = 2;

		public bool muted = false;

		SvgDocument svgDoc;
		VBANText vbt;

		public Form1() {
			InitializeComponent();
			vbt = new VBANText(IPAddress.Parse("127.0.0.1"), 6980);
		}

		private void recolorSVGDoc(IEnumerable<SvgElement> nodes, SvgPaintServer colorServer) {
			foreach (var node in nodes) {
				if (node.Fill != SvgPaintServer.None) node.Fill = colorServer;
				if (node.Color != SvgPaintServer.None) node.Color = colorServer;
				if (node.Stroke != SvgPaintServer.None) node.Stroke = colorServer;
				recolorSVGDoc(node.Descendants(), colorServer);
			}
		}

		private void Form1_Load(object sender, EventArgs e) {
			this.TransparencyKey = this.BackColor;
			svgImage.Location = new Point(0, 0);
			this.Size = svgImage.Size;



			svgDoc = SvgDocument.Open("C:\\Users\\tugle\\Downloads\\microphone-black-shape.svg");
			recolorSVGDoc(svgDoc.Descendants(), new SvgColourServer(Color.FromArgb(255, 255, 64, 0)));
			svgImage.Image = svgDoc.Draw();
			timer1.Enabled = true;


			// ALT+CTRL = 1 + 2 = 3 , CTRL+SHIFT = 2 + 4 = 6...
			RegisterHotKey(this.Handle,   MUTE_HOTKEY_ID, 3, (int)Keys.F2); ;
			RegisterHotKey(this.Handle, UNMUTE_HOTKEY_ID, 3, (int)Keys.F3); ;

			this.notifyIcon1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
			this.notifyIcon1.ContextMenuStrip.Items.Add("Exit", null, this.MenuExit_Click);
		}

		private void MenuExit_Click(object sender, EventArgs e) {
			oneShotUnMute();
			Application.Exit();
		}


		protected override void WndProc(ref Message m) {
			if (m.Msg == 0x0312 && m.WParam.ToInt32() == MUTE_HOTKEY_ID) {
				oneShotMute();
			}
			if (m.Msg == 0x0312 && m.WParam.ToInt32() == UNMUTE_HOTKEY_ID) {
				oneShotUnMute();
			}
			base.WndProc(ref m);
		}

		private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			timer2.Enabled = false;
		}
		private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			timer2.Enabled = true;
		}
		private void Form1_DoubleClick(object sender, System.Windows.Forms.MouseEventArgs e) {
			oneShotUnMute();
		}


		private void timer1_Tick(object sender, EventArgs e) {

			if (muted) {
				this.Visible = true;
				this.TopMost = true;
				color += colorDir;
				if (color > 20 || color < 1) colorDir *= -1;
				if (color > 20) color = 15;
				if (color < 0) color = 0;
				this.Opacity = (color + 80) / 100.0;
			} else {
				this.Visible = false;
				timer2.Enabled = false;
			}
		}

		private void timer2_Tick(object sender, EventArgs e) {
			Point p = Control.MousePosition;
			p.Offset(-this.Size.Width / 2, -this.Size.Height / 2);
			this.Location = p;
		}


		void oneShotMute() {
			Console.WriteLine("MUTE");
			muted = true;
			vbt.send("Strip(0).Mute = 1");
		}
		void oneShotUnMute() {
			Console.WriteLine("UNMUTE");
			muted = false;
			vbt.send("Strip(0).Mute = 0");
		}
	}
}
