using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public class StringHash
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int HashIter = 10000;
        
        private readonly byte[] _salt;
        private readonly byte[] _hash;
        
        public byte[] Salt => (byte[])_salt.Clone();
        public byte[] Hash => (byte[])_hash.Clone();

        public StringHash(string password)
        {
            if (string.IsNullOrEmpty(password)) return;

            new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
            _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
        }

        public StringHash(byte[] hashBytes)
        {
            Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
        }

        public StringHash(byte[] salt, byte[] hash)
        {
            Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
        }
        
        public byte[] ToArray()
        {
            var hashBytes = new byte[SaltSize + HashSize];

            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);

            return hashBytes;
        }

        public bool Verify(string password)
        {
            var test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            for (var i = 0; i < HashSize; i++)
            {
                if (test[i] != _hash[i])
                { return false; }
            }

            return true;
        }
    }
}
