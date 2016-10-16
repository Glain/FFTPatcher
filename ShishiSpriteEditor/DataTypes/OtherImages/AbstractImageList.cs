using System;
using System.Collections.Generic;
using System.Text;

namespace FFTPatcher.SpriteEditor.DataTypes.OtherImages
{
    public class AbstractImageList : IEnumerable<AbstractImage>
    {
        public string Name { get; set; }
        public List<AbstractImage> Images { get; set; }

        public AbstractImage this[int index]
        {
            get 
            {
                return Images[index];
            }
        }

        public int Count
        {
            get { return Images.Count; }
        }

        public override string ToString()
        {
            return Name;
        }

        public AbstractImageList()
        {
            Images = new List<AbstractImage>();
        }

        public IEnumerator<AbstractImage> GetEnumerator()
        {
            return Images.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
