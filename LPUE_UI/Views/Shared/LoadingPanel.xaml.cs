namespace PUE.Views.Shared
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Lógica de interacción para LoadingPanel.xaml
    /// </summary>
    public partial class LoadingPanel : UserControl
    {
        public delegate void EventHandlerLoading(object sender, EventArgs e);
        public event EventHandlerLoading onClickClose;

        #region EVENTOS
        /// <summary>
        /// Titulo del mensaje
        /// </summary>
        public String Mensaje { set { this.txtMessage.Text = value; } }

        /// <summary>
        /// Detalles del mensaje
        /// </summary>
        public String SubMensaje { set { this.txtSubMessage.Text = value; } }

        /// <summary>
        /// Determina si se va a mostrar el spinner
        /// </summary>
        public Visibility ProgressBar { get; set; }

        /// <summary>
        /// Determina si el proceso generó un error
        /// </summary>
        public bool EsError { get; set; }
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingPanel"/> class.
        /// </summary>
        public LoadingPanel()
        {
            InitializeComponent();
        }
        #endregion

        #region EVENTOS
        /// <summary>
        /// Load del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.EsError)
            {
                this.txtMessage.Foreground = (Brush)((new BrushConverter()).ConvertFrom("#016EBF"));

                if (this.ProgressBar == System.Windows.Visibility.Visible)
                    this.progressBar.Visibility = System.Windows.Visibility.Visible;
                else
                    this.progressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.txtMessage.Foreground = (Brush)((new BrushConverter()).ConvertFrom("#FFCDD2"));
                this.progressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Cerrar control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.onClickClose(this, new EventArgs());
        }
        #endregion
    }
}