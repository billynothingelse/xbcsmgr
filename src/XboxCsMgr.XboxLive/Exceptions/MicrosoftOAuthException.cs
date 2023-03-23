using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XboxCsMgr.XboxLive.Exceptions
{
    public class MicrosoftOAuthException : Exception
    {
        public MicrosoftOAuthException()
        {

        }

        public MicrosoftOAuthException(string message, int statusCode) : base(message)
            => StatusCode = statusCode;

        public MicrosoftOAuthException(string error, string errorDes, int[] codes, int statusCode) : base(CreateMessageFromError(error, errorDes))
            => (StatusCode, Error, ErrorDescription, ErrorCodes) = (statusCode, error, errorDes, codes);

        public int StatusCode { get; private set; }

        public string Error { get; private set; }

        public string ErrorDescription { get; private set; }

        public int[] ErrorCodes { get; private set; }

        private static string CreateMessageFromError(string error, string errorDes)
        {
            return string.Join(", ", new string[] { error, errorDes }.Where(x => !string.IsNullOrEmpty(x)));
        }

        public static MicrosoftOAuthException FromResponseBody(string responseBody, int statusCode)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                string error = null;
                string errorDes = null;
                int[] errorCodes = null;

                if (root.TryGetProperty("error", out var errorProp) &&
                    errorProp.ValueKind == JsonValueKind.String)
                    error = errorProp.GetString();
                if (root.TryGetProperty("error_description", out var errorDesProp) &&
                    errorDesProp.ValueKind == JsonValueKind.String)
                    errorDes = errorDesProp.GetString();
                if (root.TryGetProperty("error_codes", out var errorCodesProp) &&
                    errorCodesProp.ValueKind == JsonValueKind.Array)
                {
                    errorCodes = errorCodesProp
                        .EnumerateArray()
                        .Where(x => x.ValueKind == JsonValueKind.Number)
                        .Select(x => x.GetInt32())
                        .ToArray();
                }

                if (string.IsNullOrEmpty(error) && string.IsNullOrEmpty(errorDes))
                    throw new FormatException();

                return new MicrosoftOAuthException(error, errorDes, errorCodes, statusCode);
            }
            catch (JsonException)
            {
                throw new FormatException();
            }
        }
    }
}