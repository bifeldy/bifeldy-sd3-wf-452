/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Halaman Awal
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using bifeldy_sd3_lib_452.Utilities;

using bifeldy_sd3_wf_452.Forms;
using bifeldy_sd3_wf_452.Handlers;
using bifeldy_sd3_wf_452.Utilities;

namespace bifeldy_sd3_wf_452.Panels {

    public sealed partial class CMainPanel : UserControl {

        private readonly IApplication _application;
        private readonly IApp _app;
        private readonly ILogger _logger;
        private readonly IDb _db;
        private readonly IConfig _config;
        private readonly IWinReg _winreg;

        private CMainForm mainForm;

        private bool isInitialized = false;

        public CMainPanel(
            IApplication application, IApp app,
            ILogger logger,
            IDb db,
            IConfig config,
            IWinReg winreg
        ) {
            this._application = application;
            this._app = app;
            this._logger = logger;
            this._db = db;
            this._config = config;
            this._winreg = winreg;

            this.InitializeComponent();
            this.OnInit();
        }

        public Label LabelStatus => this.lblStatus;

        public ProgressBar ProgressBarStatus => this.prgrssBrStatus;

        private void OnInit() {
            this.Dock = DockStyle.Fill;
        }

        private void ImgDomar_Click(object sender, EventArgs e) {
            this.mainForm.Width = 800;
            this.mainForm.Height = 600;
        }

        private async void CMainPanel_Load(object sender, EventArgs e) {
            if (!this.isInitialized) {

                this.mainForm = (CMainForm)this.Parent.Parent;
                this.mainForm.FormBorderStyle = FormBorderStyle.Sizable;
                this.mainForm.MaximizeBox = true;
                this.mainForm.MinimizeBox = true;

                this.appInfo.Text = this._app.AppName;
                string dcKode = null;
                string namaDc = null;
                await Task.Run(async () => {
                    dcKode = await this._db.GetKodeDc();
                    namaDc = await this._db.GetNamaDc();
                });
                this.userInfo.Text = $".: {dcKode} - {namaDc} :: {this._db.LoggedInUsername} :.";

                bool windowsStartup = this._config.Get<bool>("WindowsStartup", bool.Parse(this._app.GetConfig("windows_startup")));
                this.chkWindowsStartup.Checked = windowsStartup;

                //
                // TODO :: Here Maybe Some Code ...
                //

                this.SetIdleBusyStatus(true);

                this.isInitialized = true;
            }

            this.SetIdleBusyStatus(this._app.IsIdle);
        }

        public void SetIdleBusyStatus(bool isIdle) {
            this._app.IsIdle = isIdle;
            this._application.IsIdle = this._app.IsIdle;
            this.LabelStatus.Text = $"Program {(isIdle ? "Idle" : "Sibuk")} ...";
            this.ProgressBarStatus.Style = isIdle ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
            this.EnableDisableControl(this.Controls, isIdle);
        }

        private void EnableDisableControl(ControlCollection controls, bool isIdle) {
            foreach (Control control in controls) {
                if (control is Button || control is CheckBox || control is DateTimePicker) {
                    control.Enabled = isIdle;
                }
                else {
                    this.EnableDisableControl(control.Controls, isIdle);
                }
            }
        }

        private void ChkWindowsStartup_CheckedChanged(object sender, EventArgs e) {
            CheckBox cb = (CheckBox) sender;
            this._config.Set("WindowsStartup", cb.Checked);
            this._winreg.SetWindowsStartup(cb.Checked);
        }

    }

}
