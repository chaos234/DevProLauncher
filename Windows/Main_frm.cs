﻿using System;
using System.Windows.Forms;
using DevProLauncher.Config;
using DevProLauncher.Network.Enums;
using DevProLauncher.Helpers;
using System.Diagnostics;
using DevProLauncher.Windows.MessageBoxs;

namespace DevProLauncher.Windows
{
    public partial class MainFrm : Form
    {
        public HubGameList_frm GameWindow;
        LoginFrm m_loginWindow;
        ChatFrm m_chatWindow;
        SupportFrm m_devpointWindow;
        FileManagerFrm m_filemanagerWindow;
        CustomizeFrm m_customizerWindow;
        RankingFrm m_rankingWindow;

        public MainFrm()
        {
            InitializeComponent();

            Text = GetVersionString();

            LauncherHelper.LoadBanlist();

            var loginTab = new TabPage("Login");
            m_loginWindow = new LoginFrm();
            loginTab.Controls.Add(m_loginWindow);
            mainTabs.TabPages.Add(loginTab);
            m_chatWindow = new ChatFrm();
            GameWindow = new HubGameList_frm();
            m_rankingWindow = new RankingFrm();
            m_devpointWindow = new SupportFrm();
            m_filemanagerWindow = new FileManagerFrm();
            m_customizerWindow = new CustomizeFrm();

            Program.ChatServer.ServerMessage += ServerMessage;

            mainTabs.SelectedIndexChanged += TabChange;

            ApplyTranslation();

        }

        public string GetVersionString()
        {
            char[] version = Program.Version.ToCharArray();
            return "DevPro" + " v" + version[0] + "." + version[1] + "." + version[2] + " R" + (Program.Version[3].Equals('0') ? "" : Program.Version[3] + ".") + Program.Version[4] + "." + Program.Version[5];
        }

        public void ApplyTranslation()
        {
            LanguageInfo info = Program.LanguageManager.Translation;

            OptionsBtn.Text = info.chatBtnoptions;
            ProfileBtn.Text = info.MainProfileBtn;
            DeckBtn.Text = info.MainDeckBtn;
            ReplaysBtn.Text = info.MainReplaysBtn;
            OfflineBtn.Text = info.MainOfflineBtn;
            forumBtn.Text = info.MainForumBtn;
            siteBtn.Text = info.MainSiteBtn;
            MessageLabel.Text = info.MainServerMessage;
        }
        private void ServerMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ServerMessage), message);
                return;
            }
            MessageLabel.Text = message;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public void Login()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Login));
                return;
            }

            mainTabs.TabPages.Remove(mainTabs.SelectedTab);
            LanguageInfo info = Program.LanguageManager.Translation;

            var gamelistTab = new TabPage(info.MainGameTab);
            gamelistTab.Controls.Add(GameWindow);
            mainTabs.TabPages.Add(gamelistTab);

            var chatTab = new TabPage(info.MainChatTab);
            chatTab.Controls.Add(m_chatWindow);
            mainTabs.TabPages.Add(chatTab);

            var rankingTab = new TabPage(info.MainRankingTab);
            rankingTab.Controls.Add(m_rankingWindow);
            mainTabs.TabPages.Add(rankingTab);

            var filemanagerTab = new TabPage(info.MainFileManagerTab);
            filemanagerTab.Controls.Add(m_filemanagerWindow);
            mainTabs.TabPages.Add(filemanagerTab);

            var cuztomizerTab = new TabPage(info.MainCustomizeTab);
            cuztomizerTab.Controls.Add(m_customizerWindow);
            mainTabs.TabPages.Add(cuztomizerTab);

            var devpointTab = new TabPage(info.SupportTitle);
            devpointTab.Controls.Add(m_devpointWindow);
            mainTabs.TabPages.Add(devpointTab);
                
            ConnectionCheck.Enabled = true;
            ConnectionCheck.Tick += CheckConnection;
            
            UpdateUsername();

            ProfileBtn.Enabled = true;

            Program.ChatServer.SendPacket(DevServerPackets.DevPoints);
        }

        public void UpdateUsername()
        {
            Text = GetVersionString() + " - " + Program.UserInfo.username;
        }

        public void ReLoadLanguage()
        {
            GameWindow.ApplyTranslation();
            m_filemanagerWindow.ApplyTranslations();
            m_customizerWindow.ApplyTranslation();
            m_chatWindow.ApplyTranslations();
            m_rankingWindow.ApplyTranslation();
        }

        private void CheckConnection(object sender, EventArgs e)
        {
            if (!Program.ChatServer.IsUserBanned)
            {
                if (!Program.ChatServer.Connected())
                {
                    var connectionCheck = (Timer)sender;
                    Hide();
                    connectionCheck.Enabled = false;
                    if (MessageBox.Show(!string.IsNullOrEmpty(Program.ChatServer.ServerKickBanMessage) ? Program.ChatServer.ServerKickBanMessage : "Disconnected from server.\n\r Do you want to restart DevPro ?", "Server", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var process = new Process();
                        var startInfos = new ProcessStartInfo(Application.ExecutablePath, "-r");
                        process.StartInfo = startInfos;
                        process.Start();
                        Application.Exit();
                    }
                    else
                    {
                        Application.Exit();
                    }

                }
            }
        }

        private void OfflineBtn_Click(object sender, EventArgs e)
        {
            LauncherHelper.RunGame("");
        }

        private void forumBtn_Click(object sender, EventArgs e)
        {
            Process.Start("http://forum.ygodevpro.com/");
        }
        private void siteBtn_Click(object sender, EventArgs e)
        {
            if (Program.LanguageManager.language.Equals("German"))
                Process.Start("http://de.ygodevpro.com/");
            else
                Process.Start("http://en.ygodevpro.com/");
        }

        private void DeckBtn_Click(object sender, EventArgs e)
        {
            LauncherHelper.GenerateConfig();
            LauncherHelper.RunGame("-d");
        }
        private void ReplaysBtn_Click(object sender, EventArgs e)
        {
            LauncherHelper.GenerateConfig();
            LauncherHelper.RunGame("-r");
        }

        private void ProfileBtn_Click(object sender, EventArgs e)
        {
            var profile = new ProfileFrm(Program.UserInfo.username);
            profile.ShowDialog();
        }

        private void OptionsBtn_Click(object sender, EventArgs e)
        {
            var settings = new Settings();
            settings.ShowDialog();

        }

        private void TabChange(object sender, EventArgs e)
        {
           if (mainTabs.SelectedIndex == 1)
                m_chatWindow.LoadDefaultChannel();
        }
    }
}
