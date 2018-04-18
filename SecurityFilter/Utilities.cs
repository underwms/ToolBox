using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecurityFilter
{
   public static class Hash
   {
      public static byte Hash8(params object[] values)
      {
         return (byte)GetHash(56, values);
      }

      public static short Hash16(params object[] values)
      {
         return (short)GetHash(48, values);
      }

      public static int Hash24(params object[] values)
      {
         return (int)GetHash(40, values);
      }

      public static int Hash32(params object[] values)
      {
         return (int)GetHash(32, values);
      }

      public static long Hash40(params object[] values)
      {
         return GetHash(24, values);
      }

      public static long Hash48(params object[] values)
      {
         return GetHash(16, values);
      }

      public static long Hash56(params object[] values)
      {
         return GetHash(8, values);
      }

      public static long Hash64(params object[] values)
      {
         return GetHash(0, values);
      }

      public static long GetHash(int shift, params object[] values)
      {
         return values.Aggregate(0L, AddHash) >> shift;
      }

      private static long AddHash(long hash, object value)
      {
         unchecked
         {
            var nuhash = (value ?? 0).GetHashCode();
            var result = (31 * hash) + ToLong(nuhash);

            return result;
         }
      }

      private static long ToLong(int hash)
      {
         var bytes = BitConverter.GetBytes(hash);

         /* NOTE: this ordering of bytes is rather critical to shifting operations
          *       that succeed it.  alter with care.
          */
         var result = BitConverter.ToInt64(
            new[]
            {
               bytes[0], bytes[3],
               bytes[1], bytes[2],
               bytes[2], bytes[1],
               bytes[3], bytes[0]
            },
            0);

         return result;
      }
   }
}
