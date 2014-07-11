/*
 * Created by SharpDevelop.
 * User: Glain
 * Date: 3/25/2012
 * Time: 12:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace FFTorgASM
{
	using System;
	using System.ComponentModel;
	using System.Windows.Forms;
	
	public class HexNumericUpDown : NumericUpDown 
	{
		public HexNumericUpDown()
		{
			base.Hexadecimal = true;
			base.Minimum = 0;
			base.Maximum = uint.MaxValue;
		}
		  
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new decimal Maximum 				// Doesn't serialize properly
		{
		    get { return base.Maximum; }
		    set { base.Maximum = value; }
		}
		
		protected override void UpdateEditText() 
		{
		    if (base.UserEdit) 
		   		HexParseEditText();
		    
		    if (!string.IsNullOrEmpty(base.Text)) 
		    {
				base.ChangingText = true;
				base.Text = string.Format("{0:X}", (uint)base.Value);
		    }
		}
		
		protected override void ValidateEditText() 
		{
		    HexParseEditText();
		    UpdateEditText();
		}
		
		private void HexParseEditText() 
		{
			try 
		    {
				Int64 val = Convert.ToInt64(base.Text, 16);
				
				if (val > Maximum)
		    		base.Text = string.Format("{0:X}", (uint)Maximum);
				
		    	if (!string.IsNullOrEmpty(base.Text))
		   			this.Value = val;
		    }
		    catch { }
		    finally 
		    {
		    	base.UserEdit = false;
		    }
		}
	}
}
