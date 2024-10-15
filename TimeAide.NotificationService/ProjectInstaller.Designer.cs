namespace TimeAide.NotificationService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TimeAideNotificationServiceInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.TimeAideNotificationService = new System.ServiceProcess.ServiceInstaller();
            // 
            // TimeAideNotificationServiceInstaller
            // 
            this.TimeAideNotificationServiceInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.TimeAideNotificationServiceInstaller.Password = null;
            this.TimeAideNotificationServiceInstaller.Username = null;
            // 
            // TimeAideNotificationService
            // 
            this.TimeAideNotificationService.ServiceName = "TimeAideNotificationService";
            this.TimeAideNotificationService.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.TimeAideNotificationServiceInstaller,
            this.TimeAideNotificationService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller TimeAideNotificationServiceInstaller;
        private System.ServiceProcess.ServiceInstaller TimeAideNotificationService;
    }
}