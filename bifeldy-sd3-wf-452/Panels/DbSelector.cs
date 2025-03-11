/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Database Selector
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System;
using System.Windows.Forms;

using bifeldy_sd3_lib_452.Utilities;

using bifeldy_sd3_wf_452.Forms;
using bifeldy_sd3_wf_452.Utilities;

namespace bifeldy_sd3_wf_452.Panels {

    public sealed partial class CDbSelector : UserControl {

        private readonly IApplication _application;
        private readonly IApp _app;

        private CMainForm mainForm;

        private bool isInitialized = false;

        public CDbSelector(IApplication application, IApp app) {
            this._application = application;
            this._app = app;

            this.InitializeComponent();
            this.OnInit();
        }

        private void OnInit() {
            this.Dock = DockStyle.Fill;
        }

        private void CDbSelector_Load(object sender, EventArgs e) {
            if (!this.isInitialized) {

                this.mainForm = (CMainForm)this.Parent.Parent;

                this.isInitialized = true;
            }
        }

        private void ShowCheckProgramPanel() {
            this.btnOracle.Enabled = false;
            this.btnPostgre.Enabled = false;

            // Create & Show `CekProgram` Panel
            try {
                CCekProgram cekProgram = null;
                if (!this.mainForm.PanelContainer.Controls.ContainsKey("CCekProgram")) {
                    cekProgram = CProgram.Bifeldyz.Resolve<CCekProgram>();
                    this.mainForm.PanelContainer.Controls.Add(cekProgram);
                }

                cekProgram = (CCekProgram)this.mainForm.PanelContainer.Controls["CCekProgram"];
                cekProgram.BringToFront();
            }
            catch (Exception ex) {
                _ = MessageBox.Show(ex.Message, "Terjadi Kesalahan! (｡>﹏<｡)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOracle_Click(object sender, EventArgs e) {
            this._app.IsUsingPostgres = false;
            this._application.IsUsingPostgres = this._app.IsUsingPostgres;
            this.ShowCheckProgramPanel();
        }

        private void BtnPostgre_Click(object sender, EventArgs e) {
            this._app.IsUsingPostgres = true;
            this._application.IsUsingPostgres = this._app.IsUsingPostgres;
            this.ShowCheckProgramPanel();
        }

        public void DchoOnlyBypass(object sender, EventArgs e) {
            this.BtnOracle_Click(sender, e);
        }

        public void AutoRunModeDefaultPostgre(object sender, EventArgs e) {
            this.BtnPostgre_Click(sender, e);
        }

    }

}
