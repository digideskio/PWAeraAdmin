using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace PWAeraAdmin
{
    public class PWThread
    {
        Main obj;
        public string[] DBurl = {
                                    "http://cp.haztech.com.my/pw/cmd.php",
                                    "http://localhost/aerapanel/pwi/modules/jsp/php/cmd.php"
                                };
        public string[,] htmlvals = {
                                    {
                                        "<th class=\"itemHeader\" colspan=\"2\"><span class='item_color2'>", "</span>"
                                    }, //Name
                                    {
                                        "<p><strong>Amount</strong>:", "</p>"
                                    }, //Amount
                                    {
                                        "<p>Interval Between Hits <strong>", "</strong> seconds</p>"
                                    }, //Interval
                                    {
                                        "<p>Strength +<strong>", "</strong></p>"
                                    }, //Strength
                                    {
                                        "<p>Dexterity +<strong>", "</strong></p>"
                                    }, //Dexterity
                                    {
                                        "<p>Magic +<strong>", "</strong></p>"
                                    }, //Magic
                                    {
                                        "<p>Vitality +<strong>", "</strong></p>"
                                    }, //Vitality
                                    {
                                        "<p>1: +", "</p>"
                                    }, //
                                    {
                                        "<p>2: +", "</p>"
                                    }, //
                                    {
                                        "<p>3: +", "</p>"
                                    }, //
                                    {
                                        "<p>4: +", "</p>"
                                    }, //
                                    {
                                        "<p>5: +", "</p>"
                                    }, //
                                    {
                                        "<p>6: +", "</p>"
                                    }, //
                                    {
                                        "<p>7: +", "</p>"
                                    }, //
                                    {
                                        "<p>8: +", "</p>"
                                    }, //
                                    {
                                        "<p>9: +", "</p>"
                                    }, //
                                    {
                                        "<p>10: +", "</p>"
                                    }, //
                                    {
                                        "<p>11: +", "</p>"
                                    }, //
                                    {
                                        "<p>12: +", "</p>"
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    }, //
                                    {
                                        "", ""
                                    } //
                                };
        public PWThread()
        {
        }
        public PWThread(Main obj)
        {
            this.obj = obj;
        }
        public void RefreshDB()
        {
            //Clear all lists
            Program.f.send(this.obj.lstRoles);
            Program.f.send(this.obj.lstRolesOnline);
            Program.f.send(this.obj.lstItems);
            Program.f.send(this.obj.lstSearchItems);
            //
            //Retrieve RolesDB
            Program.f.send(this.obj.mBtmStatus, "Retrieving Roles...");
            Program.Form.datadb = new Aera.FileDB(new Uri(@""+DBurl[0]+"?op=roles"));
            Program.Form.DBRoles = Program.Form.datadb.ToDataArray(this.obj.delimiter, this.obj.dID, this.obj.dName);
            foreach (Aera.Data val in this.obj.DBRoles)
            {
                Program.f.send(this.obj.lstRoles, val);
                if (val.GetInt(2) >= 1)
                    Program.f.send(this.obj.lstRolesOnline, val);
            }
            this.obj.datadb.Dispose();
            //
            //Retrieve RolesDB
            Program.f.send(this.obj.mBtmStatus, "Retrieving items...");
            this.obj.datadb = new Aera.FileDB(new Uri(@"" + DBurl[0] + "?op=listitem"));
            this.obj.DBItems = this.obj.datadb.ToDataArray(this.obj.delimiter, this.obj.dID, this.obj.dName);
            foreach (Aera.Data val in this.obj.DBItems)
            {
                Program.f.send(this.obj.lstItems, val);
            }
            this.obj.datadb.Dispose();
            Program.f.send(this.obj.mBtmStatus, "Retrieving completed...");
        }
        public void cliLogin()
        {
            Program.f.send(this.obj.mBtmStatus, "Logging-in...");
            this.obj.login = new Aera.FileDB(new Uri("http://pub.haztech.com.my/?adminlogin=1&uname=" + this.obj.uname + "&paswd=" + this.obj.paswd));
            Program.f.send(this.obj.mBtmStatus, "Successfully logged-in...");
            if (this.obj.login.Read(0) == "SUCCESS")
            {
                this.obj.lPanel = true;
                this.obj.aPanel = true;
                Program.f.send(this.obj.tabPage2, true);
                Program.f.send(this.obj.tabControl2, true);
                Program.f.send(this.obj.btnLogin, "&Log-Out");

                RefreshDB();
                MessageBox.Show("You have successfully logged-in");
            }
            else if (this.obj.login.Read(0) == "PW:WRONG")
            {
                Program.f.send(this.obj.btnLogin, "&Re-Login");
                MessageBox.Show("Password you have entered is invalid");
            }
            else if (this.obj.login.Read(0) == "PW:EMPTY")
            {
                Program.f.send(this.obj.btnLogin, "&Re-Login");
                MessageBox.Show("You have entered emptied password. Please enter a valid password.");
            }
            else if (this.obj.login.Read(0) == "UN:UNKNOWN")
            {
                Program.f.send(this.obj.btnLogin, "&Re-Login");
                MessageBox.Show("Unknown username for " + this.obj.uname);
            }
            else if (this.obj.login.Read(0) == "UN:EMPTY")
            {
                Program.f.send(this.obj.btnLogin, "&Re-Login");
                MessageBox.Show("You have entered emptied username. Please enter a valid username.");
            }
            else
            {
                Program.f.send(this.obj.btnLogin, "&Re-Login");
                MessageBox.Show("N/A for " + this.obj.uname);
            }
            this.obj.login.Dispose();
            endProgress();
        }
        public void cliSendItem()
        {
            this.obj.datadb = new Aera.FileDB(new Uri(@"" + DBurl[0] + "?op=senditem&roleid=" + this.obj.mRoleID.Text + "&iid=" + this.obj.mSendItemID.Text + "&icount=" + this.obj.mSendItemTotal.Text));
            if (this.obj.datadb.Read(0) == "SENDITEM_SUCCESS")
                MessageBox.Show("Item has been sent");
            else
                MessageBox.Show("Fail to send item");
            this.obj.datadb.Dispose();
            endProgress();
        }
        public void cliSearchItem()
        {
            Program.f.send(this.obj.lstSearchItems);
            //
            //Retrieve SearchDB
            Program.f.send(this.obj.mBtmStatus, "Retrieving search items...");
            this.obj.datadb = new Aera.FileDB(new Uri(@"" + DBurl[0] + "?op=searchitem&keyword=" + this.obj.mSearchItemKeyword.Text));
            try
            {
                this.obj.DBSearchItems = this.obj.datadb.ToDataArray(this.obj.delimiter, this.obj.dID, this.obj.dName);
                foreach (Aera.Data val in this.obj.DBSearchItems)
                {
                    Program.f.send(this.obj.lstSearchItems, val);
                }
            }
            catch { }
            Program.f.send(this.obj.mBtmStatus, "Retrieving completed...");

            if (this.obj.datadb.Read(0) == "SEARCHITEM_FAIL")
                MessageBox.Show("No result found");

            this.obj.datadb.Dispose();
            endProgress();
        }

        public void cliSearchMob()
        {
            Program.f.send(this.obj.lstMobList);
            //
            //Retrieve SearchDB
            Program.f.send(this.obj.mBtmStatus, "Retrieving search mobs...");
            this.obj.datadb = new Aera.FileDB(new Uri(@"" + DBurl[1] + "?op=searchmob&keyword=" + this.obj.lstMobSearch.Text));
            try
            {
                this.obj.DBSearchItems = this.obj.datadb.ToDataArray(this.obj.delimiter, this.obj.dID, this.obj.dName);
                foreach (Aera.Data val in this.obj.DBSearchItems)
                {
                    Program.f.send(this.obj.lstMobList, val);
                }
                Program.f.send(this.obj.mBtmStatus, "Retrieving completed...");
            }
            catch { }
            if (this.obj.datadb.Read(0) == "SEARCHITEM_FAIL")
                MessageBox.Show("No result found");

            this.obj.datadb.Dispose();
            endProgress();
        }

        public void cliAddItem()
        {

            List<object> attr = new List<object>();
            List<object> vals = new List<object>();
            attr.Add((object)"iid");
            vals.Add((object)this.obj.mItemAddID.Text);
            attr.Add((object)"iname");
            vals.Add((object)this.obj.mItemAddName.Text);
            attr.Add((object)"iinfo");
            vals.Add((object)this.obj.mItemAddDesc.Text);


            this.obj.datadb = new Aera.FileDB(new Uri(@"" + DBurl[0] + "?op=additem&" + new Aera.AeraDB("itemlist", attr, vals).ToHttp()));

            Program.f.send(this.obj.mItemAddOUT, new Aera.AeraDB("itemlist", attr, vals).ToHttp());

            try
            {
                this.obj.DBSearchItems = this.obj.datadb.ToDataArray(this.obj.delimiter, this.obj.dID, this.obj.dName);
                foreach (Aera.Data val in this.obj.DBSearchItems)
                {
                    Program.f.send(this.obj.lstItems, val);
                }
                MessageBox.Show(this.obj.mItemAddName.Text + " successfully added");
            }
            catch { }
            if (this.obj.datadb.Read(0) == "ADDITEM_FAIL")
                MessageBox.Show("Fail to add " + this.obj.mItemAddName.Text);

            this.obj.datadb.Dispose();
            endProgress();
        }

        public void cliAddMob()
        {
            List<object> attr = new List<object>();
            List<object> vals = new List<object>();
            attr.Add((object)"mid");
            vals.Add((object)this.obj.mMobDBGetLastID.Text);
            attr.Add((object)"mname");
            vals.Add((object)this.obj.mMobDBGetName.Text);
            attr.Add((object)"mlevel");
            vals.Add((object)this.obj.mMobDBGetLevel.Text);
            attr.Add((object)"mdesc");
            vals.Add((object)this.obj.mMobDBGetDesc.Text);
            attr.Add((object)"mcoor");
            vals.Add((object)this.obj.mMobDBGetCoor.Text);
            attr.Add((object)"sp");
            vals.Add((object)this.obj.mMobDBGetSP.Text);
            attr.Add((object)"money");
            vals.Add((object)this.obj.mMobDBGetMoney.Text);
            attr.Add((object)"life");
            vals.Add((object)this.obj.mMobDBGetLife.Text);
            attr.Add((object)"accuracy");
            vals.Add((object)this.obj.mMobDBGetAcc.Text);
            attr.Add((object)"evasion");
            vals.Add((object)this.obj.mMobDBGetEva.Text);
            attr.Add((object)"patkmin");
            vals.Add((object)this.obj.mMobDBGetPAtkMin.Text);
            if (this.obj.mMobDBGetPAtkMax.Text.Length > 0)
            {
                attr.Add((object)"patkmax");
                vals.Add((object)this.obj.mMobDBGetPAtkMax.Text);
            }
            attr.Add((object)"matkmin");
            vals.Add((object)this.obj.mMobDBGetMAtkMin.Text);
            if (this.obj.mMobDBGetMAtkMax.Text.Length > 0)
            {
                attr.Add((object)"matkmax");
                vals.Add((object)this.obj.mMobDBGetMAtkMax.Text);
            }
            attr.Add((object)"pdef");
            vals.Add((object)this.obj.mMobDBGetPDef.Text);

            this.obj.datadb = new Aera.FileDB(new Uri(@"" + DBurl[1] + "?op=addmob&" + new Aera.AeraDB("moblist", attr, vals).ToHttp()));
            try
            {
                this.obj.DBSearchItems = this.obj.datadb.ToDataArray(this.obj.delimiter, this.obj.dID, this.obj.dName);
                foreach (Aera.Data val in this.obj.DBSearchItems)
                {
                    Program.f.send(this.obj.lstMobList, val);
                }
                MessageBox.Show(this.obj.mMobDBGetName.Text + " successfully added");
            }
            catch { }
            if (this.obj.datadb.Read(0) == "ADDITEM_FAIL")
                MessageBox.Show("Fail to add " + this.obj.mMobDBGetName.Text);

            this.obj.datadb.Dispose();
            endProgress();
        }

        public void cliGetMobDB()
        {
            int id = Convert.ToInt32(this.obj.mMobDBGetID.Text);

            try
            {
                this.obj.datadb = new Aera.FileDB(new Uri(@"" + DBurl[1] + "?op=listnonmobs"));
                this.obj.DBMobs = Program.Form.datadb.ToDataArray(this.obj.delimiter, this.obj.dID, this.obj.dName);
                foreach (Aera.Data mob in this.obj.DBMobs)
                {
                    if (mob.ToIntID() < id)
                        continue;
                    id = mob.ToIntID();
                    if (mob.ToString() != "NA")
                        continue;
                    if ((id <= 50000) && (this.obj.cliStillProgressSub))
                    {
                        //Thread current = Thread.CurrentThread;
                        //Thread.Sleep(5000); //5 secs paused
                        Program.f.send(this.obj.mMobDBGetID, id.ToString());
                        Aera.INI.WriteIniValue("moblist", "mobid", id.ToString(), "aera.ini");
                        this.obj.datadb = new Aera.FileDB(new Uri(this.obj.mMobDBURL.Text + this.obj.mMobDBGetID.Text), true);
                        int num = 0;
                        string level = "";
                        string sp = "";
                        string money = "";
                        string life = "";
                        string accuracy = "";
                        string evasion = "";

                        string[] patkdb;
                        string patk = "";
                        string patk2 = "";
                        bool patkminmax = false;
                        string[] matkdb;
                        string matk = "";
                        string matk2 = "";
                        bool matkminmax = false;
                        string pdef = "";
                        string desc = "";
                        string name = "";
                        string coordinate = "";
                        bool descType = false;

                        foreach (string val in this.obj.datadb.DB)
                        {
                            //Program.f.send(this.obj.lstMobDBhtml, num+" : " + val);
                            num++;
                            if (val.Contains("<th colspan=\"2\">"))
                            {
                                name = val.Replace("<th colspan=\"2\">", "").Replace("</th>", "").Trim();

                            }
                            else if (val.Contains("<h3>Level</h3><p>"))
                            {
                                level = val.Replace("<h3>Level</h3><p>", "").Replace("</p>", "").Trim();

                            }
                            else if (val.Contains("<h3>SP</h3><p>"))
                            {
                                sp = val.Replace("<h3>SP</h3><p>", "").Replace("</p>", "").Trim();
                            }
                            else if (val.Contains("<h3>Money</h3><p>"))
                            {
                                money = val.Replace("<h3>Money</h3><p>", "").Replace("</p>", "").Trim();
                            }
                            else if (val.Contains("<h3>Life</h3><p>"))
                            {
                                life = val.Replace("<h3>Life</h3><p>", "").Replace("</p>", "").Trim();
                            }
                            else if (val.Contains("<h3>Accuracy</h3><p>"))
                            {
                                accuracy = val.Replace("<h3>Accuracy</h3><p>", "").Replace("</p>", "").Trim();
                            }
                            else if (val.Contains("<h3>Evasion</h3><p>"))
                            {
                                evasion = val.Replace("<h3>Evasion</h3><p>", "").Replace("</p>", "").Trim();
                            }
                            else if (val.Contains("<h3>Physical Attack</h3><p>"))
                            {
                                if (val.Contains("-"))
                                {
                                    patk = val.Replace("<h3>Physical Attack</h3><p>", "").Replace("</p>", "").Trim();
                                    patkdb = patk.Split('-');
                                    patk = patkdb[0].Trim();
                                    patk2 = patkdb[1].Trim();
                                    patkminmax = true;
                                }
                                else
                                    patk = val.Replace("<h3>Physical Attack</h3><p>", "").Replace("</p>", "").Trim();
                            }
                            else if (val.Contains("<h3>Magic Attack</h3><p>"))
                            {
                                if (val.Contains("-"))
                                {
                                    matk = val.Replace("<h3>Magic Attack</h3><p>", "").Replace("</p>", "").Trim();
                                    matkdb = matk.Split('-');
                                    matk = matkdb[0].Trim();
                                    matk2 = matkdb[1].Trim();
                                    matkminmax = true;
                                }
                                else
                                    matk = val.Replace("<h3>Magic Attack</h3><p>", "").Replace("</p>", "").Trim();
                            }
                            else if (val.Contains("<h3>Physical Defense</h3><p>"))
                            {
                                pdef = val.Replace("<h3>Physical Defense</h3><p>", "").Replace("</p>", "").Trim();
                            }
                            else if (val.Contains("<td><h3>Type</h3>"))
                            {
                                descType = true;
                            }
                            else if (descType)
                            {
                                if (val.Contains("</table>"))
                                {
                                    descType = false;
                                    continue;
                                }
                                desc = desc + val.Replace("</td>", "").Replace("<td>", "").Replace("</tr>", "").Trim();
                            }
                            else if (val.Contains("var MapCoordinats"))
                            {
                                coordinate = val.Replace("var MapCoordinats = ", "").Trim();
                            }
                        }

                        id++;
                        if ((name == "") || (name == null))
                            continue;

                        Program.f.send(this.obj.mMobDBGetLastID, id.ToString());
                        Program.f.send(this.obj.mMobDBGetCoor, coordinate);
                        Program.f.send(this.obj.mMobDBGetDesc, desc);
                        Program.f.send(this.obj.mMobDBGetLevel, level);
                        Program.f.send(this.obj.mMobDBGetName, name);
                        Program.f.send(this.obj.lstMobDBGetLastID, new Aera.Data(id, name));

                        List<object> attr = new List<object>();
                        List<object> vals = new List<object>();
                        attr.Add((object)"mid");
                        vals.Add((object)this.obj.mMobDBGetID.Text);
                        attr.Add((object)"mname");
                        vals.Add((object)name);
                        attr.Add((object)"mlevel");
                        vals.Add((object)level);
                        attr.Add((object)"mdesc");
                        vals.Add((object)desc);
                        attr.Add((object)"mcoor");
                        vals.Add((object)coordinate);
                        attr.Add((object)"sp");
                        vals.Add((object)sp);
                        attr.Add((object)"money");
                        vals.Add((object)money);
                        attr.Add((object)"life");
                        vals.Add((object)life);
                        attr.Add((object)"accuracy");
                        vals.Add((object)accuracy);
                        attr.Add((object)"evasion");
                        vals.Add((object)evasion);
                        attr.Add((object)"patkmin");
                        vals.Add((object)patk);
                        if (patkminmax)
                        {
                            attr.Add((object)"patkmax");
                            vals.Add((object)patk2);
                        }
                        attr.Add((object)"matkmin");
                        vals.Add((object)matk);
                        if (matkminmax)
                        {
                            attr.Add((object)"matkmax");
                            vals.Add((object)matk2);
                        }
                        attr.Add((object)"pdef");
                        vals.Add((object)pdef);
                        Program.f.send(this.obj.mMobDBGetSQL, new Aera.AeraDB("moblist", attr, vals).ToString());
                        //break;
                        this.obj.db.Insert(new Aera.AeraDB("moblist", attr, vals).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Program.f.send(this.obj.lstCatches, e.ToString());
            }
            /*while ((id <= 50000) && (this.obj.cliStillProgressSub))
            {

            }*/
            //int id = Convert.ToInt32(this.obj.mMobDBGetID.Text);
            endProgress();
        }
        public void cliGetQuestDB()
        {
            int id = Convert.ToInt32(this.obj.mQuestDBID.Text);
            while ((id <= 50000) && (this.obj.cliStillProgressSub))
            {
                //Thread current = Thread.CurrentThread;
                //Thread.Sleep(5000); //5 secs paused
                Program.f.send(this.obj.mQuestDBID, id.ToString());
                Aera.INI.WriteIniValue("list", "qid", id.ToString(), "aera.ini");
                this.obj.datadb = new Aera.FileDB(new Uri(this.obj.mQuestDBUrl.Text + this.obj.mQuestDBID.Text), true);
                int num = 0;
                string[] leveldb;
                string level = "";
                string level2 = "";
                bool levelminmax = false;
                string sp = "";
                string gold = "";
                string exp = "";
                string repu = "";
                string desc = "";
                string name = "";
                bool descType = false;

                foreach (string val in this.obj.datadb.DB)
                {
                    //Program.f.send(this.obj.lstMobDBhtml, num+" : " + val);
                    num++;
                    if (val.Contains("<th colspan=\"2\"><span style='color:#ffffff'>"))
                    {
                        name = val.Replace("<th colspan=\"2\"><span style='color:#ffffff'>", "").Replace("</span></th>", "").Trim();

                    }
                    else if (val.Contains("<p>Level:"))
                    {
                        if (val.Contains("-"))
                        {
                            level = val.Replace("<p>Level:", "").Replace("</p>", "").Replace("+", "").Trim();
                            leveldb = level.Split('-');
                            level = leveldb[0].Trim();
                            level2 = leveldb[1].Trim();
                            levelminmax = true;
                        }
                        else
                            level = val.Replace("<p>Level:", "").Replace("</p>", "").Replace("+", "").Trim();

                    }
                    else if (val.Contains("<p><b>SP</b>:"))
                    {
                        sp = val.Replace("<p><b>SP</b>:", "").Replace("</p>", "").Trim();
                    }
                    else if (val.Contains("<p><b>Reputation</b>:"))
                    {
                        repu = val.Replace("<p><b>Reputation</b>:", "").Replace("</p>", "").Trim();
                    }
                    else if (val.Contains("<p><b>Exp</b>:"))
                    {
                        exp = val.Replace("<p><b>Exp</b>:", "").Replace("</p>", "").Trim();
                    }
                    else if (val.Contains("<p><b>Gold</b>:"))
                    {
                        gold = val.Replace("<p><b>Gold</b>:", "").Replace("</p>", "").Trim();
                    }
                    else if (val.Contains("<td width=\"500\">"))
                    {
                        descType = true;
                    }
                    else if (descType)
                    {
                        if (val.Contains("</table>"))
                        {
                            descType = false;
                            continue;
                        }
                        desc = desc + val.Replace("</td>", "").Replace("<td>", "").Replace("</tr>", "").Replace("<tr>", "").Trim();
                    }
                }

                id++;
                if ((name == "") || (name == null))
                    continue;

                Program.f.send(this.obj.mQuestDBLastID, id.ToString());
                Program.f.send(this.obj.mQuestDBDesc, desc);
                Program.f.send(this.obj.mQuestDBLevel, level);
                Program.f.send(this.obj.mQuestDBName, name);

                List<object> attr = new List<object>();
                List<object> vals = new List<object>();
                attr.Add((object)"qid");
                vals.Add((object)this.obj.mQuestDBID.Text);
                attr.Add((object)"qname");
                vals.Add((object)name);
                attr.Add((object)"qlevelmin");
                vals.Add((object)level);
                if (levelminmax)
                {
                    attr.Add((object)"qlevelmax");
                    vals.Add((object)level2);
                }
                attr.Add((object)"qdesc");
                vals.Add((object)desc);
                attr.Add((object)"repu");
                vals.Add((object)repu);
                attr.Add((object)"sp");
                vals.Add((object)sp);
                attr.Add((object)"gold");
                vals.Add((object)gold);
                attr.Add((object)"exp");
                vals.Add((object)exp);

                Program.f.send(this.obj.mQuestDBSQL, new Aera.AeraDB("questlist", attr, vals).ToString());
                //break;
                this.obj.db.Insert(new Aera.AeraDB("questlist", attr, vals).ToString());
            }
            //int id = Convert.ToInt32(this.obj.mMobDBGetID.Text);
            endProgress();
        }
        public void cliGetItemDB()
        {
            int id = Convert.ToInt32(this.obj.mQuestDBID.Text);
            while ((id <= 50000) && (this.obj.cliStillProgressSub))
            {
                //Thread current = Thread.CurrentThread;
                //Thread.Sleep(5000); //5 secs paused
                Program.f.send(this.obj.mQuestDBID, id.ToString());
                Aera.INI.WriteIniValue("list", "qid", id.ToString(), "aera.ini");
                this.obj.datadb = new Aera.FileDB(new Uri(this.obj.mQuestDBUrl.Text + this.obj.mQuestDBID.Text), true);
                int num = 0;
                string[] leveldb;
                string level = "";
                string level2 = "";
                bool levelminmax = false;
                string sp = "";
                string gold = "";
                string exp = "";
                string repu = "";
                string desc = "";
                string name = "";
                bool descType = false;


                foreach (string val in this.obj.datadb.DB)
                {
                    //Program.f.send(this.obj.lstMobDBhtml, num+" : " + val);
                    num++;
                    if (val.Contains(""))
                    {
                        name = val.Replace("<th colspan=\"2\"><span style='color:#ffffff'>", "").Replace("</span></th>", "").Trim();

                    }
                    else if (val.Contains("<p>Level:"))
                    {
                        if (val.Contains("-"))
                        {
                            level = val.Replace("<p>Level:", "").Replace("</p>", "").Replace("+", "").Trim();
                            leveldb = level.Split('-');
                            level = leveldb[0].Trim();
                            level2 = leveldb[1].Trim();
                            levelminmax = true;
                        }
                        else
                            level = val.Replace("<p>Level:", "").Replace("</p>", "").Replace("+", "").Trim();

                    }
                    else if (val.Contains("<p><b>SP</b>:"))
                    {
                        sp = val.Replace("<p><b>SP</b>:", "").Replace("</p>", "").Trim();
                    }
                    else if (val.Contains("<p><b>Reputation</b>:"))
                    {
                        repu = val.Replace("<p><b>Reputation</b>:", "").Replace("</p>", "").Trim();
                    }
                    else if (val.Contains("<p><b>Exp</b>:"))
                    {
                        exp = val.Replace("<p><b>Exp</b>:", "").Replace("</p>", "").Trim();
                    }
                    else if (val.Contains("<p><b>Gold</b>:"))
                    {
                        gold = val.Replace("<p><b>Gold</b>:", "").Replace("</p>", "").Trim();
                    }
                    else if (val.Contains("<td width=\"500\">"))
                    {
                        descType = true;
                    }
                    else if (descType)
                    {
                        if (val.Contains("</table>"))
                        {
                            descType = false;
                            continue;
                        }
                        desc = desc + val.Replace("</td>", "").Replace("<td>", "").Replace("</tr>", "").Replace("<tr>", "").Trim();
                    }
                }

                id++;
                if ((name == "") || (name == null))
                    continue;

                Program.f.send(this.obj.mQuestDBLastID, id.ToString());
                Program.f.send(this.obj.mQuestDBDesc, desc);
                Program.f.send(this.obj.mQuestDBLevel, level);
                Program.f.send(this.obj.mQuestDBName, name);

                List<object> attr = new List<object>();
                List<object> vals = new List<object>();
                attr.Add((object)"qid");
                vals.Add((object)this.obj.mQuestDBID.Text);
                attr.Add((object)"qname");
                vals.Add((object)name);
                attr.Add((object)"qlevelmin");
                vals.Add((object)level);
                if (levelminmax)
                {
                    attr.Add((object)"qlevelmax");
                    vals.Add((object)level2);
                }
                attr.Add((object)"qdesc");
                vals.Add((object)desc);
                attr.Add((object)"repu");
                vals.Add((object)repu);
                attr.Add((object)"sp");
                vals.Add((object)sp);
                attr.Add((object)"gold");
                vals.Add((object)gold);
                attr.Add((object)"exp");
                vals.Add((object)exp);

                Program.f.send(this.obj.mQuestDBSQL, new Aera.AeraDB("questlist", attr, vals).ToString());
                //break;
                this.obj.db.Insert(new Aera.AeraDB("questlist", attr, vals).ToString());
            }
            //int id = Convert.ToInt32(this.obj.mMobDBGetID.Text);
            endProgress();
        }

        public void cliOnlineProgress()
        {
            while (this.obj.client2start)
            {
                this.obj.datadb = new Aera.FileDB(new Uri(@"" + DBurl[0] + "?op=oroles"));
                this.obj.DBGraphs = this.obj.datadb.ToDataArray(this.obj.delimiter, this.obj.dID, this.obj.dName);
                Program.f.send(this.obj.lstGraph, new OnlineGraph(this.obj.DBGraphs));

                this.obj.datadb.Dispose();
                Thread clicurrent = Thread.CurrentThread;
                Thread.Sleep(60000); //60 secs paused
            }
        }
        public void cliProgress()
        {
            //Thread current = Thread.CurrentThread;
            //Thread.Sleep(5000); //5 secs paused
            endProgress();
        }
        public void endProgress()
        {
            this.obj.cliStillProgress = false;
        }

        /// <summary>
        /// 
        /// startPROGRESS
        /// </summary>
        /// <param name="type"></param>
        public void startProgress(int type)
        {
            try
            {
                if (type < 500)
                    this.obj.client1.Abort();
                else if (type < 1000)
                    this.obj.client2.Abort();
                else
                    this.obj.client3.Abort();
            }
            catch
            {
            }
            try
            {
                if ((this.obj.cliStillProgress == false) && (type < 500))
                {
                    this.obj.cliStillProgress = true;
                    this.obj.cliStillProgressID = type;
                    if (type == 1)
                        this.obj.client1 = new Thread(new ThreadStart(cliLogin));
                    else if (type == 2)
                        this.obj.client1 = new Thread(new ThreadStart(cliSendItem));
                    else if (type == 3)
                        this.obj.client1 = new Thread(new ThreadStart(cliSearchItem));
                    else if (type == 4)
                        this.obj.client1 = new Thread(new ThreadStart(cliAddItem));
                    else if (type == 5)
                        this.obj.client1 = new Thread(new ThreadStart(cliAddMob));
                    else if (type == 6)
                        this.obj.client1 = new Thread(new ThreadStart(cliSearchMob));
                    else
                        this.obj.client1 = new Thread(new ThreadStart(cliProgress));
                    this.obj.client1.Start();
                }
                else if (type < 1000)
                {
                    if (type == 500)
                        this.obj.client2 = new Thread(new ThreadStart(cliOnlineProgress));
                    this.obj.client2.Start();
                }
                else if (type >= 1000)
                {
                    if (type == 1001)
                        this.obj.client3 = new Thread(new ThreadStart(cliGetMobDB));
                    else if (type == 1002)
                        this.obj.client3 = new Thread(new ThreadStart(cliGetQuestDB));
                    this.obj.client3.Start();
                }
                else
                    MessageBox.Show("Progress ID " + this.obj.cliStillProgressID + " still under progress");
            }
            catch { }
        }
    }
}
