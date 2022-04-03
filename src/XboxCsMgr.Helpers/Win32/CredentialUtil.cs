using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace XboxCsMgr.Helpers.Win32
{
    public static class CredentialUtil
    {
        [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool CredEnumerate(string filter, int flag, out int count, out IntPtr pCredentials);

        public enum CredentialType
        {
            Generic = 1,
            DomainPassword,
            DomainCertificate,
            DomainVisiblePassword,
            GenericCertificate,
            DomainExtended,
            Maximum,
            MaximumEx = Maximum + 1000,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDENTIAL
        {
            public uint Flags;
            public CredentialType Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public uint CredentialBlobSize;
            public IntPtr CredentialBlob;
            public uint Persist;
            public uint AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;
        }

        /// <summary>
        /// Enumerates the current available credentials via the wincred API
        /// </summary>
        /// <returns>TargetName (K) and CredentialBlob (V)</returns>
        public static Dictionary<string, string> EnumerateCredentials()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            int count;
            IntPtr credentials;

            if (CredEnumerate(null, 0, out count, out credentials))
            {
                for (int i = 0; i < count; i++)
                {
                    IntPtr cred = Marshal.ReadIntPtr(credentials, i * Marshal.SizeOf(typeof(IntPtr)));
                    CREDENTIAL c = (CREDENTIAL)Marshal.PtrToStructure(cred, typeof(CREDENTIAL));
                    if (c.CredentialBlob != IntPtr.Zero)
                        result.Add(Marshal.PtrToStringUni(c.TargetName), Marshal.PtrToStringAnsi(c.CredentialBlob));
                }

                return result;
            }

            return null;
        }
    }
}
