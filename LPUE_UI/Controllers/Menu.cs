using System;

namespace PUE.Controllers
{
    public class Menu
    {
        public enum ButtonType { Close, Minimize }
    }
}

public enum TipoResultado { OK, ERROR }


public class ComboInfo
{
    public String Text { get; set; }
    public String Value { get; set; }
}
