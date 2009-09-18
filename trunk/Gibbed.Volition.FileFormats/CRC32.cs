using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace Gibbed.Volition.FileFormats
{
	public class CRC32 : HashAlgorithm
	{
		protected uint AllOnes;
		protected static Hashtable CachedCRC32Tables;
		protected static bool _AutoCache;
	
		protected uint[] CRC32Table; 
		private uint CRC;
		
		/// <summary>
		/// Returns the default polynomial (used in WinZip, Ethernet, etc)
		/// </summary>
		public static uint DefaultPolynomial
		{
			get { return 0xedb88320; }
		}

		/// <summary>
		/// Gets or sets the auto-cache setting of this class.
		/// </summary>
		public static bool AutoCache
		{
			get { return _AutoCache; }
			set { _AutoCache = value; }
		}

		public static uint DefaultAllOnes { get { return 0xffffffff; } }

		/// <summary>
		/// Initialize the cache
		/// </summary>
		static CRC32()
		{
			CachedCRC32Tables = Hashtable.Synchronized(new Hashtable());
			_AutoCache = true;
		}

		public static void ClearCache()
		{
			CachedCRC32Tables.Clear();
		}

		/// <summary>
		/// Builds a crc32 table given a polynomial
		/// </summary>
		/// <param name="polynomial"></param>
		/// <returns></returns>
		protected static uint[] BuildCRC32Table(uint polynomial)
		{
			uint crc;
			uint[] table = new uint[256];

			// 256 values representing ASCII character codes. 
			for (uint i = 0; i < 256; i++)
			{
				crc = i;
				for (int j = 8; j > 0; j--)
				{
					if((crc & 1) == 1)
						crc = (crc >> 1) ^ polynomial;
					else
						crc >>= 1;
				}
				table[i] = crc;
			}

			return table;
		}
		
		/// <summary>
		/// Creates a CRC32 object using the DefaultPolynomial
		/// </summary>
		public CRC32() : this(DefaultPolynomial)
		{
		}

		/// <summary>
		/// Creates a CRC32 object using the specified Creates a CRC32 object 
		/// </summary>
		public CRC32(uint polynomial) : this(polynomial, CRC32.AutoCache)
		{
		}
		
		/// <summary>
		/// Construct the 
		/// </summary>
		public CRC32(uint polymonial, bool cacheTable)
		{
			this.AllOnes = DefaultAllOnes;
			this.HashSizeValue = 32;

			this.CRC32Table = (uint[])CachedCRC32Tables[polymonial];
			if (this.CRC32Table == null)
			{
				this.CRC32Table = CRC32.BuildCRC32Table(polymonial);
				
				if (cacheTable)
				{
					CachedCRC32Tables.Add(polymonial, this.CRC32Table);
				}
			}

			this.Initialize();
		}
	
		/// <summary>
		/// Initializes an implementation of HashAlgorithm.
		/// </summary>
		public override void Initialize()
		{
			this.CRC = this.AllOnes;
		}
	
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		protected override void HashCore(byte[] buffer, int offset, int count)
		{
			// Save the text in the buffer. 
			for (int i = offset; i < count; i++)
			{
				ulong tabPtr = (this.CRC & 0xFF) ^ buffer[i];
				this.CRC >>= 8;
				this.CRC ^= this.CRC32Table[tabPtr];
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override byte[] HashFinal()
		{
			return BitConverter.GetBytes(this.CRC ^ AllOnes);
		}
	
		/// <summary>
		/// Computes the hash value for the specified Stream.
		/// </summary>
		new public byte[] ComputeHash(Stream inputStream)
		{
			byte[] buffer = new byte[4096];
			int bytesRead;
			while ((bytesRead = inputStream.Read(buffer, 0, 4096)) > 0)
			{
				this.HashCore(buffer, 0, bytesRead);
			}

			return this.HashFinal();
		}


		/// <summary>
		/// Overloaded. Computes the hash value for the input data.
		/// </summary>
		new public byte[] ComputeHash(byte[] buffer)
		{
			return this.ComputeHash(buffer, 0, buffer.Length);
		}
	
		/// <summary>
		/// Overloaded. Computes the hash value for the input data.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		new public byte[] ComputeHash( byte[] buffer, int offset, int count )
		{
			this.HashCore(buffer, offset, count);
			return this.HashFinal();
		}
	}

	public class BrokenCRC32 : CRC32
	{
		public override void Initialize()
		{
			this.AllOnes = 0;
			base.Initialize();
		}
	}
}
