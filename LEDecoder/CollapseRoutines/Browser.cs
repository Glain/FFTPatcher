using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LEDecoder.CollapseRoutines
{
    public partial class Browser : Form
    {
        public MainForm Main;
        public HtmlElement element;

        public Browser(MainForm mainform)
        {
            InitializeComponent();
            Main = mainform;

           switch (Main.txt_InputFile.Text)
           {
               case "E:\\fft\\ASM\\debugging\\Disassembly\\SCUS Disassembly.txt":
                   Client.Navigate("http://ffhacktics.com/w/index.php?title=Special:UserLogin&returnto=SCUS+942.21&returntoquery=action%3Dedit");
                   break;
               case "E:\\fft\\ASM\\debugging\\Disassembly\\BATTLE Disassembly.txt":
                   Client.Navigate("http://ffhacktics.com/w/index.php?title=Special:UserLogin&returnto=BATTLE.BIN&returntoquery=action%3Dedit");
                   break;
               case "E:\\fft\\ASM\\debugging\\Disassembly\\WORLD Disassembly.txt":
                   Client.Navigate("http://ffhacktics.com/w/index.php?title=Special:UserLogin&returnto=WORLD.BIN&returntoquery=action%3Dedit");
                   break;
               case "E:\\fft\\ASM\\debugging\\Disassembly\\WLDCORE Disassembly.txt":
                   Client.Navigate("http://ffhacktics.com/w/index.php?title=Special:UserLogin&returnto=WLDCORE.BIN&returntoquery=action%3Dedit");
                   break;
               case "E:\\fft\\ASM\\debugging\\Disassembly\\REQUIRE Disassembly.lst":
                   Client.Navigate("http://ffhacktics.com/w/index.php?title=Special:UserLogin&returnto=REQUIRE.OUT&returntoquery=action%3Dedit");
                   break;
               case "E:\\fft\\ASM\\debugging\\Disassembly\\EQUIP Disassembly.txt":
                   Client.Navigate("http://ffhacktics.com/w/index.php?title=Special:UserLogin&returnto=EQUIP.OUT&returntoquery=action%3Dedit");
                   break;
           }

           Client.DocumentCompleted += Client_LogIn;
           
        }

        private void Client_LogIn(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            element = Client.Document.GetElementById("wpName1");
            element.SetAttribute("value", "Choto");

            element = Client.Document.GetElementById("wpPassword1");
            element.SetAttribute("value", "isshinryu1");

            element = Client.Document.GetElementById("wpLoginAttempt");
            element.InvokeMember("click");

            Client.DocumentCompleted -= Client_LogIn;
            Client.DocumentCompleted += Client_EditRoutines;
           
        }

        private void Client_EditRoutines(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Client.DocumentCompleted -= Client_EditRoutines;

            Thread.Sleep(1);

            HtmlElement textbox = Client.Document.GetElementById("wpTextbox1");

            string innertext = textbox.InnerText;
            string address = "";
            long longaddress = 0;
            int previousindex = 0;

            for (int k = 0; k < Main.Routines.Length-1;k++ )
            {
                if (innertext.Contains(Main.Routines[k].TitleLine))
                {
                    Main.Routines[k] = null;
                }
            }
            try
            {

            
                foreach(Routine routine in Main.Routines)
                {
                    if(routine != null)
                    {
                        //innertext.Length
                        long routinelongaddress = StringToAddress(routine.TitleLine.Remove(8));
                        int index = FindRoutineInsertAddress(routinelongaddress, innertext);
                        if(index != 0 && index != innertext.Length)
                        {
                            innertext = innertext.Insert(index, routine.TitleLine + "\r\n\r\n");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            textbox.InnerText = innertext;
        }

        public int FindRoutineInsertAddress(long routinelongaddress, string innertext)
        {
            try
            {
                long longaddress = 0;
                int index = 0;
                int startindex = 0;
                innertext += "END";
                int endindex = innertext.IndexOf("END");

                do
                {
                    if(innertext.IndexOf(":",startindex) < 0)
                    {
                        return endindex;
                    }
                    index = innertext.IndexOf(":",startindex) - 19;
                    if (index > endindex)
                        break;
                    longaddress = StringToAddress(innertext.Substring(index).Remove(8));
                    if(longaddress > routinelongaddress)
                    {
                        return index;
                    }
                    else
                    {
                        startindex = index + 20;
                    }
                } while (routinelongaddress >= longaddress);
                return innertext.Length;
            }
            catch(Exception ex)
            {
                innertext = innertext.Substring(0x12b0);
                MessageBox.Show(ex.ToString());
                return 0;
            }
           
        }

        public static long StringToAddress(string InString)
        {
            InString = InString.Replace(" ", "");
            InString = InString.Replace("\t", "");

            if (InString.Contains("0x"))
            {
                InString = InString.Replace("0x", "");
            }
            if (InString.Contains("/"))
            {
                InString = InString.Remove(InString.IndexOf("/"));
            }
            long intout = 0;
            //intout = HextoSignedLong(InString);
            byte[] byteOut = new byte[3];

            bool negative = false;
            if (InString.Contains("-"))
            {
                negative = true;
                InString = InString.Replace("-", "");
            }
            if (InString != "")
            {
                String strInString = InString;
                int l = strInString.Length;
                if (l >= 6)
                {
                    strInString = strInString.Substring(l - 6, 6);
                }
                int p = l;
                int c = 0;
                while ((p > 0) == true)
                {
                    if ((((l - p) % 2) == 0) & (p != l))
                    {
                        strInString = strInString.Insert(p, " ");
                        c++;
                    }
                    p--;
                }
                //while ((p < l) == true)
                //{
                //    if (((p % 2) == 0) & (p != 0))
                //    {
                //        strInString = strInString.Insert(p + c, " ");
                //        c++;
                //    }
                //    p++;
                //}

                string[] strInputSplit = strInString.Split(' ');

                if (strInputSplit.Length > 3)
                {
                    string[] newstringsplit = new string[3];
                    newstringsplit[0] = strInputSplit[0];
                    newstringsplit[1] = strInputSplit[1];
                    newstringsplit[2] = strInputSplit[2];
                    strInputSplit = newstringsplit;
                }
                //strInString.Split(' ');

                if (InString.Contains("-"))
                {
                    sbyte byteout1 = Convert.ToSByte(strInputSplit[0].ToString());
                }
                else
                {
                    byteOut[0] = Convert.ToByte(strInputSplit[0], 16);
                    if (strInputSplit.Length > 1)
                    {
                        byteOut[1] = Convert.ToByte(strInputSplit[1], 16);
                    }
                    if (strInputSplit.Length > 2)
                    {
                        byteOut[2] = Convert.ToByte(strInputSplit[2], 16);
                    }
                    int mod = 1;
                    for (int i = 1; i < strInputSplit.Length; i++)
                    {
                        mod *= 256;
                    }

                    intout += byteOut[0] * mod;
                    intout += byteOut[1] * mod / 256;
                    intout += byteOut[2] * mod / 65536;

                    if (negative)
                    {
                        intout = 0 - intout;
                    }
                }


            }
            else
            {
                byteOut[0] = 0;
            }

            return intout;
        }
       

    }
}
