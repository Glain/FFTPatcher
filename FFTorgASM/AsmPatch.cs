using System;
using System.Collections.Generic;
using PatcherLib.Datatypes;

namespace FFTorgASM
{
    class FileAsmPatch : AsmPatch
    {
        private static System.Windows.Forms.OpenFileDialog ofd;

        private static System.Windows.Forms.OpenFileDialog OpenFileDialog
        {
            get
            {
                if ( ofd == null )
                {
                    ofd = new System.Windows.Forms.OpenFileDialog();
                    ofd.CheckFileExists = true;
                    ofd.CheckPathExists = true;
                    ofd.FileName = string.Empty;
                    ofd.Filter = "All files (*.*)|*.*";
                    ofd.Multiselect = false;
                    ofd.ShowHelp = false;
                    ofd.ShowReadOnly = true;
                }
                return ofd;
            }
                
        }

        private InputFilePatch patch;
        public FileAsmPatch( string name, string description, InputFilePatch patch )
            : base( name, description, new PatchedByteArray[] { patch }, new bool[] { false }, false )
        {
            this.patch = patch;
        }

        public void SetFilename( string filename )
        {
            patch.SetFilename( filename );
        }

        public override bool ValidatePatch()
        {
            bool result = false;
            System.Windows.Forms.MethodInvoker mi = delegate()
            {
                if ( !string.IsNullOrEmpty( patch.Filename ) )
                {
                    OpenFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName( patch.Filename );
                    OpenFileDialog.FileName = System.IO.Path.GetFileName( patch.Filename );
                }

                if ( OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    try
                    {
                        SetFilename( OpenFileDialog.FileName );
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            };

            mi();

            return result;
        }

    }

    public struct VariableType
    {
    	public KeyValuePair<string, List<PatchedByteArray>> content;
    	public char numBytes;
        public byte[] byteArray;
    }
    
    public class AsmPatch : IList<PatchedByteArray>
    {
        List<PatchedByteArray> innerList;
        List<PatchedByteArray> varInnerList;
        public List<bool> isDataSectionList;

        public string Name { get; private set; }
        public string Description { get; private set; }
        public IList<VariableType> Variables { get; private set; }

        private IEnumerator<PatchedByteArray> enumerator;

        public bool HideInDefault { get; private set; }

        public virtual bool ValidatePatch()
        {
            return true;
        }

        public AsmPatch( string name, string description, IEnumerable<PatchedByteArray> patches, IEnumerable<bool> isDataSectionList, bool hideInDefault )
        {
            enumerator = new AsmPatchEnumerator( this );
            this.Name = name;
            Description = description;
            innerList = new List<PatchedByteArray>( patches );
            Variables = new VariableType[0];
            varInnerList = new List<PatchedByteArray>();
            this.isDataSectionList = new List<bool>(isDataSectionList);
            this.HideInDefault = hideInDefault;
        }

        public AsmPatch(string name, string description, IEnumerable<PatchedByteArray> patches, IEnumerable<bool> isDataSectionList, bool hideInDefault, IList<VariableType> variables)
            : this( name, description, patches, isDataSectionList, hideInDefault )
        {
        	VariableType[] myVars = new VariableType[variables.Count];
            variables.CopyTo( myVars, 0 );
            Variables = myVars;
            SetVarInnerList();
        }

        private void SetVarInnerList()
        {
            varInnerList.Clear();
            foreach (VariableType varType in Variables)
            {
                List<PatchedByteArray> patchedByteArrayList = varType.content.Value;
                if (patchedByteArrayList != null)
                {
                    foreach (PatchedByteArray patchedByteArray in patchedByteArrayList)
                    {
                        varInnerList.Add(patchedByteArray);
                    }
                }
            }
        }

        public int IndexOf( PatchedByteArray item )
        {
            return innerList.IndexOf( item );
        }

        public void Insert( int index, PatchedByteArray item )
        {
            throw new NotImplementedException();
        }

        public void RemoveAt( int index )
        {
            throw new InvalidOperationException( "collection is readonly" );
        }

        public PatchedByteArray this[int index]
        {
            get
            {
                if ( index < innerList.Count )
                {
                    return innerList[index];
                }
                else
                {
                    return varInnerList[index - innerList.Count];
                }
            }
            set
            {
                throw new InvalidOperationException( "collection is readonly" );
            }
        }

        public void Add( PatchedByteArray item )
        {
            throw new InvalidOperationException( "collection is readonly" );
        }

        public void Clear()
        {
            throw new InvalidOperationException( "collection is readonly" );
        }

        public bool Contains( PatchedByteArray item )
        {
            return innerList.Contains( item );
        }

        public void CopyTo( PatchedByteArray[] array, int arrayIndex )
        {
            innerList.CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return innerList.Count + varInnerList.Count; }
        }

        public int NonVariableCount
        {
            get { return innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove( PatchedByteArray item )
        {
            throw new InvalidOperationException( "collection is readonly" );
        }

        public IEnumerator<PatchedByteArray> GetEnumerator()
        {
            enumerator.Reset();
            return enumerator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            enumerator.Reset();
            return enumerator as System.Collections.IEnumerator;
        }

        public override string ToString()
        {
            return Name;
        }
        
		// Returns combined value of byte array (little endian)
		public static UInt32 GetUnsignedByteArrayValue_LittleEndian(Byte[] bytes)
        {
			UInt32 result = 0;
			int i = 0;
			foreach (Byte currentByte in bytes)
			{
				result = result | ((uint)(currentByte << (i * 8)));
				i++;
			}
			return result;
        }

        private class AsmPatchEnumerator : IEnumerator<PatchedByteArray>
        {
            private int index = -1;
            private AsmPatch owner;
            public AsmPatchEnumerator( AsmPatch owner )
            {
                this.owner = owner;
            }
            #region IEnumerator<PatchedByteArray> Members

            public PatchedByteArray Current
            {
                get { return owner[index]; }
            }

            #endregion


            #region IDisposable Members

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                index++;
                return index < owner.Count;
            }

            public void Reset()
            {
                index = -1;
            }

            #endregion
        }
    }
}
