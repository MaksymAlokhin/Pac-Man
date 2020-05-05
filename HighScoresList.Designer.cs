namespace PacMan
{
    partial class HighScoresList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HighScoresList));
            this.HighScoresListView = new System.Windows.Forms.ListView();
            this.Player = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Score = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResetBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // HighScoresListView
            // 
            this.HighScoresListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Player,
            this.Score});
            this.HighScoresListView.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HighScoresListView.HideSelection = false;
            this.HighScoresListView.Location = new System.Drawing.Point(14, 15);
            this.HighScoresListView.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.HighScoresListView.Name = "HighScoresListView";
            this.HighScoresListView.Size = new System.Drawing.Size(376, 271);
            this.HighScoresListView.TabIndex = 0;
            this.HighScoresListView.UseCompatibleStateImageBehavior = false;
            this.HighScoresListView.View = System.Windows.Forms.View.Details;
            // 
            // Player
            // 
            this.Player.Text = "Player";
            this.Player.Width = 186;
            // 
            // Score
            // 
            this.Score.Text = "Score";
            this.Score.Width = 171;
            // 
            // ResetBtn
            // 
            this.ResetBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ResetBtn.Location = new System.Drawing.Point(81, 295);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(75, 23);
            this.ResetBtn.TabIndex = 1;
            this.ResetBtn.Text = "Reset";
            this.ResetBtn.UseVisualStyleBackColor = true;
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OKBtn.Location = new System.Drawing.Point(223, 295);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // HighScoresList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 346);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.ResetBtn);
            this.Controls.Add(this.HighScoresListView);
            this.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "HighScoresList";
            this.Text = "HighScores";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColumnHeader Player;
        private System.Windows.Forms.ColumnHeader Score;
        public System.Windows.Forms.ListView HighScoresListView;
        private System.Windows.Forms.Button ResetBtn;
        private System.Windows.Forms.Button OKBtn;
    }
}