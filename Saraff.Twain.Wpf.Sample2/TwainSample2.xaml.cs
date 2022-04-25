using Entidades;
using Saraff.Twain.Aux;
using Saraff.Twain.Wpf.Sample2.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Saraff.Twain.Wpf.Sample2
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class TwainSample2 : Window
    {
        private static int cont_img = 1;
        private WriteableBitmap imge;
        private List<EntiDocumentos> addDocs = new List<EntiDocumentos>();
        private List<EntiDocumentosR> addDocsRespaldo = new List<EntiDocumentosR>();

        private Propiedades propi2 = new Propiedades();
        private int img { get; set; }
        private bool _isLoading = true;

        private int id_documento = 0;
        private string N_combo { get; set; }
        List<string> DocumentosRemove = new List<string>();
        public static List<ImageDocs> listDocuments = new List<ImageDocs>();
        private int itemsInitial = 0;
        public TwainSample2()
        {
            InitializeComponent();
        }

        #region Agregar y quitar elementos de la lista
        private void RemoveElement(EntiDocumentos d)
        {
            cmbdocumentos.ItemsSource = null;
            //DocumentosRemove.Remove(d.nombre);
            //addDocs = propi2.DocumentosV();
            addDocs.RemoveAll(r => r.ID_DOCUMENTO == d.ID_DOCUMENTO);
            cmbdocumentos.ItemsSource = addDocs;
        }
        private void AddElement(string d)
        {
            d = d.Replace("_", " ");
            cmbdocumentos.ItemsSource = null;
            DocumentosRemove.Add(d);
            //foreach (var item in DocumentosRemove)
            //{
            var res = addDocsRespaldo.Find(f => f.nombre == d);
            addDocs.Add(new EntiDocumentos { ID_DOCUMENTO = res.id, NOMBRE = res.nombre });
            //}
            //addDocs = propi2.DocumentosV(DocumentosRemove);

            //var rr = addDocsRespaldo.Find(f => f.nombre == d);
            cmbdocumentos.ItemsSource = addDocs;
        }
        #endregion

        #region Methods
        //Identifica los Dispositivos que se encuentran conectados
        private void _Load()
        {
            addDocs = propi2.DocumentosV();
            foreach (var item in addDocs)
            {
                addDocsRespaldo.Add(new EntiDocumentosR { id = item.ID_DOCUMENTO, nombre = item.NOMBRE });
            }
            itemsInitial = addDocs.Count;
            foreach (var item in addDocs)
            {
                DocumentosRemove.Add(item.NOMBRE);
            }
            cmbdocumentos.ItemsSource = addDocs;
            this._XferMech.Source = new string[0];
            var _sources = this._GetSources();
            //var colection = new Collection<Source>();
            List<object> listnew = new List<object>();
            foreach (var scan in _sources)
            {
                if (scan.Visual == "[1.x; x86]: HP ScanJet Pro 2500 f1 TWAIN")
                {
                    listnew.Add(scan);
                }
            }

            this._Sources.Source = listnew;

            for (var i = 0; i < listnew.Count; i++)
            {
                //if (listnew[i])
                //{

                this._Sources.View.MoveCurrentToPosition(0);
                //}
            }

            this._Sources.View.CurrentChanged += this._DataSourceCurrentChanged;

            this._isLoading = false;
            if (this._Sources.View != null && !this._Sources.View.IsEmpty)
            {
                this._DeviceChanged();
            }
        }

        private void _DeviceChanged()
        {
            //this._GetCaps(this._CurrentDataSource);// este metodo nos sirve para verificar si esta conectado

            this._XferMech.View.CurrentChanged += this._XferMechCurrentChanged;
            this._SetVisibilityImageFileFormExpander();
        }

        private Collection<Source> _GetSources()
        {
            var _result = new Collection<Source>();
            foreach (var _host in new string[] { Source.x86Aux, Source.msilAux })
            {
                TwainExternalProcess.Execute(
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), _host),
                    twain =>
                    {
                        try
                        {
                            if (_host == Source.msilAux && !twain.IsTwain2Supported)
                            {
                                return;
                            }
                            for (var i = 0; i < twain.SourcesCount; i++)
                            {
                                _result.Add(new Source
                                {
                                    Id = i,
                                    Name = twain.GetSourceProductName(i),
                                    IsX64Platform = _host == Source.x86Aux ? false : Environment.Is64BitOperatingSystem,
                                    IsTwain2 = twain.IsTwain2Supported,
                                    IsDefault = twain.SourceIndex == i
                                });
                            }
                        }
                        catch
                        {
                        }
                    });
            }
            return _result;
        }

        private void _GetCaps(Source source)
        {

            TwainExternalProcess.Execute(
                System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), source.ExecFileName),
                twain =>
                {
                    try
                    {
                        twain.SourceIndex = source.Id;
                        twain.OpenDataSource();
                    }
                    catch
                    {
                    }
                });
        }

        private void _Acquire()
        {

            TwainExternalProcess.Execute(
                System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), this._CurrentDataSource.ExecFileName),
                twain =>
                {
                    #region Memory

                    #region SetupMemXferEvent

                    twain.SetupMemXferEvent += (sender, e) =>
                    {
                        try
                        {
                            System.Windows.Media.PixelFormat _format = PixelFormats.Default;
                            BitmapPalette _pallete = null;
                            switch (e.ImageInfo.PixelType)
                            {
                                case TwPixelType.BW:
                                    _format = PixelFormats.BlackWhite;
                                    break;
                                case TwPixelType.Gray:
                                    _format = new Dictionary<short, System.Windows.Media.PixelFormat> {
                                        {2,PixelFormats.Gray2},
                                        {4,PixelFormats.Gray4},
                                        {8,PixelFormats.Gray8},
                                        {16,PixelFormats.Gray16}
                                    }[e.ImageInfo.BitsPerPixel];
                                    break;
                                case TwPixelType.Palette:
                                    _pallete = new BitmapPalette(new Func<IList<System.Windows.Media.Color>>(() =>
                                    {
                                        var _res = new Collection<System.Windows.Media.Color>();
                                        var _colors = twain.Palette.Get().Colors;
                                        for (int i = 0; i < _colors.Length; i++)
                                        {
                                            _res.Add(System.Windows.Media.Color.FromArgb(_colors[i].A, _colors[i].R, _colors[i].G, _colors[i].B));
                                        }
                                        return _res;
                                    })());
                                    _format = new Dictionary<short, System.Windows.Media.PixelFormat> {
                                        {2,PixelFormats.Indexed1},
                                        {4,PixelFormats.Indexed2},
                                        {8,PixelFormats.Indexed4},
                                        {16,PixelFormats.Indexed8}
                                    }[e.ImageInfo.BitsPerPixel];
                                    break;
                                case TwPixelType.RGB:
                                    _format = new Dictionary<short, System.Windows.Media.PixelFormat> {
                                        {8,PixelFormats.Rgb24},
                                        {24,PixelFormats.Rgb24},
                                        {16,PixelFormats.Rgb48},
                                        {48,PixelFormats.Rgb48}
                                    }[e.ImageInfo.BitsPerPixel];
                                    break;
                                default:
                                    throw new InvalidOperationException("Este formato de píxel no es compatible.");
                            }

                            this.Dispatcher.BeginInvoke(
                                new Action(() =>
                                {
                                    try
                                    {
                                        imge = new WriteableBitmap(
                                          e.ImageInfo.ImageWidth,
                                          e.ImageInfo.ImageLength,
                                          e.ImageInfo.XResolution,
                                          e.ImageInfo.YResolution,
                                          _format,
                                          _pallete);
                                        //listDocuments.Add(new ImageDocs { id = id_documento, imagen = BitmapFromWriteableBitmap(imge) });
                                        //BitmapFromWriteableBitmap
                                        this.scanImage.Source = imge;
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.ErrorMessageBox();
                                    }
                                })
                            );

                        }
                        catch
                        {
                        }
                    };

                    #endregion

                    twain.MemXferEvent += (sender, e) =>
                    {
                        try
                        {
                            this.Dispatcher.BeginInvoke(
                                new Action(() =>
                                {
                                    try
                                    {
                                        if (this._CurrentDataSource.Visual == "[1.x; x86]: Canon DR-C225 TWAIN")
                                        {
                                            (this.scanImage.Source as WriteableBitmap).WritePixels(
                                                                                new Int32Rect(0, 0, 2480, (int)e.ImageMemXfer.Rows),
                                                                                e.ImageMemXfer.ImageData,
                                                                                (int)e.ImageMemXfer.BytesPerRow,
                                                                                (int)e.ImageMemXfer.XOffset,
                                                                                (int)e.ImageMemXfer.YOffset);
                                        }
                                        else
                                        {
                                            (this.scanImage.Source as WriteableBitmap).WritePixels(
                                                                               new Int32Rect(0, 0, (int)e.ImageMemXfer.Columns, (int)e.ImageMemXfer.Rows),
                                                                               e.ImageMemXfer.ImageData,
                                                                               (int)e.ImageMemXfer.BytesPerRow,
                                                                               (int)e.ImageMemXfer.XOffset,
                                                                               (int)e.ImageMemXfer.YOffset);
                                        }
                                        BitmapFromWriteableBitmap(imge);


                                        // GetBmp() 
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.ErrorMessageBox();
                                    }
                                })
                            );

                        }
                        catch (Exception ex)
                        {
                        }

                    };

                    #endregion
                    #region Set Capabilities

                    // propi.JsonSettings = JsonSettings;
                    twain.SourceIndex = this._CurrentDataSource.Id;
                    twain.OpenDataSource();
                    twain.SetCap(TwCap.XResolution, propi2.resolucion);
                    //twain.SetCap(TwCap.YResolution, propi2.GetResolutions(JsonSettings));
                    //twain.SetCap(TwCap.IPixelType, propi2.Digitalizacion());
                    twain.SetCap(TwCap.IPixelType, propi2.Ptype);
                    twain.SetCap(TwCap.IXferMech, TwSX.Memory);
                    twain.SetCap(TwCap.ImageFileFormat, TwFF.Jfif);
                    twain.Capabilities.Indicators.Set(false);
                    if (propi2.sided)//aqui seria el codigo para identificar si quiere doble cara de la pagina
                    {
                        var _duplex = (ushort)twain.GetCap(TwCap.Duplex);
                        if (_duplex > 0)
                        {
                            if ((twain.IsCapSupported(TwCap.XferCount) & TwQC.GetCurrent) != 0)
                            {
                                twain.SetCap(TwCap.DuplexEnabled, true);
                            }
                        }
                    }

                    // deleteImage(ruta);

                    twain.Acquire();
                    twain.Dispose();
                    #endregion
                });

            //this.Dispatcher.DisableProcessing();
        }
        private void _SetVisibilityImageFileFormExpander()
        {
            //this.imageFileFormExpander.Visibility = (this.imageFileFormExpander.IsEnabled = ((TwSX)this._XferMech.View.CurrentItem) == TwSX.File) ? Visibility.Visible : Visibility.Hidden;
        }



        #region Properties

        private CollectionViewSource _Sources
        {
            get
            {
                return this.Resources["TwainSources"] as CollectionViewSource;
            }
        }
        //Obtenemos las resoluciones 150 200  300 400 500 600
        private CollectionViewSource _Resolutions
        {
            get
            {
                return this.Resources["Resolutions"] as CollectionViewSource;
            }
        }
        //Obtenemos los datos BW, RGB, gray
        private CollectionViewSource _PixelTypes
        {
            get
            {
                return this.Resources["PixelTypes"] as CollectionViewSource;
            }
        }

        private CollectionViewSource _XferMech
        {
            get
            {
                return this.Resources["XferMech"] as CollectionViewSource;
            }
        }

        private CollectionViewSource _ImageFileFormats
        {
            get
            {
                return this.Resources["ImageFileFormats"] as CollectionViewSource;
            }
        }

        private Source _CurrentDataSource
        {
            get
            {
                return (this.dataSourcesExpander.Content as ListBox).SelectedValue as Source;
            }
        }

        #endregion

        #region EventHandlers

        private void _DataSourceCurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this._isLoading)
                {
                    this._DeviceChanged();
                }
            }
            catch (Exception ex)
            {
                ex.ErrorMessageBox();
            }
        }

        private void _XferMechCurrentChanged(object sender, EventArgs e)
        {
            try
            {
                this._SetVisibilityImageFileFormExpander();
            }
            catch (Exception ex)
            {
                ex.ErrorMessageBox();
            }
        }

        private void _AcquireButtonClick(object sender, RoutedEventArgs e)
        {
            PB pb = new PB();
            try
            {
                EntiDocumentos d = (EntiDocumentos)cmbdocumentos.SelectedValue;
                if (d != null)
                {

                    pb.Show();
                    img = 0;
                    id_documento = d.ID_DOCUMENTO;
                    this.scanImage.Source = null;
                    this._Acquire();//metodo que empieza el evento del escaneo
                    string name = d.NOMBRE.Replace(" ", "_");
                    list.Items.Add(name + ", Ok");

                    RemoveElement(d);
                }
                else
                {
                    MessageBox.Show("Selecciones un opción por favor.", "Advertencia", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                pb.Close();
            }
        }



        private void _ByWidthButtonChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.scanImage.Stretch = Stretch.UniformToFill;
                this.scrol.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                this.scrol.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            catch
            {

            }
        }

        private void _ByHeightButtonChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.scanImage.Stretch = Stretch.Uniform;
                this.scrol.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                this.scrol.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
            catch
            {

            }
        }

        private void _DefaultButtonChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.scanImage.Stretch = Stretch.None;
                this.scrol.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                this.scrol.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            catch (Exception ex)
            {
                ex.ErrorMessageBox();
            }
        }

        private void _BySizeButtonChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.scanImage.Stretch = Stretch.Fill;
                this.scrol.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                this.scrol.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
            catch
            {

            }
        }

        private void _WindowLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this._Load();
            }
            catch (Exception EX)
            {

            }
        }

        #endregion

        #region Propiedades
        private class Source
        {
            public const string x86Aux = "Saraff.Twain.Aux_x86.exe";
            public const string msilAux = "Saraff.Twain.Aux_MSIL.exe";

            public static readonly DependencyProperty CurrentProperty;

            static Source()
            {
                Source.CurrentProperty = DependencyProperty.RegisterAttached("Current", typeof(Source), typeof(Source), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
            }

            public string Visual
            {
                get
                {
                    return this.ToString();
                }
            }

            public int Id
            {
                get;
                set;
            }

            public string ExecFileName
            {
                get
                {
                    return !this.IsX64Platform && !this.IsTwain2 ? Source.x86Aux : Source.msilAux;
                }
            }

            public string Name
            {
                get;
                set;
            }

            public bool IsX64Platform
            {
                get;
                set;
            }

            public bool IsTwain2
            {
                get;
                set;
            }

            public bool IsDefault
            {
                get;
                set;
            }

            public override bool Equals(object obj)
            {
                for (var _val = obj as Source; _val != null;)
                {
                    return _val.IsX64Platform == this.IsX64Platform && _val.IsTwain2 == this.IsTwain2 && _val.Id == this.Id;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return this.Id.GetHashCode();
            }

            public override string ToString()
            {
                return String.Format("[{0}.x; {1}]: {2}", this.IsTwain2 ? "2" : "1", this.IsX64Platform ? "x64" : "x86", this.Name);
            }
        }
        #endregion

        #region Metodo que detecta el doble click a la lista y la devuelve al ComboBox
        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;
            while (obj != null && obj != list)
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    //EntiDocumentos doc = (EntiDocumentos)list.SelectedItem;
                    AddElement(list.SelectedItem.ToString().Replace(", Ok", ""));
                    list.Items.Remove(list.SelectedItem);
                    listDocuments.RemoveAll(t => t.id == id_documento);
                    this.scanImage.Source = null;
                    //string ruta = System.IO.Path.Combine(System.IO.Path.GetTempPath(), id_documento + "_" + img + ".jpeg");
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }
        #endregion

        #region Boton que agrega al combobox
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (list.Items.Count > 0)
            {
                var sele = list.SelectedItem.ToString();
                AddElement(list.SelectedItem.ToString().Replace(", Ok", ""));
                list.Items.Remove(list.SelectedItem);
            }

            scanImage.Source = null;
        }
        #endregion

        #region Boton que cierra la forma
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var vari = Window.GetWindow(this);
            vari.Close();
            //vari.Close();
        }
        #endregion

        #region Metodo para convertir en Bitmap
        public System.Drawing.Image ConvertToBitmap(string fileName)
        {
            System.Drawing.Image image = null;
            Bitmap bitmap = new Bitmap(fileName);
            image = (System.Drawing.Image)bitmap;
            return image;
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        #endregion

        private void BitmapFromWriteableBitmap(WriteableBitmap writeBmp)
        {
            System.Drawing.Bitmap bmp;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();

                enc.Frames.Add(BitmapFrame.Create((BitmapSource)writeBmp));

                enc.Save(outStream);
                bmp = new System.Drawing.Bitmap(outStream);
                bmp.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\scanner_" + id_documento + ".jpeg");
            }
            bmp.Dispose();
        }
        private void WriteWriteableBitmap(WriteableBitmap bmp)
        {
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "scanner.jpeg");
            using (FileStream stream = new FileStream(ruta, FileMode.Create, FileAccess.Write))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(stream);
            }
        }
        private System.Drawing.Image GetBmp()
        {
            Bitmap bmp = new Bitmap(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "scanner.jpeg"));
            return bmp;
        }

    }
}