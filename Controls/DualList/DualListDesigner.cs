using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace FFTPatcher.Controls
{
    class DualListDesigner : ComponentDesigner
    {
        private const string buttonKey = "Button";
        private const string listBoxFromKey = "ListBoxFrom";
        private const string listBoxToKey = "ListBoxTo";
        public Button Button
        {
            get { return (Button)ShadowProperties[buttonKey]; }
            set { ShadowProperties[buttonKey] = value; }
        }

        public ListBox ListBoxFrom
        {
            get { return (ListBox)ShadowProperties[listBoxFromKey]; }
            set { ShadowProperties[listBoxFromKey] = value; }
        }

        public ListBox ListBoxTo
        {
            get { return (ListBox)ShadowProperties[listBoxToKey]; }
            set { ShadowProperties[listBoxToKey] = value; }
        }

        protected override void PreFilterProperties( System.Collections.IDictionary properties )
        {
            base.PreFilterProperties( properties );
            properties[buttonKey] = TypeDescriptor.CreateProperty(
                typeof( DualListDesigner ),
                (PropertyDescriptor)properties[buttonKey],
                new Attribute[0] );
            properties[listBoxFromKey] = TypeDescriptor.CreateProperty(
                typeof( DualListDesigner ),
                (PropertyDescriptor)properties[listBoxFromKey],
                new Attribute[0] );
            properties[listBoxToKey] = TypeDescriptor.CreateProperty(
                typeof( DualListDesigner ),
                (PropertyDescriptor)properties[listBoxToKey],
                new Attribute[0] );

        }
    }
}
