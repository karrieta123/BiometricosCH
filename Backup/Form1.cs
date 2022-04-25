/*
    This file is part of the Evolis SDK.

    The Evolis SDK is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    The Evolis SDK is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with the Evolis SDK.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace Cs_Example
{
	public partial class Form1 : Form
	{
		
		public Form1()
		{
			InitializeComponent();
		}

		//----------------------------------------
		// Declaration of the document to print
		//----------------------------------------
		
		private System.Drawing.Printing.PrintDocument docToPrint = new System.Drawing.Printing.PrintDocument();

		// The PrintDialog will print the document
		// by handling the document's PrintPage event.
		
		private void document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			//----------------------------------------
			// Add Text
			//----------------------------------------

			System.Drawing.Font printFont = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Regular);
			e.Graphics.DrawString(txtName.Text, printFont, System.Drawing.Brushes.Black, 20, 20);

			//----------------------------------------
			// Add a picture
			//----------------------------------------
			//
			// e.Graphics.DrawImage(Image.FromFile("imagefile.bmp")), e.Graphics.VisibleClipBounds);
			//
			
			//----------------------------------------
			// Magnetic Encoding
			//----------------------------------------
			//
			// We use the Pipe command whose syntax is 
			//
			//  |{TrackNumber}{Datas}|
			//
			//
			
			e.Graphics.DrawString("|1" + Track1.Text + "|", printFont, System.Drawing.Brushes.Black, 20, 20);
			e.Graphics.DrawString("|2" + Track2.Text + "|", printFont, System.Drawing.Brushes.Black, 20, 20);
			e.Graphics.DrawString("|3" + Track3.Text + "|", printFont, System.Drawing.Brushes.Black, 20, 20);
						
			e.HasMorePages = false;			
			

		}

		//----------------------------------------
		// Button Event
		//----------------------------------------
		

		private void button1_Click(object sender, EventArgs e)
		{
			printDialog1.Document = docToPrint;

			docToPrint.PrintPage += new PrintPageEventHandler(this.document_PrintPage);

			DialogResult result = printDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				docToPrint.Print();
			}
		}
	}
}