using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PUELicencia_Huella
{
    public partial class Mensaje : Form
    {
        public string mensaje { get; set; }
        public Mensaje()
        {
            InitializeComponent();
        }

        private void Mensaje_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Datos d = new Datos();
            List<string> causas = d.DatosCausas();
            for (int i = 0; i < causas.Count(); i++)
            {
                cbcausas.Items.Add(causas[i]);
            }


        }

        private void btnconfirmar_Click(object sender, EventArgs e)
        {
            if (cbcausas.Text != "Seleccione una opción.")
            {
                mensaje = cbcausas.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Opción no valida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
