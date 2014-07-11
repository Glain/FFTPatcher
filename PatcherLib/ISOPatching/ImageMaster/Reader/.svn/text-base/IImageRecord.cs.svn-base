using System.Collections.ObjectModel;

namespace ImageMaster
{
    public interface IImageRecord
    {
        ImageRecord Parent
        {
            get;
        }

        Collection<ImageRecord> SubItems
        {
            get;
        }

        ImageRecord GetItem( string name );
        ImageRecord GetItemRecursive( string name );
        ImageRecord GetItemPath( string relativePath );

        long Location
        {
            get;
        }

        long Size
        {
            get;
        }

        bool IsUDF
        {
            get;
        }

        bool IsDirectory
        {
            get;
        }

        bool IsSystemItem
        {
            get;
        }

        string Name
        {
            get;
        }

        string Path
        {
            get;
        }

        object Tag
        {
            get;
            set;
        }

        void Clear();
    }
}