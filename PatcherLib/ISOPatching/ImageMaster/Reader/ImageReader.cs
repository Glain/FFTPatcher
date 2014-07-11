using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ImageMaster
{
    public abstract class ImageReader
    {
        public const int SECTOR_SIZE = 2048;
        public const int VIRTUAL_SECTOR_SIZE = 512;
        public const int PRIMARY_VOLUME_SECTOR = 16;
        public const int BOOTRECORD_VOLUME_SECTOR = 17;

        internal ImageRecord _rootDirectory;
        internal int CurrentBlockSize = SECTOR_SIZE;
        internal string _volumename;

        private string _filename;
        private Stream _stream;
        private long _archiveSize;
        private bool _cancel;

        public Stream BaseStream
        {
            get { return _stream; }
        }

        public ImageRecord RootDirectory
        {
            get { return _rootDirectory; }
        }

        public Collection<ImageRecord> Items
        {
            get { return _rootDirectory.SubItems; }
        }

        public ImageRecord GetItem( string name )
        {
            foreach ( ImageRecord record in _rootDirectory.SubItems )
            {
                if ( record.Name == name )
                    return record;
            }
            return null;
        }

        public ImageRecord GetItemPath( string relativePath )
        {
            string[] segments = relativePath.Split( new char[] { '/', '\\' }, 2, StringSplitOptions.RemoveEmptyEntries );
            ImageRecord child = GetItem( segments[0] );
            if ( segments.Length == 1 )
            {
                return child;
            }
            else
            {
                return child.GetItemPath( segments[1] );
            }
        }

        public ImageRecord GetItemRecursive( string name )
        {
            foreach ( ImageRecord item in _rootDirectory.SubItems )
            {
                if ( item.Name == name )
                {
                    return item;
                }
                else
                {
                    ImageRecord recurse = item.GetItemRecursive( name );
                    if ( recurse != null )
                    {
                        return recurse;
                    }
                }
            }

            return null;
        }

        public long Size
        {
            get { return _archiveSize; }
        }

        public bool CancelExtraction
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        public string FileName
        {
            get { return _filename; }
        }

        public string VolumeName
        {
            get { return _volumename; }
        }

        public void Initialize(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException();
            if (!File.Exists(path))
                throw new FileNotFoundException();
            _filename = path;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            Initialize(fs);
        }

        public void Initialize(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException();
            _stream = stream;
            _archiveSize = stream.Length;
        }

        public virtual bool Open()
        {
            if (this.BaseStream == null)
                throw new InvalidOperationException();
            return true;
        }

        protected virtual int ReadStream(byte[] buffer, int size)
        {
            int processedSize = 0;
            while (size != 0)
            {
                int curSize = (size < CurrentBlockSize) ? size : CurrentBlockSize;
                int bytesRead = 0;
                bytesRead = BaseStream.Read(buffer, 0, curSize);
                processedSize += bytesRead;
                size -= bytesRead;
                if (bytesRead == 0)
                    return processedSize;
            }
            return processedSize;
        }
    }
}