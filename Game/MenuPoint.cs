using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class MenuPoint
    {
        public string Text { get; set; }
        public bool On { get; set; }

        public event EventHandler Press;

        public MenuPoint(string text)
        {
            On = true;
            this.Text = text;
        }

        public void PressButton()
        {
            if (Press != null)
                Press(this, null);
        }
    }
}
