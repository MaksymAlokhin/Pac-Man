namespace PacMan
{
    partial class PacForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PacForm));
            this.tmr_movement = new System.Windows.Forms.Timer(this.components);
            this.tmr_refresh = new System.Windows.Forms.Timer(this.components);
            this.tmr_animation = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.invincBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.energyBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.highScoresToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmr_movement
            // 
            this.tmr_movement.Interval = 16;
            this.tmr_movement.Tick += new System.EventHandler(this.MovementTick);
            // 
            // tmr_refresh
            // 
            this.tmr_refresh.Interval = 16;
            this.tmr_refresh.Tick += new System.EventHandler(this.RefreshScreenTick);
            // 
            // tmr_animation
            // 
            this.tmr_animation.Interval = 33;
            this.tmr_animation.Tick += new System.EventHandler(this.AnimationTick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.pauseToolStripMenu,
            this.invincBtn,
            this.energyBtn,
            this.highScoresToolStripMenu,
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            resources.ApplyResources(this.gameToolStripMenuItem, "gameToolStripMenuItem");
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            resources.ApplyResources(this.startToolStripMenuItem, "startToolStripMenuItem");
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // pauseToolStripMenu
            // 
            this.pauseToolStripMenu.Name = "pauseToolStripMenu";
            resources.ApplyResources(this.pauseToolStripMenu, "pauseToolStripMenu");
            this.pauseToolStripMenu.Click += new System.EventHandler(this.pauseToolStripMenu_Click);
            // 
            // invincBtn
            // 
            this.invincBtn.CheckOnClick = true;
            this.invincBtn.Name = "invincBtn";
            resources.ApplyResources(this.invincBtn, "invincBtn");
            // 
            // energyBtn
            // 
            this.energyBtn.CheckOnClick = true;
            this.energyBtn.Name = "energyBtn";
            resources.ApplyResources(this.energyBtn, "energyBtn");
            // 
            // highScoresToolStripMenu
            // 
            this.highScoresToolStripMenu.Name = "highScoresToolStripMenu";
            resources.ApplyResources(this.highScoresToolStripMenu, "highScoresToolStripMenu");
            this.highScoresToolStripMenu.Click += new System.EventHandler(this.highScoresToolStripMenu_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // PacForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PacForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Timer tmr_movement;
        public System.Windows.Forms.Timer tmr_refresh;
        public System.Windows.Forms.Timer tmr_animation;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highScoresToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenu;
        public System.Windows.Forms.ToolStripMenuItem invincBtn;
        public System.Windows.Forms.ToolStripMenuItem energyBtn;
    }
}

