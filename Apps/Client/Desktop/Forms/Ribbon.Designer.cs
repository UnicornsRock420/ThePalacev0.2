using ThePalace.Client.Desktop.Entities;

namespace ThePalace.Core.Desktop.Plugins.Forms
{
    public partial class Ribbon : FormBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon));
            toolStrip = new ToolStrip();
            tsButtonConnection = new ToolStripButton();
            tsDDBBookmarks = new ToolStripDropDownButton();
            tsButtonLiveDirectory = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            tsButtonBack = new ToolStripButton();
            tsDDBBack = new ToolStripDropDownButton();
            tsMIPreviousRoom = new ToolStripMenuItem();
            tsButtonForward = new ToolStripButton();
            tsDDBForward = new ToolStripDropDownButton();
            toolStripSeparator2 = new ToolStripSeparator();
            tsButtonUserlist = new ToolStripButton();
            tsDDBUserlist = new ToolStripDropDownButton();
            tsButtonRoomlist = new ToolStripButton();
            tsDDBRoomlist = new ToolStripDropDownButton();
            tsButtonSounds = new ToolStripButton();
            tsDDBSounds = new ToolStripDropDownButton();
            tsButtonDraw = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            tsButtonDoorOutlines = new ToolStripButton();
            tsButtonUserNametags = new ToolStripButton();
            tsButtonChatlog = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            tsButtonTabsOff = new ToolStripButton();
            tsButtonTabsOn = new ToolStripButton();
            tsButtonTerminal = new ToolStripButton();
            tsButtonSuperUser = new ToolStripButton();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.GripMargin = new Padding(0);
            toolStrip.ImageScalingSize = new Size(38, 38);
            toolStrip.Items.AddRange(new ToolStripItem[] { tsButtonConnection, tsDDBBookmarks, tsButtonLiveDirectory, toolStripSeparator1, tsButtonBack, tsDDBBack, tsButtonForward, tsDDBForward, toolStripSeparator2, tsButtonUserlist, tsDDBUserlist, tsButtonRoomlist, tsDDBRoomlist, tsButtonSounds, tsDDBSounds, tsButtonDraw, toolStripSeparator3, tsButtonDoorOutlines, tsButtonUserNametags, tsButtonChatlog, toolStripSeparator4, tsButtonTabsOff, tsButtonTabsOn, tsButtonTerminal, tsButtonSuperUser });
            toolStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(0);
            toolStrip.Size = new Size(1034, 45);
            toolStrip.Stretch = true;
            toolStrip.TabIndex = 0;
            toolStrip.ItemClicked += toolStrip_ItemClicked;
            // 
            // tsButtonConnection
            // 
            tsButtonConnection.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonConnection.Image = (Image)resources.GetObject("tsButtonConnection.Image");
            tsButtonConnection.ImageTransparentColor = Color.Magenta;
            tsButtonConnection.Name = "tsButtonConnection";
            tsButtonConnection.Size = new Size(42, 42);
            tsButtonConnection.Text = "Connection";
            tsButtonConnection.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonConnection.ToolTipText = "Connection";
            // 
            // tsDDBBookmarks
            // 
            tsDDBBookmarks.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsDDBBookmarks.Image = (Image)resources.GetObject("tsDDBBookmarks.Image");
            tsDDBBookmarks.ImageTransparentColor = Color.Magenta;
            tsDDBBookmarks.Name = "tsDDBBookmarks";
            tsDDBBookmarks.Size = new Size(51, 42);
            tsDDBBookmarks.Text = "Bookmarks";
            tsDDBBookmarks.TextImageRelation = TextImageRelation.ImageAboveText;
            tsDDBBookmarks.ToolTipText = "Bookmarks";
            tsDDBBookmarks.Click += toolStripMenuItem_Click;
            // 
            // tsButtonLiveDirectory
            // 
            tsButtonLiveDirectory.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonLiveDirectory.Image = (Image)resources.GetObject("tsButtonLiveDirectory.Image");
            tsButtonLiveDirectory.ImageTransparentColor = Color.Magenta;
            tsButtonLiveDirectory.Name = "tsButtonLiveDirectory";
            tsButtonLiveDirectory.Size = new Size(42, 42);
            tsButtonLiveDirectory.Text = "Live Directory";
            tsButtonLiveDirectory.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonLiveDirectory.ToolTipText = "Live Directory";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.AutoSize = false;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 45);
            // 
            // tsButtonBack
            // 
            tsButtonBack.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonBack.Image = (Image)resources.GetObject("tsButtonBack.Image");
            tsButtonBack.ImageTransparentColor = Color.Magenta;
            tsButtonBack.Name = "tsButtonBack";
            tsButtonBack.Size = new Size(42, 42);
            tsButtonBack.Text = "Go Back";
            tsButtonBack.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonBack.ToolTipText = "Go Back";
            // 
            // tsDDBBack
            // 
            tsDDBBack.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsDDBBack.DropDownItems.AddRange(new ToolStripItem[] { tsMIPreviousRoom });
            tsDDBBack.Image = (Image)resources.GetObject("tsDDBBack.Image");
            tsDDBBack.ImageTransparentColor = Color.Magenta;
            tsDDBBack.Name = "tsDDBBack";
            tsDDBBack.Size = new Size(51, 42);
            tsDDBBack.Text = "Go Back";
            tsDDBBack.TextImageRelation = TextImageRelation.ImageAboveText;
            tsDDBBack.ToolTipText = "Go Back";
            // 
            // tsMIPreviousRoom
            // 
            tsMIPreviousRoom.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsMIPreviousRoom.Name = "tsMIPreviousRoom";
            tsMIPreviousRoom.Size = new Size(154, 22);
            tsMIPreviousRoom.Text = "Previous Room";
            tsMIPreviousRoom.TextImageRelation = TextImageRelation.ImageAboveText;
            tsMIPreviousRoom.ToolTipText = "Previous Room";
            tsMIPreviousRoom.Click += toolStripMenuItem_Click;
            // 
            // tsButtonForward
            // 
            tsButtonForward.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonForward.Image = (Image)resources.GetObject("tsButtonForward.Image");
            tsButtonForward.ImageTransparentColor = Color.Magenta;
            tsButtonForward.Name = "tsButtonForward";
            tsButtonForward.Size = new Size(42, 42);
            tsButtonForward.Text = "Go Forward";
            tsButtonForward.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonForward.ToolTipText = "Go Forward";
            // 
            // tsDDBForward
            // 
            tsDDBForward.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsDDBForward.Image = (Image)resources.GetObject("tsDDBForward.Image");
            tsDDBForward.ImageTransparentColor = Color.Magenta;
            tsDDBForward.Name = "tsDDBForward";
            tsDDBForward.Size = new Size(51, 42);
            tsDDBForward.Text = "Go Forward";
            tsDDBForward.TextImageRelation = TextImageRelation.ImageAboveText;
            tsDDBForward.ToolTipText = "Go Forward";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.AutoSize = false;
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 45);
            // 
            // tsButtonUserlist
            // 
            tsButtonUserlist.CheckOnClick = true;
            tsButtonUserlist.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonUserlist.Image = (Image)resources.GetObject("tsButtonUserlist.Image");
            tsButtonUserlist.ImageTransparentColor = Color.Magenta;
            tsButtonUserlist.Name = "tsButtonUserlist";
            tsButtonUserlist.Size = new Size(42, 42);
            tsButtonUserlist.Text = "Users List";
            tsButtonUserlist.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonUserlist.ToolTipText = "Users List";
            // 
            // tsDDBUserlist
            // 
            tsDDBUserlist.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsDDBUserlist.Image = (Image)resources.GetObject("tsDDBUserlist.Image");
            tsDDBUserlist.ImageTransparentColor = Color.Magenta;
            tsDDBUserlist.Name = "tsDDBUserlist";
            tsDDBUserlist.Size = new Size(51, 42);
            tsDDBUserlist.Text = "Users List";
            tsDDBUserlist.TextImageRelation = TextImageRelation.ImageAboveText;
            tsDDBUserlist.ToolTipText = "Users List";
            // 
            // tsButtonRoomlist
            // 
            tsButtonRoomlist.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonRoomlist.Image = (Image)resources.GetObject("tsButtonRoomlist.Image");
            tsButtonRoomlist.ImageTransparentColor = Color.Magenta;
            tsButtonRoomlist.Name = "tsButtonRoomlist";
            tsButtonRoomlist.Size = new Size(42, 42);
            tsButtonRoomlist.Text = "Rooms List";
            tsButtonRoomlist.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonRoomlist.ToolTipText = "Rooms List";
            // 
            // tsDDBRoomlist
            // 
            tsDDBRoomlist.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsDDBRoomlist.Image = (Image)resources.GetObject("tsDDBRoomlist.Image");
            tsDDBRoomlist.ImageTransparentColor = Color.Magenta;
            tsDDBRoomlist.Name = "tsDDBRoomlist";
            tsDDBRoomlist.Size = new Size(51, 42);
            tsDDBRoomlist.Text = "Rooms List";
            tsDDBRoomlist.TextImageRelation = TextImageRelation.ImageAboveText;
            tsDDBRoomlist.ToolTipText = "Rooms List";
            // 
            // tsButtonSounds
            // 
            tsButtonSounds.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonSounds.Image = (Image)resources.GetObject("tsButtonSounds.Image");
            tsButtonSounds.ImageTransparentColor = Color.Magenta;
            tsButtonSounds.Name = "tsButtonSounds";
            tsButtonSounds.Size = new Size(42, 42);
            tsButtonSounds.Text = "Sounds";
            tsButtonSounds.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonSounds.ToolTipText = "Sounds";
            // 
            // tsDDBSounds
            // 
            tsDDBSounds.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsDDBSounds.Image = (Image)resources.GetObject("tsDDBSounds.Image");
            tsDDBSounds.ImageTransparentColor = Color.Magenta;
            tsDDBSounds.Name = "tsDDBSounds";
            tsDDBSounds.Size = new Size(51, 42);
            tsDDBSounds.Text = "Sounds";
            tsDDBSounds.TextImageRelation = TextImageRelation.ImageAboveText;
            tsDDBSounds.ToolTipText = "Sounds";
            // 
            // tsButtonDraw
            // 
            tsButtonDraw.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonDraw.Image = (Image)resources.GetObject("tsButtonDraw.Image");
            tsButtonDraw.ImageTransparentColor = Color.Magenta;
            tsButtonDraw.Name = "tsButtonDraw";
            tsButtonDraw.Size = new Size(42, 42);
            tsButtonDraw.Text = "Draw";
            tsButtonDraw.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonDraw.ToolTipText = "Draw";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.AutoSize = false;
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 45);
            // 
            // tsButtonDoorOutlines
            // 
            tsButtonDoorOutlines.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonDoorOutlines.Image = (Image)resources.GetObject("tsButtonDoorOutlines.Image");
            tsButtonDoorOutlines.ImageTransparentColor = Color.Magenta;
            tsButtonDoorOutlines.Name = "tsButtonDoorOutlines";
            tsButtonDoorOutlines.Size = new Size(42, 42);
            tsButtonDoorOutlines.Text = "Door Outlines";
            tsButtonDoorOutlines.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonDoorOutlines.ToolTipText = "Door Outlines";
            // 
            // tsButtonUserNametags
            // 
            tsButtonUserNametags.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonUserNametags.Image = (Image)resources.GetObject("tsButtonUserNametags.Image");
            tsButtonUserNametags.ImageTransparentColor = Color.Magenta;
            tsButtonUserNametags.Name = "tsButtonUserNametags";
            tsButtonUserNametags.Size = new Size(42, 42);
            tsButtonUserNametags.Text = "User Nametags";
            tsButtonUserNametags.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonUserNametags.ToolTipText = "User Nametags";
            // 
            // tsButtonChatlog
            // 
            tsButtonChatlog.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonChatlog.Image = (Image)resources.GetObject("tsButtonChatlog.Image");
            tsButtonChatlog.ImageTransparentColor = Color.Magenta;
            tsButtonChatlog.Name = "tsButtonChatlog";
            tsButtonChatlog.Size = new Size(42, 42);
            tsButtonChatlog.Text = "Chatlog";
            tsButtonChatlog.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonChatlog.ToolTipText = "Chatlog";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.AutoSize = false;
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 45);
            // 
            // tsButtonTabsOff
            // 
            tsButtonTabsOff.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonTabsOff.Image = (Image)resources.GetObject("tsButtonTabsOff.Image");
            tsButtonTabsOff.ImageTransparentColor = Color.Magenta;
            tsButtonTabsOff.Name = "tsButtonTabsOff";
            tsButtonTabsOff.Size = new Size(42, 42);
            tsButtonTabsOff.Text = "Windowed UI";
            tsButtonTabsOff.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonTabsOff.ToolTipText = "Windowed UI";
            // 
            // tsButtonTabsOn
            // 
            tsButtonTabsOn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonTabsOn.Image = (Image)resources.GetObject("tsButtonTabsOn.Image");
            tsButtonTabsOn.ImageTransparentColor = Color.Magenta;
            tsButtonTabsOn.Name = "tsButtonTabsOn";
            tsButtonTabsOn.Size = new Size(42, 42);
            tsButtonTabsOn.Text = "Tabbed UI";
            tsButtonTabsOn.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonTabsOn.ToolTipText = "Tabbed UI";
            // 
            // tsButtonTerminal
            // 
            tsButtonTerminal.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonTerminal.Image = (Image)resources.GetObject("tsButtonTerminal.Image");
            tsButtonTerminal.ImageTransparentColor = Color.Magenta;
            tsButtonTerminal.Name = "tsButtonTerminal";
            tsButtonTerminal.Size = new Size(42, 42);
            tsButtonTerminal.Text = "Terminal";
            tsButtonTerminal.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonTerminal.ToolTipText = "Terminal";
            // 
            // tsButtonSuperUser
            // 
            tsButtonSuperUser.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsButtonSuperUser.Image = (Image)resources.GetObject("tsButtonSuperUser.Image");
            tsButtonSuperUser.ImageTransparentColor = Color.Magenta;
            tsButtonSuperUser.Name = "tsButtonSuperUser";
            tsButtonSuperUser.Size = new Size(42, 42);
            tsButtonSuperUser.Text = "SuperUser";
            tsButtonSuperUser.TextImageRelation = TextImageRelation.ImageAboveText;
            tsButtonSuperUser.ToolTipText = "SuperUser";
            // 
            // Test
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1034, 450);
            Controls.Add(toolStrip);
            Name = "Test";
            Text = "Test";
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip;
        private ToolStripDropDownButton tsDDBBack;
        private ToolStripDropDownButton tsDDBForward;
        private ToolStripButton tsButtonUserNametags;
        private ToolStripButton tsButtonLiveDirectory;
        private ToolStripButton tsButtonDoorOutlines;
        private ToolStripButton tsButtonSounds;
        private ToolStripButton tsButtonBack;
        private ToolStripButton tsButtonForward;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsButtonUserlist;
        private ToolStripButton tsButtonRoomlist;
        private ToolStripButton tsButtonDraw;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsButtonChatlog;
        private ToolStripButton tsButtonTerminal;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton tsButtonSuperUser;
        private ToolStripDropDownButton tsDDBBookmarks;
        private ToolStripDropDownButton tsDDBUserlist;
        private ToolStripDropDownButton tsDDBRoomlist;
        private ToolStripDropDownButton tsDDBSounds;
        private ToolStripMenuItem tsMIPreviousRoom;
        private ToolStripButton tsButtonTabsOn;
        private ToolStripButton tsButtonTabsOff;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton tsButtonConnection;
    }
}