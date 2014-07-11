using System;
using System.Collections.Generic;
using System.Text;

namespace FFTPatcher.Datatypes
{
    public abstract class BaseDataType : System.Xml.Serialization.IXmlSerializable
    {
        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema System.Xml.Serialization.IXmlSerializable.GetSchema()
        {
            return null;
        }

        protected abstract void ReadXml( System.Xml.XmlReader reader );

        protected abstract void WriteXml( System.Xml.XmlWriter writer );

        #endregion

        #region IXmlSerializable Members


        void System.Xml.Serialization.IXmlSerializable.ReadXml( System.Xml.XmlReader reader )
        {
            ReadXml( reader );
        }

        void System.Xml.Serialization.IXmlSerializable.WriteXml( System.Xml.XmlWriter writer )
        {
            WriteXml( writer );
        }

        #endregion
    }
}
