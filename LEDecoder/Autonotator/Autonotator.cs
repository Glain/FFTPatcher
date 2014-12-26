using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LEDecoder
{
    public partial class Autonotator : Form
    {
        MainForm Mainform;
        public Autonotator(MainForm LEDecoderForm)
        {
            InitializeComponent();
            Mainform = LEDecoderForm;
        }

        public static long StringToAddress(string InString)
        {
            if (InString.Contains("0x"))
            {
                InString = InString.Replace("0x", "");
            }
            long intout = 0;
            byte[] byteOut = new byte[3];
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

            }
            else
            {
                byteOut[0] = 0;
            }

            return intout;
        }
        
        public void GetRegisterDescriptions()
        {
            foreach(Register reg in Mainform.Registers)
            {
                reg.GetDescription(Mainform);
            }
        }
        public void GetRegisterValues()
        {
            Mainform.Registers[1].Value = (int)StringToAddress(txt_Register1.Text);
            Mainform.Registers[2].Value = (int)StringToAddress(txt_Register2.Text);
            Mainform.Registers[3].Value = (int)StringToAddress(txt_Register3.Text);
            Mainform.Registers[4].Value = (int)StringToAddress(txt_Register4.Text);
            Mainform.Registers[5].Value = (int)StringToAddress(txt_Register5.Text);
            Mainform.Registers[6].Value = (int)StringToAddress(txt_Register6.Text);
            Mainform.Registers[7].Value = (int)StringToAddress(txt_Register7.Text);
            Mainform.Registers[8].Value = (int)StringToAddress(txt_Register8.Text);
            Mainform.Registers[9].Value = (int)StringToAddress(txt_Register9.Text);
            Mainform.Registers[10].Value = (int)StringToAddress(txt_Register10.Text);
            Mainform.Registers[11].Value = (int)StringToAddress(txt_Register11.Text);
            Mainform.Registers[12].Value = (int)StringToAddress(txt_Register12.Text);
            Mainform.Registers[13].Value = (int)StringToAddress(txt_Register13.Text);
            Mainform.Registers[14].Value = (int)StringToAddress(txt_Register14.Text);
            Mainform.Registers[15].Value = (int)StringToAddress(txt_Register15.Text);
            Mainform.Registers[16].Value = (int)StringToAddress(txt_Register16.Text);
            Mainform.Registers[17].Value = (int)StringToAddress(txt_Register17.Text);
            Mainform.Registers[18].Value = (int)StringToAddress(txt_Register18.Text);
            Mainform.Registers[19].Value = (int)StringToAddress(txt_Register19.Text);
            Mainform.Registers[20].Value = (int)StringToAddress(txt_Register20.Text);
            Mainform.Registers[21].Value = (int)StringToAddress(txt_Register21.Text);
            Mainform.Registers[22].Value = (int)StringToAddress(txt_Register22.Text);
            Mainform.Registers[23].Value = (int)StringToAddress(txt_Register23.Text);
            Mainform.Registers[24].Value = (int)StringToAddress(txt_Register24.Text);
            Mainform.Registers[25].Value = (int)StringToAddress(txt_Register25.Text);
            Mainform.Registers[26].Value = (int)StringToAddress(txt_Register26.Text);
            Mainform.Registers[27].Value = (int)StringToAddress(txt_Register27.Text);
            Mainform.Registers[28].Value = (int)StringToAddress(txt_Register28.Text);
            Mainform.Registers[29].Value = (int)StringToAddress(txt_Register29.Text);
            Mainform.Registers[30].Value = (int)StringToAddress(txt_Register30.Text);
            Mainform.Registers[31].Value = (int)StringToAddress(txt_Register31.Text);
        }

        private void richTextBox_Enter(object sender, EventArgs e)
        {
            RichTextBox box = sender as RichTextBox;
            if (box.Text == "0x00000000")
            {
                box.Text = "0x";
            }

            
        }

        private void Autonotator_FormClosing(object sender, FormClosingEventArgs e)
        {
            GetRegisterValues();        //store user input register values
            GetRegisterDescriptions();  //Set register descriptions based on those values
        }
    }
}
