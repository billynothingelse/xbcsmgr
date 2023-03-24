using System;
using System.Text;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using XboxCsMgr.XboxLive.Model.Authentication;

namespace XboxCsMgr.XboxLive
{
    public class XboxLiveSecurity
    {
        internal readonly AsymmetricCipherKeyPair keyPair;
        internal readonly ECPublicKeyParameters publicKey;
        internal readonly ECPrivateKeyParameters privateKey;

        private ProofKeyProperties? proofKey;
        public ProofKeyProperties ProofKey
        {
            get
            {
                if (proofKey == null)
                    proofKey = CreateProofKey();
                return proofKey;
            }
        }

        public XboxLiveSecurity()
        {
            keyPair = GenerateKeyPair();
            publicKey = (ECPublicKeyParameters)keyPair.Public;
            privateKey = (ECPrivateKeyParameters)keyPair.Private;
        }

        private static AsymmetricCipherKeyPair GenerateKeyPair()
        {
            var curve = NistNamedCurves.GetByName("P-256");
            var keyGenerationParameters = new ECKeyGenerationParameters(new ECDomainParameters(curve), new SecureRandom());
            var keyGen = new ECKeyPairGenerator("ECDSA");
            keyGen.Init(keyGenerationParameters);

            return keyGen.GenerateKeyPair();
        }

        private ProofKeyProperties CreateProofKey()
        {
            return new ProofKeyProperties
            {
                KeyType = "EC",
                Algorithm = "ES256",
                Use = "sig",
                Curve = "P-256",
                X = base64url(publicKey.Q.AffineXCoord.GetEncoded()),
                Y = base64url(publicKey.Q.AffineYCoord.GetEncoded())
            };
        }

        public string GenerateSignature(string reqUri, string token, string body)
        {
            var timestamp = GetWindowsTimestamp();
            var data = GeneratePayload(timestamp, reqUri, token, body);
            var signature = SignRequest(timestamp, data);
            return Convert.ToBase64String(signature);
        }

        private byte[] GeneratePayload(ulong windowsTimestamp, string uri, string token, string payload)
        {
            var pathAndQuery = new Uri(uri).PathAndQuery;

            var allocSize =
                4 + 1 +
                8 + 1 +
                4 + 1 +
                pathAndQuery.Length + 1 +
                token.Length + 1 +
                payload.Length + 1;
            var bytes = new byte[allocSize];

            var policyVersion = BitConverter.GetBytes((int)1);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(policyVersion);
            Array.Copy(policyVersion, 0, bytes, 0, 4);

            var windowsTimestampBytes = BitConverter.GetBytes(windowsTimestamp);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(windowsTimestampBytes);
            Array.Copy(windowsTimestampBytes, 0, bytes, 5, 8);

            var strs =
                $"POST\0" +
                $"{pathAndQuery}\0" +
                $"{token}\0" +
                $"{payload}\0";
            var strsBytes = Encoding.ASCII.GetBytes(strs);
            Array.Copy(strsBytes, 0, bytes, 14, strsBytes.Length);

            return bytes;
        }

        private byte[] SignRequest(ulong windowsTimestamp, byte[] bytes)
        {
            var signer = SignerUtilities.GetSigner("SHA256WITHPLAIN-ECDSA");
            signer.Init(true, privateKey);
            signer.BlockUpdate(bytes, 0, bytes.Length);
            var signature = signer.GenerateSignature();

            var policyVersion = BitConverter.GetBytes((int)1);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(policyVersion);

            var windowsTimestampBytes = BitConverter.GetBytes(windowsTimestamp);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(windowsTimestampBytes);

            var header = new byte[signature.Length + 12];
            Array.Copy(policyVersion, 0, header, 0, 4);
            Array.Copy(windowsTimestampBytes, 0, header, 4, 8);
            Array.Copy(signature, 0, header, 12, signature.Length);

            return header;
        }

        private ulong GetWindowsTimestamp()
        {
            var unixTimestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            ulong windowsTimestamp = (unixTimestamp + 11644473600u) * 10000000u;
            return windowsTimestamp;
        }

        private string base64url(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd(new char[] { '=' }).Replace('+', '-').Replace('/', '_');
        }
    }
}
