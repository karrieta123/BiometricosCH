namespace PUE.Controllers
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Biometrics : IDataErrorInfo, INotifyPropertyChanged
    {
        byte[] _Firma = null;
        byte[] _Foto = null;
        byte[] _Huella1 = null;
        byte[] _Huella2 = null;

        public string Error { get { throw new NotImplementedException(); } }
        public string this[string propertyName]
        {
            get
            {
                //string result = IsValid(propertyName);

                //if (result != string.Empty && propertyName != "NumeroDeAviso")
                //	Errors.Add(propertyName, result);
                return "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public byte[] Firma
        {
            get { return _Firma; }
            set { _Firma = value; NotifyPropertyChanged(); }
        }

        public byte[] Foto
        {
            get { return _Foto; }
            set { _Foto = value; NotifyPropertyChanged(); }
        }

        public byte[] Huella1
        {
            get { return _Huella1; }
            set { _Huella1 = value; NotifyPropertyChanged(); }
        }

        public byte[] Huella2
        {
            get { return _Huella2; }
            set { _Huella2 = value; NotifyPropertyChanged(); }
        }

        public String GuardarBiometricos()
        {
            //--Id Registro almacenado
            return "1";
        }
    }
}
