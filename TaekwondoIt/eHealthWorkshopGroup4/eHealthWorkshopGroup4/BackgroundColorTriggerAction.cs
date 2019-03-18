using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace eHealthWorkshopGroup4
{
    class BackgroundColorTriggerAction : TriggerAction<VisualElement>
    {
        public Color BackgroundColor { get; set; }

        protected override void Invoke(VisualElement visual)
        {
            var button = visual as Button;
            if (button == null) return;
            if (BackgroundColor != null) button.BackgroundColor = BackgroundColor;
        }
    }
}
