using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace PWAeraAdmin
{
    public partial class Main : Form, IDisposable
    {
        public string uname = "";
        public string paswd = "";
        public Aera.FileDB login;
        public Aera.FileDB datadb;
        public Aera.FileDB tempdb;
        public bool lPanel = false;
        public bool aPanel = false;
        public Thread client1;
        public Thread client2;
        public Thread client3;
        public bool cliStillProgress = false;
        public bool cliStillProgressSub = false;
        public int cliStillProgressID = 0;
        public bool client2start = true;

        public Aera.DBConfig db;

        //Threadings
        private PWThread pw;

        //Data DB
        public char delimiter = ':';
        public int dID = 0;
        public int dName = 1;
        public List<Aera.Data> DBtemp = new List<Aera.Data>();
        public List<Aera.Data> DBRoles = new List<Aera.Data>();
        public List<Aera.Data> DBItems = new List<Aera.Data>();
        public List<Aera.Data> DBMobs = new List<Aera.Data>();
        public List<Aera.Data> DBSearchItems = new List<Aera.Data>();
        public List<Aera.Data> DBGuilds = new List<Aera.Data>();
        public List<Aera.Data> DBBattlePowers = new List<Aera.Data>();
        public List<Aera.Data> DBGraphs = new List<Aera.Data>();

        //PWI DB

        public string[,,] htmlvals = 
        { 
            { //ITEM
                {"<th class=\"itemHeader\" colspan=\"2\">","</span>"},          // 00 : NAME
                {"Type: ","<br />"},                                            // 01 : 
                {"Subtype: ","<br />  "},                                       // 02 : 
                {"Used by character(s):","<br />  "},                           // 03 : 
                {"LV. ","<br />"},                                              // 04 : 
                {"Level Required: ","<br />"},                                  // 05 : 
                {"Price: ","<br />"},                                           // 06 : 
                {"Stacked: ","<br />"},                                         // 07 : 
                {"Craft Rate with 2 socket(s): ","%<br />"},                    // 08 : 
                {"Craft Rate with 3 socket(s): ","%<br />"},                    // 09 : 
                {"Craft Rate with 4 socket(s): ","%<br />"},                    // 00 : 
                {"Drop rate with 2 socket(s): ","%<br />"},                     // 11 : 
                {"Drop rate with 3 socket(s): ","%<br />"},                     // 12 : 
                {"Drop rate with 4 socket(s): ","%<br />"},                     // 13 : 
                {"<p><strong>Amount</strong>:","</p>"},                         // 14 : 
                {"<p>Interval Between Hits <strong>","</strong> seconds</p>"},  // 15 : 
                {"<p>Strength +<strong>","</strong></p>"},                      // 16 : 
                {"<p>Dexterity +<strong>","</strong></p>"},                     // 17 : 
                {"<p>Magic +<strong>","</strong></p>"},                         // 18 : 
                {"<p>Vitality +<strong>","</strong></p>"},                      // 19 : 
                {"<p>1: +","</p>"},                                             // 20 : 
                {"<p>2: +","</p>"},                                             // 21 : 
                {"<p>3: +","</p>"},                                             // 22 : 
                {"<p>4: +","</p>"},                                             // 23 : 
                {"<p>5: +","</p>"},                                             // 24 : 
                {"<p>6: +","</p>"},                                             // 25 : 
                {"<p>7: +","</p>"},                                             // 26 : 
                {"<p>8: +","</p>"},                                             // 27 : 
                {"<p>9: +","</p>"},                                             // 28 : 
                {"<p>10: +","</p>"},                                            // 29 : 
                {"<p>11: +","</p>"},                                            // 30 : 
                {"<p>12: +","</p>"}                                             // 31 : 
            },
            { //MOBS
                {"<th colspan=\"2\">","</th>"},                                 //00 : NAME
                {"<p><a href=\"mtype/","</a></p>"},                             //01 : TYPE
                {"<h3>Element</h3><p>","</span></p>"},                          //02 : ELEMENT
                {"<h3>Level</h3><p>","</p>"},                                   //03 : LEVEL
                {"<h3>Exp</h3><p>","</p>"},                                     //04 : EXP
                {"<h3>SP</h3><p>","</p>"},                                      //05 : SP
                {"<h3>Money</h3><p>","</p>"},                                   //06 : MONEY
                {"<p>Agro Time: ","</p>"},                                      //07 : AGROTIME
                {"<p>Agro Range: ","</p>"},                                     //08 : AGRORANGE
                {"<p>0: ","</p>"},                                              //09 : DROPRATE 1
                {"<p>1: ","</p>"},                                              //10 : DROPRATE 2
                {"<p>2: ","</p>"},                                              //11 : DROPRATE 3
                {"<p>3: ","</p>"},                                              //12 : DROPRATE 4
                {"<h3>Life</h3><p>","</p>"},                                    //13 : LIFE
                {"<h3>Accuracy</h3><p>","</p>"},                                //14 : ACCURACY
                {"<h3>Evasion</h3><p>","</p>"},                                 //15 : EVASION
                {"<h3>Physical Attack</h3><p>","</p>"},                         //16 : PHY-ATK X Y
                {"<h3>Magic Attack</h3><p>","</p>"},                            //17 : MAG-ATK X Y
                {"<h3>Physical Defense</h3><p>","</p>"},                        //18 : PHY-DEF
                {"<p><span class=\"el_metal\">Metal</span>: ","</p>"},          //19 : MAG-DEF METAL
                {"<p><span class=\"el_tree\">Wood</span>: ","</p>"},            //20 : 
                {"<p><span class=\"el_water\">Water</span>: ","</p>"},          //21 : 
                {"<p><span class=\"el_fire\">Fire</span>: ","</p>"},            //22 : 
                {"<p><span class=\"el_earth\">Earth</span>: ","</p>"},          //23 : 
                {"<p>Walk: ","</p>"},                                           //24 : 
                {"<p>Run: ","</p>"},                                            //25 : 
                {"<p>Fly: ","</p>"},                                            //26 : 
                {"<p>Swim: ","</p>"},                                           //27 : 
                {"var MapCoordinats = ",""},                                    //28 :
                {"",""},                                                        //29 : 
                {"",""},                                                        //30 : 
                {"",""}                                                         //31 : 
            },
            { //QUEST
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""},
                {"",""}
            }
        };

        //Disposing the existing data
        public void DisposeAll()
        {
            client2start = false;
            try
            {
                client1.Abort();
            }
            catch { }
            try
            {
                client2.Abort();
            }
            catch { }
        }

        public Main()
        {
            InitializeComponent();

            //Registerating
            pw = new PWThread(this);

            //Form Customization
            mBtmStatus.Text = "";
            tabPage2.Enabled = false;
            tabControl2.Visible = false;
            if (Aera.INI.GetIniValue("moblist", "mobid", "aera.ini") != null)
                mMobDBGetID.Text = Aera.INI.GetIniValue("moblist", "mobid", "aera.ini");
            if (Aera.INI.GetIniValue("list", "qid", "aera.ini") != null)
                mQuestDBID.Text = Aera.INI.GetIniValue("list", "qid", "aera.ini");

            //Background Threads
            pw.startProgress(500);
        }


        //KEYPRESSSION
        private void itemSearchKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char keyChar;
            keyChar = e.KeyChar;
            if (keyChar == 13)
            {
                pw.startProgress(3);
            }
        }
        //LOGIN
        private void loginKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char keyChar;
            keyChar = e.KeyChar;
            if (keyChar == 13)
            {
                Login();
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }
        private void Login()
        {
            if (btnLogin.Text == "&Login")
            {
                uname = m_Username.Text.Trim();
                paswd = m_Password.Text.Trim();
                m_Username.Enabled = false;
                m_Password.Enabled = false;
                m_Username.Clear();
                m_Password.Clear();
                pw.startProgress(1);
            }
            else
            {
                lPanel = false;
                aPanel = false;
                tabPage2.Enabled = false;
                tabControl2.Visible = false;
                m_Username.Enabled = true;
                m_Password.Enabled = true;
                m_Username.Clear();
                m_Password.Clear();
                if (btnLogin.Text == "&Login")
                    MessageBox.Show("You have successfully logged-out");
                btnLogin.Text = "&Login";
            }
        }

        public void getRoleToData(Aera.Data val)
        {
            mRoleID.Text = val.ToSID();
            mRoleName.Text = val.ToString();
            mRoleLvl.Text = val.Get(3);
            mRoleGID.Text = val.Get(4);
            mRoleGuild.Text = val.Get(5);
            mRoleBPLVL.Text = val.Get(6);
            mRoleBPEXP.Text = val.Get(7);
            mRoleQuest.Text = val.Get(8);
            mRoleUID.Text = val.Get(9);
            mRoleUName.Text = val.Get(10);
            mRoleEmail.Text = val.Get(11);
        }

        private void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getRoleToData((Aera.Data)lstRoles.SelectedItem);
            }
            catch { }
        }

        private void lstRolesOnline_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getRoleToData((Aera.Data)lstRolesOnline.SelectedItem);
            }
            catch { }
        }

        private void btnSendItem_Click(object sender, EventArgs e)
        {
            pw.startProgress(2);
        }

        private void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSendItemID.Text = ((Aera.Data)lstItems.SelectedItem).ToSID();
        }

        private void btnRefreshDB_Click(object sender, EventArgs e)
        {
            pw.RefreshDB();
        }

        private void btnSearchItemKeyword_Click(object sender, EventArgs e)
        {
            pw.startProgress(3);
        }

        private void lstSearchItems_SelectedIndexChanged(object sender, EventArgs e)
        {

            mSendItemID.Text = ((Aera.Data)lstSearchItems.SelectedItem).ToSID();
        }

        private void lstGraph_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Program.f.send(lstGraphOnline);
                foreach (Aera.Data val in ((OnlineGraph)lstGraph.SelectedItem).DBRoles)
                {
                    Program.f.send(lstGraphOnline, val);
                }
            }
            catch { }
        }

        private void lstGraphOnline_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getRoleToData((Aera.Data)lstGraphOnline.SelectedItem);
            }
            catch { }
        }

        private void btnMobDBGet_Click(object sender, EventArgs e)
        {
            if (btnMobDBGet.Text == "S&top")
            {
                db.Dispose();
                cliStillProgressSub = false;
                btnMobDBGet.Text = "&Get";
            }
            else
            {
                db = new Aera.DBConfig();
                cliStillProgressSub = true;
                btnMobDBGet.Text = "S&top";
                pw.startProgress(1001);
            }
        }

        private void btnQuestDB_Click(object sender, EventArgs e)
        {

            if (btnQuestDB.Text == "S&top")
            {
                db.Dispose();
                cliStillProgressSub = false;
                btnQuestDB.Text = "&Get";
            }
            else
            {
                db = new Aera.DBConfig();
                cliStillProgressSub = true;
                btnQuestDB.Text = "S&top";
                pw.startProgress(1002);
            }
        }

        private void mobSearchKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char keyChar;
            keyChar = e.KeyChar;
            if (keyChar == 13)
            {
                pw.startProgress(6);
            }
        }
        private void btnMobList_Click(object sender, EventArgs e)
        {
            pw.startProgress(6);
        }

        private void lstMobList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //mMobDBGet
            if (cliStillProgressSub != true)
            {
                mMobDBGetID.Text = ((Aera.Data)lstMobList.SelectedItem).ToSID();
                mMobDBGetName.Text = ((Aera.Data)lstMobList.SelectedItem).ToString();
            }
            Program.f.send(lstMobDBGetLastID, (Aera.Data)lstMobList.SelectedItem);
            mMobDBGetID2.Text = ((Aera.Data)lstMobList.SelectedItem).ToSID();
            mMobDBGetName2.Text = ((Aera.Data)lstMobList.SelectedItem).ToString();
            mMobDBGetLevel.Text = ((Aera.Data)lstMobList.SelectedItem).Get(2);
            mMobDBGetDesc.Text = ((Aera.Data)lstMobList.SelectedItem).Get(3);
            mMobDBGetCoor.Text = ((Aera.Data)lstMobList.SelectedItem).Get(4);
            mMobDBGetSP.Text = ((Aera.Data)lstMobList.SelectedItem).Get(5);
            mMobDBGetMoney.Text = ((Aera.Data)lstMobList.SelectedItem).Get(6);
            mMobDBGetLife.Text = ((Aera.Data)lstMobList.SelectedItem).Get(7);
            mMobDBGetAcc.Text = ((Aera.Data)lstMobList.SelectedItem).Get(8);
            mMobDBGetEva.Text = ((Aera.Data)lstMobList.SelectedItem).Get(9);
            mMobDBGetPAtkMin.Text = ((Aera.Data)lstMobList.SelectedItem).Get(10);
            mMobDBGetPAtkMax.Text = ((Aera.Data)lstMobList.SelectedItem).Get(11);
            mMobDBGetMAtkMin.Text = ((Aera.Data)lstMobList.SelectedItem).Get(12);
            mMobDBGetMAtkMax.Text = ((Aera.Data)lstMobList.SelectedItem).Get(13);
            mMobDBGetPDef.Text = ((Aera.Data)lstMobList.SelectedItem).Get(14);
        }

        private void lstMobDBGetLastID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                mMobDBGetID2.Text = ((Aera.Data)lstMobDBGetLastID.SelectedItem).ToSID();
                mMobDBGetName2.Text = ((Aera.Data)lstMobDBGetLastID.SelectedItem).ToString();
            }
            catch { }
        }

        private void lstCatches_SelectedIndexChanged(object sender, EventArgs e)
        {
            mCatches.Text = lstCatches.SelectedItem.ToString();
        }

        private void btnItemAdd_Click(object sender, EventArgs e)
        {
            pw.startProgress(4);
        }

        private void btnMobDBAdd_Click(object sender, EventArgs e)
        {

            pw.startProgress(5);
        }

        private void lstQuest_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch { }
        }

        private void btnQuestSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnQuestAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnItemDBGet_Click(object sender, EventArgs e)
        {

            datadb = new Aera.FileDB(new Uri(mItemDBUrl.Text + mItemDBID.Text));
            datadb.Save("test.txt");
        }
    }
}
