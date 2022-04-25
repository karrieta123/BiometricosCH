using System;
using System.Windows.Forms;

namespace Finger
{
    public partial class fingerless : Form
    {
        public string causa { get; set; }
        public fingerless()
        {
            InitializeComponent();
        }

        private void btnaceptar_Click(object sender, EventArgs e)
        {
            string valor = cbcausas.Text;
            if (valor != "")
            {
                causa = valor;
                this.Close();
            }
            else
            {
                MessageBox.Show("Selecciones una opción, por favor.", "Advertencia.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fingerless_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Datos d = new Datos();
            //cbcausas.DataSource = d.FillCausa();
            //cbcausas.ValueMember = "ID_DEDOS_CAUSA";
            //cbcausas.DisplayMember = "NOMBRE";


        }
    }
}
