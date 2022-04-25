using DataPUE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUE.Views.Shared
{
    public partial class Busqueda : Form
    {
        EntitieLocal _context;

        public Busqueda()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            using (_context = new EntitieLocal())
            {
                var cIUDADANO = _context.CIUDADANO.Where(x => x.CURP == txt_Curp.Text).FirstOrDefault();

                var y = cIUDADANO;
            }
        }
    }
}
