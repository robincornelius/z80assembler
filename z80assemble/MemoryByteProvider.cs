using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Be.Windows.Forms;

namespace z80assemble
{
    public class MemoryByteProvider : IByteProvider
    {

        Dictionary<long, byte> buffer = new Dictionary<long, byte>();

        public byte ReadByte(long index)
        { return buffer[index]; }

        /// <summary>
        /// Writes a byte into the provider
        /// </summary>
        /// <param name="index">the index of the byte to write</param>
        /// <param name="value">the byte to write</param>
        public void WriteByte(long index, byte value)
        {
            buffer[index] = value;
        }

        public void clear()
        {
            buffer = new Dictionary<long, byte>();
        }

        /// <summary>
        /// Inserts bytes into the provider
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bs"></param>
        /// <remarks>This method must raise the LengthChanged event.</remarks>
        public void InsertBytes(long index, byte[] bs) 
        {
            foreach (byte b in bs)
            {
                buffer[index++] = b;
            }

        }
        /// <summary>
        /// Deletes bytes from the provider
        /// </summary>
        /// <param name="index">the start index of the bytes to delete</param>
        /// <param name="length">the length of the bytes to delete</param>
        /// <remarks>This method must raise the LengthChanged event.</remarks>
        public void DeleteBytes(long index, long length) { }

        /// <summary>
        /// Returns the total length of bytes the byte provider is providing.
        /// </summary>
        public long Length { get{return buffer.Count;} }
    
        /// <summary>
        /// Occurs, when the Length property changed.
        /// </summary>
        public event EventHandler LengthChanged;

        /// <summary>
        /// True, when changes are done.
        /// </summary>
        public bool HasChanges() { return false; }
        /// <summary>
        /// Applies changes.
        /// </summary>
        public void ApplyChanges() { }
        /// <summary>
        /// Occurs, when bytes are changed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Returns a value if the WriteByte methods is supported by the provider.
        /// </summary>
        /// <returns>True, when it´s supported.</returns>
        public bool SupportsWriteByte() { return false; }
        /// <summary>
        /// Returns a value if the InsertBytes methods is supported by the provider.
        /// </summary>
        /// <returns>True, when it´s supported.</returns>
        public bool SupportsInsertBytes() { return false; }
        /// <summary>
        /// Returns a value if the DeleteBytes methods is supported by the provider.
        /// </summary>
        /// <returns>True, when it´s supported.</returns>
        public bool SupportsDeleteBytes() { return false; }

    }
}
